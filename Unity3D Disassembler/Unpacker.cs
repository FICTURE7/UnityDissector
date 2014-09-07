using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity3DDisassembler.Compression.LZMA;
using Unity3DDisassembler.Unity;

namespace Unity3DDisassembler
{
    public class Unpacker
    {
        public Unpacker(string filePath)
        {
            if (filePath != null && Path.GetExtension(filePath) == ".unity3d")
            {
                _filePath = filePath;
                ConsoleIO.Log("File path: " + _filePath);
                _fileName = Path.GetFileNameWithoutExtension(filePath);
                ConsoleIO.Log("File name: " + _fileName);
                _fileExtension = Path.GetExtension(filePath);
                ConsoleIO.Log("File extension: " + _fileExtension);
                _fileBytes = ReadFile(filePath);
                _fileSize = _fileBytes.LongLength;
                ConsoleIO.Log("File Size: " + _fileSize);

                _compressedFile = new CompressedFile(_fileBytes);
            }
            else
            {
                ConsoleIO.WriteError("Invalid file type or could not reach file.");
            }
        }

        private string _filePath;
        private string _fileName;
        private string _fileExtension;
        private long _fileSize;
        private byte[] _fileBytes;
        private CompressedFile _compressedFile;
        private DecompressedFile _decompressedFile;

        public string FilePath { get { return _filePath; } }
        public string FileName { get { return _fileName; } }
        public string FileExtension { get { return _fileExtension; } }
        public long FileSize { get { return _fileSize; } }
        public byte[] FileBytes { get { return _fileBytes; } }
        public CompressedFile CompressedFile { get { return _compressedFile; } }
        public DecompressedFile DecompressedFile { get { return _decompressedFile; } }

        public void BurteForceUnpack(int timesToTry)
        {
            ConsoleIO.WriteInfo("Brute forcing unpacking file.");
            byte[] buf = null;
            for (int i = 0; i < timesToTry; i++)
            {
                try
                {
                    buf = new byte[_compressedFile.Bytes.Length - 1];
                    Buffer.BlockCopy(_compressedFile.Bytes, i, buf, 0, buf.Length);

                    SevenZipHelper.Decompress(buf);
                    ConsoleIO.WriteInfo("Was able to decompress file at offset: " + i.ToString("X"));
                    break;
                }
                catch
                { }

                ConsoleIO.WriteError("Failed to brute force unpack file. Tried: " + i.ToString());
            }

            _decompressedFile = new DecompressedFile(SevenZipHelper.Decompress(buf));
            Extract();
        }

        public void Unpack()
        {
            byte[] buf = new byte[_compressedFile.Bytes.Length - _compressedFile.Header.Bytes.Length];
            Buffer.BlockCopy(_compressedFile.Bytes, _compressedFile.Header.Bytes.Length, buf, 0, _compressedFile.Bytes.Length - _compressedFile.Header.Bytes.Length); //Get file's body bytes

            _decompressedFile = new DecompressedFile(SevenZipHelper.Decompress(buf));
        }

        public void Extract()
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + _fileName);
            _decompressedFile.ExtractFiles(AppDomain.CurrentDomain.BaseDirectory + _fileName);
        }

        public void ExtractTo(string path)
        {
            Directory.CreateDirectory(path + _fileName);
            _decompressedFile.ExtractFiles(path + _fileName);
        }

        private byte[] ReadFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
