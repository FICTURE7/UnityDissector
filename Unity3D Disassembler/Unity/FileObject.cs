using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3DDisassembler.Unity
{
    public class FileObject
    {
        public string Name { get; set; }
        public int Offset { get; set; }
        public int Size { get; set; }
        public byte[] Bytes { get; set; }

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
