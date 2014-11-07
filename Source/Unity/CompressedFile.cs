using System;
using System.IO;
using Unity3DDisassembler.IO;

namespace Unity3DDisassembler.Unity
{
    public class CompressedFile
    {
        /// <summary>
        /// Parses the header and thats it
        /// </summary>
        /// <param name="file">Compressed file bytes</param>
        public CompressedFile(byte[] file)
        {
            _FileReader = new FileReader(file);
            _Header = new CompressedFileHeader();

            _Bytes = file;
            _Size = file.Length;
        }

        public CompressedFile()
        {
            _FileWriter = new FileWriter();
            _Header = new CompressedFileHeader();
        }

        private int _Size;
        private byte[] _Bytes;
        private FileReader _FileReader;
        private FileWriter _FileWriter;
        private CompressedFileHeader _Header;

        public int Size { get { return _Size; } }
        public byte[] Bytes { get { return _Bytes; } }
        public FileReader FileReader { get { return _FileReader; } }
        public FileWriter FileWriter { get { return _FileWriter; } }
        public CompressedFileHeader Header { get { return _Header; } }

        public void ReadFile()
        {
            _Header.ReadHeader(_FileReader);
        }

        public void WriteFile(byte[] compressedBody)
        {
            _Header.WriteHeader(_FileWriter);
            _FileWriter.WriteByteArray(compressedBody);
        }
    }
}
