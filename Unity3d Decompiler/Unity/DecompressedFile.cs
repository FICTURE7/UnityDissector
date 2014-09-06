using System;
using System.IO;
using Unity3dDecompiler.Unity;

namespace Unity3dDecompiler.Unity
{
    public class DecompressedFile
    {
        public DecompressedFile(byte[] file)
        {
            _Stream = new DataStream(file);

            _Bytes = file;
            _Size = file.Length;

            ParseHeader();
            GetHeaderBytes();
            GetFiles();

            ConsoleIO.Log("Decompressed file size: " + _Bytes.Length);
            ConsoleIO.Log("Decompressed file header size: " + _HeaderBytes.Length);
            ConsoleIO.Log("Files: ");
            for (int i = 0; i < _Files.Length; i++)
            {
                ConsoleIO.Log("Name: \"" + _Files[i].Name + "\" Size: " + _Files[i].Size + " Offset: " + _Files[i].Offset);
            }
        }

        private int _Size;
        private int _FileCount;
        private byte[] _HeaderBytes;
        private byte[] _Bytes;
        private DataStream _Stream;
        private FileObject[] _Files;

        public int Size { get { return _Size; } }
        public int FileCount { get { return _FileCount; } }
        public byte[] HeaderBytes { get { return _HeaderBytes; } }
        public byte[] Bytes { get { return _Bytes; } }
        public DataStream Stream { get { return _Stream; } }
        public FileObject[] Files { get { return _Files; } }

        public void GetHeaderBytes()
        {
            _HeaderBytes = new byte[_Files[0].Offset];
            Buffer.BlockCopy(_Bytes, 0, _HeaderBytes, 0, _Files[0].Offset);
        }

        public void GetFiles()
        {
            for (int i = 0; i < _FileCount; i++)
            {
                _Files[i].Bytes = new byte[_Files[i].Size];
                _Stream.MainStream.Position = _Files[i].Offset;
                _Files[i].Bytes = _Stream.ReadByteArray(_Files[i].Bytes.Length);
            }
        }

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
            _Files = files;
        }

        public void ExtractFiles(string path)
        {
            for (int i = 0; i < _FileCount; i++)
            {
                _Files[i].WriteFile(path);
            }
        }
    }
}
