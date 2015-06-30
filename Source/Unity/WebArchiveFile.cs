using System.IO;
using Unity3DDisassembler.IO;

namespace Unity3DDisassembler.Unity
{
    public class WebArchiveFile
    {
        public string Name { get; set; }
        public int Offset { get; set; }
        public int Size { get; set; }
        public byte[] Bytes { get; set; }

        public void ReadFileObject(FileReader reader)
        {
            Name = reader.ReadString();
            Offset = reader.ReadInt32();
            Size = reader.ReadInt32();
            //reader.BaseStream.Position = Offset;
            //Bytes = reader.ReadByteArray(Size);
        }

        public void WriteFileObject(FileWriter writer)
        {
            writer.WriteString(Name);
            writer.WriteInt32(Offset);
            writer.WriteInt32(Size);
            //writer.BaseStream.Position = Offset;
            //writer.WriteByteArray(Bytes);
        }

        public void WriteFile(string path)
        {
            if (Name.Contains("/"))
            {
                string[] directory = Name.Split('/');
                Directory.CreateDirectory(path + @"\" + directory[0]);
                File.WriteAllBytes(path + @"\" + directory[0] + @"\" + directory[1], Bytes);
            }
            else
            {
                File.WriteAllBytes(path + @"\" + Name, Bytes);
            }
        }
    }
}
