using System;
using System.IO;
using Unity3DDisassembler.Common;
using Unity3DDisassembler.Unity;

namespace Unity3DDisassembler.Unity
{
    public class DecompressedFile
    {
        /// <summary>
        /// Initialize and disassemble the file straight away
        /// </summary>
        /// <param name="file">Decompressed file bytes</param>
        public DecompressedFile(byte[] file)
        {
            _Stream = new DataStream(file);
            _Header = new DecompressedFileHeader();

            _Bytes = file;
            _Size = file.Length;

            ParseHeader();
            GetHeaderBytes();
            GetFiles();

            ConsoleIO.Log("Decompressed file size: " + _Bytes.Length);
            ConsoleIO.Log("Decompressed file header size: " + _Header.Bytes.Length);
            ConsoleIO.Log("Files: ");
            for (int i = 0; i < _Header.Files.Length; i++)
            {
                ConsoleIO.Log("Name: \"" + _Header.Files[i].Name + "\" Size: " + _Header.Files[i].Size + " Offset: " + _Header.Files[i].Offset);
            }
        }

        private int _Size;
        private int _FileCount;
        private byte[] _Bytes;
        private DataStream _Stream;
        private DecompressedFileHeader _Header;

        public int Size { get { return _Size; } }
        public int FileCount { get { return _FileCount; } }
        public byte[] Bytes { get { return _Bytes; } }
        public DataStream Stream { get { return _Stream; } }
        public DecompressedFileHeader Header { get { return _Header; } }

        public void ParseHeader()
        {
            _FileCount = _Stream.ReadInt();
            FileObject[] files = new FileObject[_FileCount];

            for (int i = 0; i < _FileCount; i++)
            {
                files[i] = new FileObject();
                files[i].Name = _Stream.ReadString();
                files[i].Offset = _Stream.ReadInt();
                files[i].Size = _Stream.ReadInt();
            }
            _Header.Files = files;
        }

        public void GetHeaderBytes()
        {
            _Header.Bytes = new byte[_Header.Files[0].Offset];
            Buffer.BlockCopy(_Bytes, 0, _Header.Bytes, 0, _Header.Files[0].Offset);
        }

        public void GetFiles()
        {
            for (int i = 0; i < _FileCount; i++)
            {
                _Header.Files[i].Bytes = new byte[_Header.Files[i].Size];
                _Stream.MainStream.Position = _Header.Files[i].Offset;
                _Header.Files[i].Bytes = _Stream.ReadByteArray(_Header.Files[i].Bytes.Length);
            }
        }

        public void ExtractFiles(string path)
        {
            for (int i = 0; i < _FileCount; i++)
            {
                _Header.Files[i].WriteFile(path);
            }
        }
    }
}
