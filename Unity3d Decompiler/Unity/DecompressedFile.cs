using System;
using System.IO;
using Unity3dDecompiler.Unity;

namespace Unity3dDecompiler.Unity
{
    public class DecompressedFile
    {
        private int _Size;
        public int Size { get { return _Size; } }

        private int _fileCount;
        public int FileCount { get { return _fileCount; } }

        private Stream _Stream;
        public Stream Stream { get { return _Stream; } }

        private byte[] _HeaderBytes;
        public byte[] HeaderBytes { get { return _HeaderBytes; } }

        private byte[] _Bytes;
        public byte[] Bytes { get { return _Bytes; } }

        private FileInfo[] _files;
        public FileInfo[] Files { get { return _files; } }

        public DecompressedFile(byte[] file)
        {
            _Stream = new MemoryStream(file);

            _Bytes = file;
            _Size = file.Length;
            _files = ParseHeader();
            _HeaderBytes = GetHeaderBytes(_files[0].Offset);
            GetFiles();

            ConsoleIO.Log("Decompressed file size: " + _Bytes.Length);
            ConsoleIO.Log("Decompressed file header size: " + _HeaderBytes.Length);
            ConsoleIO.Log("Files: ");
            for (int i = 0; i < _files.Length; i++)
            {
                ConsoleIO.Log("Name: \"" + _files[i].Name + "\" Size: " +  _files[i].Size + " Offset: " + _files[i].Offset);
            }
        }

        public byte[] GetHeaderBytes(int offset)
        {
            _HeaderBytes = new byte[offset];
            Buffer.BlockCopy(_Bytes, 0, _HeaderBytes, 0, offset);

            return _HeaderBytes;
        }

        public void GetFiles()
        {
            DataStream fileReader = new DataStream(_Stream);
            for (int i = 0; i < _fileCount; i++)
            {
                _files[i].Bytes = new byte[_files[i].Size];
                fileReader.MainStream.Position = _files[i].Offset;
                _files[i].Bytes = fileReader.ReadByteArray(_files[i].Bytes.Length);
            }
        }

        public FileInfo[] ParseHeader()
        {
            DataStream fileReader = new DataStream(_Stream);
            _fileCount = fileReader.ReadInt();
            FileInfo[] files = new FileInfo[_fileCount];

            for (int i = 0; i < _fileCount; i++)
            {
                files[i] = new FileInfo();
                files[i].Name = fileReader.ReadString();
                files[i].Offset = fileReader.ReadInt();
                files[i].Size = fileReader.ReadInt();
            }
            return files;
        }

        public void ExtractFiles(string path)
        {
            for (int i = 0; i < _fileCount; i++)
            {
                _files[i].WriteFile(path);
            }
        }
    }
}
