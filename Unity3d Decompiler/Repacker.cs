using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity3dDecompiler.Unity;

namespace Unity3dDecompiler
{
    public class Repacker
    {
        public FileObject[] Files { get; set; }

        public Repacker(string directory)
        {
            Files = new FileObject[Directory.GetFiles(directory).Length];
            for (int i = 0; i < Directory.GetFiles(directory).Length; i++)
            {
                Files[i] = new FileObject();
                Files[i].Name = GetFilesName(directory)[i];
                Files[i].Bytes = File.ReadAllBytes(directory + @"\" + Files[i].Name);
                Files[i].Size = Files[i].Bytes.Length;
            }
        }

        public void Repack()
        {
            byte[] fullBytes = new byte[0];
            for (int i = 0; i < Files.Length; i++)
            {
                fullBytes = ConcatBytes(fullBytes, Files[i].Bytes);
            }
            MakeHeader();
        }

        public string[] GetFilesName(string directory)
        {
            string[] filesPath = Directory.GetFiles(directory);
            string[] FileNames = new string[filesPath.Length];
            for (int i = 0; i < filesPath.Length; i++)
            {
                string[] split = filesPath[i].Split('\\');
                FileNames[i] = split[split.Length - 1];
            }
            return FileNames;
        }

        public byte[] MakeHeader()
        {
            Files[0].Offset = CalcSize();
            byte[] fullBytes = null;
            using(MemoryStream stream = new MemoryStream())
            {
                DataStream fileStream = new DataStream(stream);
                fileStream.WriteInt(Files.Length);
                fileStream.WriteString(Files[0].Name);
                fileStream.WriteInt(Files[0].Offset);
                fileStream.WriteInt(Files[0].Size);

                for(int i = 1; i < Files.Length; i++)
                {
                    fileStream.WriteString(Files[i].Name);
                    Files[i].Offset = Files[i - 1].Size + Files[i - 1].Offset;
                    fileStream.WriteInt(Files[i].Offset);
                }

                fullBytes = stream.ToArray();
                File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Git\Unity3D-Deompiler\Unity3d Decompiler\bin\Debug\Dump.txt", fullBytes);
                return fullBytes;
            }
        }

        public int CalcSize()
        {
            int Size = 0;
            for(int i = 0; i < Files.Length; i++)
            {
                Size = Size + Files[i].Name.Length + 1 + 4 + 4;
            }
            return Size;
        }

        public void WriteFile(FileObject file, DataStream stream)
        {
            stream.WriteString(file.Name);
        }

        public static byte[] ConcatBytes(params byte[][] bytes)
        {
            List<byte> result = new List<byte>();
            foreach (byte[] array in bytes)
                result.AddRange(array);
            return result.ToArray();
        }
    }
}
