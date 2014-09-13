using System;
using System.IO;
using Unity3DDisassembler.IO;
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
            _FileReader = new FileReader(file);
            _Header = new DecompressedFileHeader();

            _Bytes = file;
            _Size = file.Length;

            ParseHeader();
            GetHeaderBytes();
            GetFiles();
        }

        private int _Size;
        private int _FileCount;
        private byte[] _Bytes;
        private FileReader _FileReader;
        private DecompressedFileHeader _Header;

        public int Size { get { return _Size; } }
        public int FileCount { get { return _FileCount; } }
        public byte[] Bytes { get { return _Bytes; } }
        public FileReader FileReader { get { return _FileReader; } }
        public DecompressedFileHeader Header { get { return _Header; } }

        public void ParseHeader()
        {
            _FileCount = _FileReader.ReadInt32();
            _Header.Files = new FileObject[_FileCount];
            ConsoleIO.Log("-------------Parsing Compressed Header-------------");
            for (int i = 0; i < _FileCount; i++)
            {
                _Header.Files[i] = new FileObject();
                _Header.Files[i].Name = _FileReader.ReadString();
                _Header.Files[i].Offset = _FileReader.ReadInt32();
                _Header.Files[i].Size = _FileReader.ReadInt32();
                ConsoleIO.Log("Name: \"" + _Header.Files[i].Name + "\" Size: " + _Header.Files[i].Size + " Offset: " + _Header.Files[i].Offset);
            }
            ConsoleIO.Log("-------------Operation done!-------------");
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
                _FileReader.Goto(_Header.Files[i].Offset);
                _Header.Files[i].Bytes = _FileReader.ReadByteArray(_Header.Files[i].Bytes.Length);
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
