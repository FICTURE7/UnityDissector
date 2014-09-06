using System;
using System.IO;

namespace Unity3dDecompiler.Unity
{
    public class CompressedFile
    {
        /// <summary>
        /// Parses the header and thats it
        /// </summary>
        /// <param name="file">Compressed file bytes</param>
        public CompressedFile(byte[] file)
        {
            _Stream = new DataStream(file);

            _Bytes = file;
            _Size = file.Length;

            ParseHeader();
            GetHeaderBytes();
        }

        private int _Size;
        private byte[] _Bytes;
        private DataStream _Stream;
        private CompressedFileHeader _Header;

        public int Size { get { return _Size; } }
        public byte[] Bytes { get { return _Bytes; } }
        public DataStream Stream { get { return _Stream; } }
        public CompressedFileHeader Header { get { return _Header; } }

        public void ParseHeader()
        {
            _Header = new CompressedFileHeader();

            if (_Bytes != null)
            {
                _Header.Signature = _Stream.ReadString();
                if (_Header.Signature == "UnityWeb")
                {
                    _Header.BuildVerison = _Stream.ReadInt();

                    _Header.WebPlayerVersion = _Stream.ReadString();
                    ConsoleIO.Log("Unity WebPlayer version: " + _Header.WebPlayerVersion, ConsoleIO.LogType.Info);

                    _Header.UnityEngineVersion = _Stream.ReadString();
                    ConsoleIO.Log("Unity Engine version: " + _Header.UnityEngineVersion, ConsoleIO.LogType.Info);

                    _Header.CompressedFileSize = _Stream.ReadInt();
                    ConsoleIO.Log("Compressed file size: " + _Header.CompressedFileSize, ConsoleIO.LogType.Info);

                    _Header.OffsetCompressedData = _Stream.ReadInt();
                    ConsoleIO.Log("Offset of compressed data: " + _Header.OffsetCompressedData, ConsoleIO.LogType.Info);
                    _Stream.SkipBytes(8); //Skip the '00 00 00 01 00 00 00 01' bytes

                    _Header.CompressedFileSizeWithoutHeader = _Stream.ReadInt();
                    ConsoleIO.Log("Compressed file size without header: " + _Header.CompressedFileSizeWithoutHeader, ConsoleIO.LogType.Info);

                    _Header.DecompressedFileSize = _Stream.ReadInt();
                    ConsoleIO.Log("Decompressed file size: " + _Header.DecompressedFileSize, ConsoleIO.LogType.Info);

                    _Header.CompressedFileSize2 = _Stream.ReadInt();
                    ConsoleIO.Log("Compressed file size: " + _Header.CompressedFileSize2, ConsoleIO.LogType.Info);

                    _Header.DecompressedFileHeaderEndingOffset = _Stream.ReadInt();
                    ConsoleIO.Log("Begining of data in decompressed file: " + _Header.DecompressedFileHeaderEndingOffset, ConsoleIO.LogType.Info);
                }
                else
                {
                    ConsoleIO.WriteError("File header is not a unity3d file's header.");
                }
            }
            else
            {
                ConsoleIO.WriteError("File was null.");
            }
        }

        public void GetHeaderBytes()
        {
            var bytes = new byte[_Header.OffsetCompressedData];
            Buffer.BlockCopy(_Bytes, 0, bytes, 0, _Header.OffsetCompressedData);
            _Header.Bytes = bytes;
        }
    }
}
