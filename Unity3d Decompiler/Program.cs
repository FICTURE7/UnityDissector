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
            Unpacker unpacker = new Unpacker(@"C:\Users\Ramda_000\Documents\Git\Unity3D-Deompiler\Unity3d Decompiler\bin\Debug\WebPlayer.unity3d");
            ConsoleIO.WriteInfo("File Path: " + unpacker.FilePath);
            ConsoleIO.WriteInfo("File Name: " + unpacker.FileName);
            ConsoleIO.WriteInfo("File Extension: " + unpacker.FileExtension);
            ConsoleIO.WriteInfo("File Size: " + unpacker.FileSize + " bytes");
            unpacker.Unpack();
            ConsoleIO.WriteInfo("Sucessfully unpacked file");
            //byte[] shizz;
            //shizz = SevenZipHelper.Compress(FileToRead(@"C:\Users\Ramda_000\Documents\Visual Studio 2012\Projects\Unity3d Decompiler\Unity3d Decompiler\bin\Debug\Modded.txt"));
            //File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Visual Studio 2012\Projects\Unity3d Decompiler\Unity3d Decompiler\bin\Debug\Modded Compressed.txt", shizz);
            Console.Read();
        }
    }
}
