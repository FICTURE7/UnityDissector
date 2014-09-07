using System;
using Unity3DDisassembler.Common;

namespace Unity3DDisassembler
{
    class Program
    {
        static void Main()
        {
            Disassembler unpacker = new Disassembler(@"C:\Users\Ramda_000\Documents\Unity Test\Test\Testss.unity3d");
            ConsoleIO.WriteInfo("File Path: " + unpacker.FilePath);
            ConsoleIO.WriteInfo("File Name: " + unpacker.FileName);
            ConsoleIO.WriteInfo("File Extension: " + unpacker.FileExtension);
            ConsoleIO.WriteInfo("File Size: " + unpacker.FileSize + " bytes");
            unpacker.Disassemble();
            unpacker.Extract();
            ConsoleIO.WriteInfo("Sucessfully unpacked file");
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
