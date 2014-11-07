using System;
using System.IO;
using Unity3DDisassembler.IO;
using Unity3DDisassembler.Compression.LZMA;
using Unity3DDisassembler.Unity;

namespace Unity3DDisassembler
{
    public class Disassembler : IDisposable
    {
        /// <summary>
        /// The Disassembler which will disassemble the file and extract it.
        /// </summary>
        /// <param name="filePath">Path pointing to file.</param>
        public Disassembler(string filePath)
        {
            if (filePath != null)
            {
                if (Path.GetExtension(filePath) == ".unity3d")
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
                    ConsoleIO.WriteLine("Invalid file type", ConsoleIO.LogType.Error);
                    throw new NotSupportedException("Invalid file type, the file type was not a .unity3d");
                }
            }
            else
            {
                ConsoleIO.WriteLine("Could not reach file", ConsoleIO.LogType.Error);
                throw new ArgumentNullException("filePath", "filePath was null :[");
            }
        }

        /// <summary>
        /// Initializes a new instance of Disassmebler class.
        /// </summary>
        /// <param name="fileBytes">File to disassemble in bytes.</param>
        public Disassembler(byte[] fileBytes)
        {
            if (fileBytes != null)
            {
                _fileBytes = fileBytes;
                _fileSize = fileBytes.LongLength;
                ConsoleIO.Log("File Size: " + _fileSize);
            }
            else
            {
                throw new ArgumentNullException("fileBytes", "fileBytes was null. :[");
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

        /// <summary>
        /// Try to disassemble the .unity3d file.
        /// </summary>
        /// <param name="timesToTry">Times to try</param>
        public void BurteForceDisassemble(int timesToTry)
        {
            ConsoleIO.WriteLine("Brute force disassembling file.");
            byte[] buf = null;
            for (int i = 0; i < timesToTry; i++)
            {
                try
                {
                    buf = new byte[_compressedFile.Bytes.Length - i];
                    Buffer.BlockCopy(_compressedFile.Bytes, i, buf, 0, buf.Length);

                    LzmaUtils.Decompress(buf);
                    ConsoleIO.WriteLine("Was able to decompress file at offset: " + i.ToString("X"));
                    _decompressedFile = new DecompressedFile(LzmaUtils.Decompress(buf));
                    break;
                }
                catch { }
                ConsoleIO.WriteLine("Failed to brute force unpack file. Tried: " + i.ToString(), ConsoleIO.LogType.Error);
            }
        }

        /// <summary>
        /// Disassemble the .unity3d file.
        /// </summary>
        public void Disassemble()
        {
            _compressedFile.ReadFile();
            _compressedFile.FileReader.BaseStream.Position = _compressedFile.Header.CompressedFileHeaderSize;
            byte[] compressed_file_body = _compressedFile.FileReader.ReadByteArray(_compressedFile.Header.CompressedFileSizeWithoutHeader); //Get the body of the compressed file

            ConsoleIO.Log("Decompressing the .unity3d with Lzma...");
            _decompressedFile = new DecompressedFile(LzmaUtils.Decompress(compressed_file_body));
            _decompressedFile.ReadFile();
        }

        public void List()
        {
            ConsoleIO.WriteLine("List: ");
            for (int i = 0; i < _decompressedFile.Header.Files.Length; i++)
            {
                ConsoleIO.WriteLine(_decompressedFile.Header.Files[i].Name);
            }
        }

        /// <summary>
        /// Extract all files at the disassembler's directory with the .unity file's name 
        /// as root directory.
        /// </summary>
        public void Extract()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + _fileName))//Make sure that the directory was created
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + _fileName);
                _decompressedFile.ExtractFiles(AppDomain.CurrentDomain.BaseDirectory + _fileName);
            }
            else
            {
                _decompressedFile.ExtractFiles(AppDomain.CurrentDomain.BaseDirectory + _fileName);
            }
        }

        /// <summary>
        /// Extract all files at given path with the .unity3d file's name
        /// as root directory.
        /// </summary>
        /// <param name="path">Path to extract files.</param>
        public void ExtractTo(string path)
        {
            if (!Directory.Exists(path + _fileName))//Make sure that the directory was created
            {
                Directory.CreateDirectory(path + _fileName);
                _decompressedFile.ExtractFiles(path + _fileName);
            }
            else
            {
                _decompressedFile.ExtractFiles(path + _fileName);
            }
        }

        public void Dispose() //Faily attempt to reduce memory usage. :[
        {
            if (_fileBytes != null) _fileBytes = null;
            if (_fileExtension != null) _fileExtension = null;
            if (_fileName != null) _fileName = null;
            if (_filePath != null) _filePath = null;
            if (_compressedFile != null) _compressedFile = null;
            if (_decompressedFile != null) _decompressedFile = null;
        }

        private byte[] ReadFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
