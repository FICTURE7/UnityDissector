using System;
using System.Collections.Generic;
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
            _FileWriter = new FileWriter();
            _FileReader = new FileReader(file);
            _Header = new DecompressedFileHeader();

            _Bytes = file;
            _Size = file.Length;
        }

        public DecompressedFile()
        {
            _FileWriter = new FileWriter();
            _Header = new DecompressedFileHeader();
        }

        private int _Size;
        private int _FileCount;
        private byte[] _Bytes;
        private FileReader _FileReader;
        private FileWriter _FileWriter;
        private DecompressedFileHeader _Header;

        public int Size { get { return _Size; } }
        public int FileCount { get { return _FileCount; } }
        public byte[] Bytes { get { return _Bytes; } }
        public FileReader FileReader { get { return _FileReader; } }
        public FileWriter FileWriter { get { return _FileWriter; } }
        public DecompressedFileHeader Header { get { return _Header; } }

        public void ReadFile()
        {
            _Header.ReadHeader(_FileReader);
            for (int i = 0; i < _Header.FileCount; i++)
            {
                _FileReader.BaseStream.Position = _Header.Files[i].Offset;
                _Header.Files[i].Bytes = _FileReader.ReadByteArray(_Header.Files[i].Size);
            }
        }

        public void WriteFile()
        {
            _Header.CalculateOffsets();
            _Header.WriteHeader(_FileWriter);
            for (int i = 0; i < _Header.FileCount; i++)
            {
                _FileWriter.BaseStream.Position = _Header.Files[i].Offset;
                _FileWriter.WriteByteArray(_Header.Files[i].Bytes);
            }
        }

        public void ExtractFiles(string path)
        {
            for (int i = 0; i < _Header.FileCount; i++)
            {
                _Header.Files[i].WriteFile(path);
            }
        }
    }
}
