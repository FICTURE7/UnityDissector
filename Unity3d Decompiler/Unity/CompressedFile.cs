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

        private string[] _header;
        public string[] Header { get { return _header; } }

        public CompressedFile(byte[] file)
        {
            _Stream = new MemoryStream(file);

            _Bytes = file;
            _Size = file.Length;
            _header = ParseHeader();
            _HeaderBytes = GetHeaderBytes(Convert.ToInt32(_header[5]));
        }

        public byte[] GetHeaderBytes(int offset)
        {
            _HeaderBytes = new byte[offset];
            Buffer.BlockCopy(_Bytes, 0, _HeaderBytes, 0, offset);

            return _HeaderBytes;
        }

        public string[] ParseHeader()
        {
            string[] FileInfo = new string[10];

            if (_Bytes != null)
            {
                DataStream fileReader = new DataStream(_Stream);

                //Signature 'UnityWeb' for web archive (.unity3d)
                FileInfo[0] = fileReader.ReadString();
                if (FileInfo[0] == "UnityWeb")
                {
                    //File version
                    FileInfo[1] = fileReader.ReadInt().ToString();
                    //Unity Engine version
                    FileInfo[2] = fileReader.ReadString();
                    ConsoleIO.Log("Unity WebPlayer version: " + FileInfo[1], ConsoleIO.LogType.Info);
                    //Full Unity Engine version
                    FileInfo[3] = fileReader.ReadString();
                    ConsoleIO.Log("Unity Engine version: " + FileInfo[1], ConsoleIO.LogType.Info);
                    //Compressed file size.
                    FileInfo[4] = fileReader.ReadInt().ToString();
                    //Begin of compressed data/end of header
                    FileInfo[5] = fileReader.ReadInt().ToString();
                    fileReader.SkipBytes(8);
                    //Compressed size file without header.
                    FileInfo[6] = fileReader.ReadInt().ToString();
                    //Decompressed file size.
                    FileInfo[7] = fileReader.ReadInt().ToString();
                    //Compressed file size.
                    FileInfo[8] = fileReader.ReadInt().ToString();
                    if (FileInfo[4] != FileInfo[8] || FileInfo[4] != _Size.ToString() || FileInfo[8] != _Size.ToString())
                    {
                        ConsoleIO.WriteWarning("Compressed file size does not match header.");
                    }
                    //Begining of uncompressed data in uncompressed file.
                    FileInfo[9] = fileReader.ReadInt().ToString();
                    return FileInfo;
                }
                else
                {
                    ConsoleIO.WriteError("File header is not a unity3d file's header.");
                    return FileInfo;
                }
            }
            else
            {
                ConsoleIO.WriteError("File returned a null.");
                return FileInfo;
            }
        }
    }
}
