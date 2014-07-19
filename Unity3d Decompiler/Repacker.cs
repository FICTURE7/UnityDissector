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
                Files[i].Name = Directory.GetFiles(directory)[i];
                string[] temp = Files[i].Name.Split('\\');
                Files[i].Name = temp[temp.Length - 1];
                Files[i].Bytes = File.ReadAllBytes(directory + @"\" + Files[i].Name);
                Files[i].Size = Files[i].Bytes.Length;
            }
        }

        public void Repack()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataStream fileStream = new DataStream(stream);
                fileStream.WriteInt(Files.Length);
            }
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
