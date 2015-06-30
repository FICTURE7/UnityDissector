using System;
using System.Collections.Generic;
using System.IO;
using Unity3DDisassembler.IO;
using Unity3DDisassembler.Unity;

namespace Unity3DDisassembler
{
    class Program
    {
        //TODO: Improve this thing
        static List<string> Commands;

        static void Main()
        {
            var webArchive = new WebArchive(@"C:\Users\Ramda_000\Documents\GitHub\Unity3D-Disassembler\Source\bin\Debug\WebPortal(1)\WebPortal(1).unity3d");
            webArchive.OpenNew();
            webArchive.Close(true);
            //for (int i = 0; i < webArchive.FileCount; i++)
            //{
            //    Console.WriteLine("FileName: {0}\nSize: {1}\nOffset: {2}\n", webArchive[i].Name,
            //        webArchive[i].Size, webArchive[i].Offset);
            //}
            //webArchive.Extract();

            Console.ReadLine();
        }

        static void WriteCommads()
        {
            ConsoleIO.WriteLine("Available Commands:");
            ConsoleIO.WriteLine("assemble         parameters: directory");
            ConsoleIO.WriteLine("assemble         parameters: directory, fileName");
            ConsoleIO.WriteLine("extract          parameters: directory");
            ConsoleIO.WriteLine("extract          parameters: directory, fileName");
            ConsoleIO.WriteLine("list             parameters: directory");
            ConsoleIO.WriteLine("help             parameters: none");
        }

        static void InitializeCommands()
        {
            Commands = new List<string>();
            Commands.Add("assemble");
            Commands.Add("extract");
            Commands.Add("extract");
            Commands.Add("list");
            Commands.Add("help");
        }

        static bool IsValidCommand(string cmd)
        {
            if (Commands.Contains(cmd))
                return true;
            else
                return false;
        }
    }
}
