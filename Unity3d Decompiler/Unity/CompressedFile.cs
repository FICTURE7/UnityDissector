using System;
using System.IO;

namespace Unity3dDecompiler.Unity
{
    public class CompressedFile
    {
        private int _Size;
        public int Size { get { return _Size; } }

        private Stream _Stream;
        public Stream Stream { get { return _Stream; } }

        private byte[] _HeaderBytes;
        public byte[] HeaderBytes { get { return _HeaderBytes; } }

        private byte[] _Bytes;
        public byte[] Bytes { get { return _Bytes; } }

        private CompressedFileHeader _Header;
        public CompressedFileHeader Headers { get { return _Header; } }

        public CompressedFile(byte[] file)
        {
            _Stream = new MemoryStream(file);

            _Bytes = file;
            _Size = file.Length;
            _Header = ParseHeader();
            _HeaderBytes = GetHeaderBytes(_Header.OffsetCompressedData);
        }

        public byte[] GetHeaderBytes(int offset)
        {
            _HeaderBytes = new byte[offset];
            Buffer.BlockCopy(_Bytes, 0, _HeaderBytes, 0, offset);

            return _HeaderBytes;
        }

        public CompressedFileHeader ParseHeader()
        {
            CompressedFileHeader FileHeader = new CompressedFileHeader();

            if (_Bytes != null)
            {
                DataStream fileReader = new DataStream(_Stream);

                FileHeader.Signature = fileReader.ReadString();
                if (FileHeader.Signature == "UnityWeb")
                {

                    FileHeader.BuildVerison = fileReader.ReadInt();

                    FileHeader.WebPlayerVersion = fileReader.ReadString();
                    ConsoleIO.Log("Unity WebPlayer version: " + FileHeader.WebPlayerVersion, ConsoleIO.LogType.Info);

                    FileHeader.UnityEngineVersion = fileReader.ReadString();
                    ConsoleIO.Log("Unity Engine version: " + FileHeader.UnityEngineVersion, ConsoleIO.LogType.Info);

                    FileHeader.CompressedFileSize = fileReader.ReadInt();
                    ConsoleIO.Log("Compressed file size: " + FileHeader.CompressedFileSize, ConsoleIO.LogType.Info);

                    FileHeader.OffsetCompressedData = fileReader.ReadInt();
                    ConsoleIO.Log("Offset of compressed data: " + FileHeader.OffsetCompressedData, ConsoleIO.LogType.Info);
                    fileReader.SkipBytes(8);

                    FileHeader.CompressedFileSizeWithoutHeader = fileReader.ReadInt();
                    ConsoleIO.Log("Compressed file size without header: " + FileHeader.CompressedFileSizeWithoutHeader, ConsoleIO.LogType.Info);

                    FileHeader.DecompressedFileSize = fileReader.ReadInt();
                    ConsoleIO.Log("Decompressed file size: " + FileHeader.DecompressedFileSize, ConsoleIO.LogType.Info);

                    FileHeader.CompressedFileSize2 = fileReader.ReadInt();
                    ConsoleIO.Log("Compressed file size: " + FileHeader.CompressedFileSize2, ConsoleIO.LogType.Info);

                    FileHeader.DecompressedFileHeaderEndingOffset = fileReader.ReadInt();
                    ConsoleIO.Log("Begining of data in decompressed file: " + FileHeader.DecompressedFileHeaderEndingOffset, ConsoleIO.LogType.Info);
                    return FileHeader;
                }
                else
                {
                    ConsoleIO.WriteError("File header is not a unity3d file's header.");
                    return FileHeader;
                }
            }
            else
            {
                ConsoleIO.WriteError("File returned a null.");
                return FileHeader;
            }
        }
    }
}
