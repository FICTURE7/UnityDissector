using SevenZip.Compression.LZMA;
using Unity3dDecompiler.Unity;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Unity3dDecompiler
{
    public class Unpacker
    {
        private string _filePath;
        public string FilePath { get { return _filePath; } }

        private string _fileName;
        public string FileName { get { return _fileName; } }

        private string _fileExtension;
        public string FileExtension { get { return _fileExtension; } }

        private long _fileSize;
        public long FileSize { get { return _fileSize; } }

        private byte[] _fileBytes;
        public byte[] FileBytes { get { return _fileBytes; } }

        private CompressedFile _compressedFile;
        public CompressedFile CompressedFile { get { return _compressedFile; } }

        private DecompressedFile _decompressedFile;
        public DecompressedFile DecompressedFile { get { return _decompressedFile; } }

        public Unpacker(string filePath)
        {
            if (filePath != null && Path.GetExtension(filePath) == ".unity3d")
            {
                _filePath = filePath;
                _fileName = Path.GetFileNameWithoutExtension(filePath);
                _fileExtension = Path.GetExtension(filePath);
                _fileBytes = FileToRead(filePath);
                _fileSize = _fileBytes.LongLength;

                _compressedFile = new CompressedFile(_fileBytes);
            }
            else
            {
                ConsoleIO.WriteError("Invalid file type or could not reach file.");
            }
        }

        public void BurteForceUnpack(int timesToTry)
        {
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
            }

            _decompressedFile = new DecompressedFile(SevenZipHelper.Decompress(buf));
            Extract();
        }

        public void Unpack()
        {
            byte[] buf = new byte[_compressedFile.Bytes.Length - _compressedFile.HeaderBytes.Length];
            Buffer.BlockCopy(_compressedFile.Bytes, _compressedFile.HeaderBytes.Length, buf, 0, _compressedFile.Bytes.Length - _compressedFile.HeaderBytes.Length);

            _decompressedFile = new DecompressedFile(SevenZipHelper.Decompress(buf));
            Extract();
        }

        public void Extract()
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + _fileName);
            _decompressedFile.ExtractFiles(AppDomain.CurrentDomain.BaseDirectory + _fileName);
        }

        private byte[] FileToRead(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
