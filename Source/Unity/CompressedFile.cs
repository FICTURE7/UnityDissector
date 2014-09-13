using System;
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
            _FileStream = new FileReader(file);

            _Bytes = file;
            _Size = file.Length;

            ParseHeader();
            GetHeaderBytes();
        }

        private int _Size;
        private byte[] _Bytes;
        private FileReader _FileStream;
        private CompressedFileHeader _Header;

        public int Size { get { return _Size; } }
        public byte[] Bytes { get { return _Bytes; } }
        public FileReader FileReader { get { return _FileStream; } }
        public CompressedFileHeader Header { get { return _Header; } }

        public void ParseHeader()
        {
            ConsoleIO.Log("-------------Parsing Compressed header-------------", ConsoleIO.LogType.Info);
            _Header = new CompressedFileHeader();

            if (_Bytes != null)
            {
                _Header.Signature = _FileStream.ReadString();
                if (_Header.Signature == "UnityWeb")
                {
                    _Header.BuildVerison = _FileStream.ReadInt32();

                    _Header.WebPlayerVersion = _FileStream.ReadString();
                    ConsoleIO.Log("Unity WebPlayer version: " + _Header.WebPlayerVersion, ConsoleIO.LogType.Info);

                    _Header.UnityEngineVersion = _FileStream.ReadString();
                    ConsoleIO.Log("Unity Engine version: " + _Header.UnityEngineVersion, ConsoleIO.LogType.Info);

                    _Header.CompressedFileSize = _FileStream.ReadInt32();
                    ConsoleIO.Log("Compressed file size: " + _Header.CompressedFileSize, ConsoleIO.LogType.Info);

                    _Header.CompressedFileHeaderSize = _FileStream.ReadInt32();
                    ConsoleIO.Log("Offset of compressed data: " + _Header.CompressedFileHeaderSize, ConsoleIO.LogType.Info);
                    _FileStream.SkipBytes(8); //Skip the '00 00 00 01 00 00 00 01' bytes

                    _Header.CompressedFileSizeWithoutHeader = _FileStream.ReadInt32();
                    ConsoleIO.Log("Compressed file size without header: " + _Header.CompressedFileSizeWithoutHeader, ConsoleIO.LogType.Info);

                    _Header.DecompressedFileSize = _FileStream.ReadInt32();
                    ConsoleIO.Log("Decompressed file size: " + _Header.DecompressedFileSize, ConsoleIO.LogType.Info);

                    _Header.CompressedFileSize2 = _FileStream.ReadInt32();
                    ConsoleIO.Log("Compressed file size: " + _Header.CompressedFileSize2, ConsoleIO.LogType.Info);

                    _Header.DecompressedFileHeaderSize = _FileStream.ReadInt32();
                    ConsoleIO.Log("Decompressed file header size: " + _Header.DecompressedFileHeaderSize, ConsoleIO.LogType.Info);
                }
                else
                {
                    ConsoleIO.WriteLine("File header is not a unity3d file's header.", ConsoleIO.LogType.Error);
                }
            }
            else
            {
                ConsoleIO.WriteLine("File was null.", ConsoleIO.LogType.Error);
            }
            ConsoleIO.Log("-------------Operation done!-------------", ConsoleIO.LogType.Info);
        }

        public void GetHeaderBytes()
        {
            var bytes = new byte[_Header.CompressedFileHeaderSize];
            Buffer.BlockCopy(_Bytes, 0, bytes, 0, _Header.CompressedFileHeaderSize);
            _Header.Bytes = bytes;
        }
    }
}
