using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity3DDisassembler.Common;
using Unity3DDisassembler.Compression.LZMA;
using Unity3DDisassembler.Unity;

namespace Unity3DDisassembler
{
    public class Disassembler
    {
        /// <summary>
        /// The Disassembler which will disassemble the file and extract it
        /// </summary>
        /// <param name="filePath">Path pointing to file</param>
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
                    ConsoleIO.WriteError("Invalid file type");
                    throw new NotSupportedException("Invalid file type, the file type was not a .unity3d");
                }
            }
            else
            {
                ConsoleIO.WriteError("Could not reach file");
                throw new ArgumentNullException("filePath", "filePath was null :[");
            }
        }

        /// <summary>
        /// The Disassembler which will disassemble the file and extract it
        /// </summary>
        /// <param name="fileBytes">File to disassemble in bytes</param>
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
        /// Try to disassemble the .unity3d file
        /// </summary>
        /// <param name="timesToTry">Times to try</param>
        public void BurteForceDisassemble(int timesToTry)
        {
            ConsoleIO.WriteInfo("Brute force disassembling file.");
            byte[] buf = null;
            for (int i = 0; i < timesToTry; i++)
            {
                try
                {
                    buf = new byte[_compressedFile.Bytes.Length - i];
                    Buffer.BlockCopy(_compressedFile.Bytes, i, buf, 0, buf.Length);

                    LzmaUtils.Decompress(buf);
                    ConsoleIO.WriteInfo("Was able to decompress file at offset: " + i.ToString("X"));
                    break;
                }
                catch { }
                ConsoleIO.WriteError("Failed to brute force unpack file. Tried: " + i.ToString());
            }

            _decompressedFile = new DecompressedFile(LzmaUtils.Decompress(buf));
        }

        /// <summary>
        /// Disassemble the .unity3d file
        /// </summary>
        public void Disassemble()
        {
            byte[] compressed_file_body = new byte[_compressedFile.Bytes.Length - _compressedFile.Header.Bytes.Length]; //Get the body of the compressed file
            Buffer.BlockCopy(_compressedFile.Bytes, _compressedFile.Header.Bytes.Length, compressed_file_body, 0, _compressedFile.Bytes.Length - _compressedFile.Header.Bytes.Length);

            _decompressedFile = new DecompressedFile(LzmaUtils.Decompress(compressed_file_body));
        }

        /// <summary>
        /// Extract all files at the disassembler's directory with the .unity file's name 
        /// as root directory
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
        /// as root directory
        /// </summary>
        /// <param name="path">Path to extract files</param>
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

        private byte[] ReadFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
