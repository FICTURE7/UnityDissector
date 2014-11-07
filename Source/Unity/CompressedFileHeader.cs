using System.IO;
using Unity3DDisassembler.IO;

namespace Unity3DDisassembler.Unity
{
    public class CompressedFileHeader
    {
        public string Signature { get; set; }

        public int BuildVerison { get; set; }

        public string WebPlayerVersion { get; set; }

        public string UnityEngineVersion { get; set; }

        public int CompressedFileSize { get; set; }

        public int CompressedFileHeaderSize { get; set; }

        public int CompressedFileSizeWithoutHeader { get; set; }

        public int DecompressedFileSize { get; set; }

        public int CompressedFileSize2 { get; set; }

        public int DecompressedFileHeaderSize { get; set; }

        public byte[] Bytes { get; set; }

        public void ReadHeader(FileReader reader)
        {
            if (reader.ReadString() != "UnityWeb") 
                throw new InvalidDataException("Invalid .unity3d Expected \"UnityWeb\" at: 0");

            BuildVerison = reader.ReadInt32();
            WebPlayerVersion = reader.ReadString();
            UnityEngineVersion = reader.ReadString();
            CompressedFileSize = reader.ReadInt32();
            CompressedFileHeaderSize = reader.ReadInt32();
            reader.ReadInt32();
            reader.ReadInt32();
            CompressedFileSizeWithoutHeader = reader.ReadInt32();
            DecompressedFileSize = reader.ReadInt32();
            CompressedFileSize2 = reader.ReadInt32();
            DecompressedFileHeaderSize = reader.ReadInt32();
            if (reader.ReadByte() != 0x00) throw new InvalidDataException("Expected 0x00 at: " + reader.BaseStream.Position);
        }

        public void WriteHeader(FileWriter writer)
        {
            writer.WriteString("UnityWeb");
            writer.WriteInt32(BuildVerison);
            writer.WriteString(WebPlayerVersion);
            writer.WriteString(UnityEngineVersion);
            writer.WriteInt32(CompressedFileSize);
            writer.WriteInt32(CompressedFileHeaderSize);
            writer.WriteInt32(1);
            writer.WriteInt32(1);
            writer.WriteInt32(CompressedFileSizeWithoutHeader);
            writer.WriteInt32(DecompressedFileHeaderSize);
            writer.WriteInt32(CompressedFileSize2);
            writer.WriteInt32(DecompressedFileHeaderSize);
            writer.WriteByte(0x00);
        }
    }
}
