using System;
using System.IO;
using Unity3DDisassembler.IO;

namespace Unity3DDisassembler.Unity
{
    public class DecompressedFileHeader
    {
        public int FileCount { get { return Files.Length; } }

        public byte[] Bytes { get; set; }

        public FileObject[] Files { get; set; }

        public void ReadHeader(FileReader reader)
        {
            reader.BaseStream.Position = 0;
            Files = new FileObject[reader.ReadInt32()]; //Read int32 to get array length
            for (int i = 0; i < FileCount; i++)
            {
                Files[i] = new FileObject();
                Files[i].ReadFileObject(reader);
            }
        }

        public void WriteHeader(FileWriter writer)
        {
            if (Files == null || FileCount == 0)
                throw new NullReferenceException("Files property cannot be null or empty.");

            writer.BaseStream.Position = 0;
            writer.WriteInt32(FileCount);
            for (int i = 0; i < FileCount; i++)
                Files[i].WriteFileObject(writer);
        }

        public void CalculateOffsets()
        {
            if (Files == null || FileCount == 0)
                throw new NullReferenceException("Files property cannot be null or empty.");

            int firstOffset = 4; //4 bytes for file conut.
            for (int i = 0; i < FileCount; i++)
                firstOffset = firstOffset + Files[i].Name.Length + 9;
            Files[0].Offset = firstOffset;
            for (int i = 1; i < FileCount; i++)
                Files[i].Offset = Files[i - 1].Offset + Files[i - 1].Size;
        }
    }
}
