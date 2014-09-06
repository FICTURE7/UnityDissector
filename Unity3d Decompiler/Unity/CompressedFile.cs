using System;
using System.IO;

namespace Unity3dDecompiler.Unity
{
    public class CompressedFile
    {
        public CompressedFile(byte[] file)
        {
            _Stream = new DataStream(file);

            _Bytes = file;
            _Size = file.Length;

            ParseHeader();
            GetHeaderBytes();
        }

        private int _Size;
        private byte[] _HeaderBytes;
        private byte[] _Bytes;
        private DataStream _Stream;
        private CompressedFileHeader _Header;

        public int Size { get { return _Size; } }
        public byte[] HeaderBytes { get { return _HeaderBytes; } }
        public byte[] Bytes { get { return _Bytes; } }
        public DataStream Stream { get { return _Stream; } }
        public CompressedFileHeader Header { get { return _Header; } }

        public void GetHeaderBytes()
        {
            _HeaderBytes = new byte[_Header.OffsetCompressedData];
            Buffer.BlockCopy(_Bytes, 0, _HeaderBytes, 0, _Header.OffsetCompressedData);
        }

        public void ParseHeader()
        {
            CompressedFileHeader FileHeader = new CompressedFileHeader();

            if (_Bytes != null)
            {
                FileHeader.Signature = _Stream.ReadString();
                if (FileHeader.Signature == "UnityWeb")
                {

                    FileHeader.BuildVerison = _Stream.ReadInt();

                    FileHeader.WebPlayerVersion = _Stream.ReadString();
                    ConsoleIO.Log("Unity WebPlayer version: " + FileHeader.WebPlayerVersion, ConsoleIO.LogType.Info);

                    FileHeader.UnityEngineVersion = _Stream.ReadString();
                    ConsoleIO.Log("Unity Engine version: " + FileHeader.UnityEngineVersion, ConsoleIO.LogType.Info);

                    FileHeader.CompressedFileSize = _Stream.ReadInt();
                    ConsoleIO.Log("Compressed file size: " + FileHeader.CompressedFileSize, ConsoleIO.LogType.Info);

                    FileHeader.OffsetCompressedData = _Stream.ReadInt();
                    ConsoleIO.Log("Offset of compressed data: " + FileHeader.OffsetCompressedData, ConsoleIO.LogType.Info);
                    _Stream.SkipBytes(8);

                    FileHeader.CompressedFileSizeWithoutHeader = _Stream.ReadInt();
                    ConsoleIO.Log("Compressed file size without header: " + FileHeader.CompressedFileSizeWithoutHeader, ConsoleIO.LogType.Info);

                    FileHeader.DecompressedFileSize = _Stream.ReadInt();
                    ConsoleIO.Log("Decompressed file size: " + FileHeader.DecompressedFileSize, ConsoleIO.LogType.Info);

                    FileHeader.CompressedFileSize2 = _Stream.ReadInt();
                    ConsoleIO.Log("Compressed file size: " + FileHeader.CompressedFileSize2, ConsoleIO.LogType.Info);

                    FileHeader.DecompressedFileHeaderEndingOffset = _Stream.ReadInt();
                    ConsoleIO.Log("Begining of data in decompressed file: " + FileHeader.DecompressedFileHeaderEndingOffset, ConsoleIO.LogType.Info);
                    _Header = FileHeader;
                }
                else
                {
                    ConsoleIO.WriteError("File header is not a unity3d file's header.");
                    _Header = FileHeader;
                }
            }
            else
            {
                ConsoleIO.WriteError("File was null.");
                _Header = FileHeader;
            }
        }
    }
}
