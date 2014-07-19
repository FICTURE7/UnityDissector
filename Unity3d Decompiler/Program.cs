using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity3dDecompiler.Unity;
using SevenZip.Compression;
using SevenZip.Compression.LZMA;
using System.IO;

namespace Unity3dDecompiler
{
    class Program
    {
        static void Main()
        {
            Unpacker unpacker = new Unpacker(@"C:\Users\Ramda_000\Documents\Unity Test\Test\Tests.unity3d");
            ConsoleIO.WriteInfo("File Path: " + unpacker.FilePath);
            ConsoleIO.WriteInfo("File Name: " + unpacker.FileName);
            ConsoleIO.WriteInfo("File Extension: " + unpacker.FileExtension);
            ConsoleIO.WriteInfo("File Size: " + unpacker.FileSize + " bytes");
            unpacker.Unpack();
            ConsoleIO.WriteInfo("Sucessfully unpacked file");
            //byte[] shizz;
            //shizz = SevenZipHelper.Compress(unpacker.DecompressedFile.Bytes);

            //byte[] test = null;
            //test = concatBytes(unpacker.CompressedFile.HeaderBytes, shizz);
            //File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Unity Test\Test\Stuff and things.unity3d", test);

            Repacker repacker = new Repacker(@"C:\Users\Ramda_000\Documents\Git\Unity3D-Deompiler\Unity3d Decompiler\bin\Debug\Tests");
            repacker.Repack();
            Console.Read();
        }

        private static byte[] concatBytes(params byte[][] bytes)
        {
            List<byte> result = new List<byte>();
            foreach (byte[] array in bytes)
                result.AddRange(array);
            return result.ToArray();
        }
    }
}
