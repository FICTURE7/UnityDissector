using System;
using Unity3DDisassembler.IO;

namespace Unity3DDisassembler
{
    class Program
    {
        static void Main()
        {
            Disassembler unpacker = new Disassembler(@"C:\Users\Ramda_000\Documents\Unity Test\Test\Testss.unity3d");
            ConsoleIO.WriteLine("File Path: " + unpacker.FilePath);
            ConsoleIO.WriteLine("File Name: " + unpacker.FileName);
            ConsoleIO.WriteLine("File Extension: " + unpacker.FileExtension);
            ConsoleIO.WriteLine("File Size: " + unpacker.FileSize + " bytes");
            unpacker.Disassemble();
            ConsoleIO.WriteLine("Sucessfully disassembled file");
            ConsoleIO.WriteLine("Extracting files at dir: " + AppDomain.CurrentDomain.BaseDirectory + unpacker.FileName);
            unpacker.Extract();
            ConsoleIO.WriteLine("Operation done!");
            Console.ReadLine();


            //byte[] shizz;
            //shizz = SevenZipHelper.Compress(unpacker.DecompressedFile.Bytes);

            //byte[] test = null;
            //test = concatBytes(unpacker.CompressedFile.HeaderBytes, shizz);
            //File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Unity Test\Test\Stuff and things.unity3d", test);

            //Repacker repacker = new Repacker(@"C:\Users\Ramda_000\Documents\Git\Unity3D-Deompiler\Unity3d Decompiler\bin\Debug\Testss");
            //repacker.Repack();
            //Console.Read();
        }
    }
}
