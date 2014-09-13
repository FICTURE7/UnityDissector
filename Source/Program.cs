using System;
using System.Collections.Generic;
using System.IO;
using Unity3DDisassembler.IO;

namespace Unity3DDisassembler
{
    class Program
    {
        static List<string> Commands;

        static void Main(string[] args)
        {
            Console.WindowWidth = 100;

            InitializeCommands();
            if (args.Length > 0)
            {
                if (IsValidCommand(args[0]))
                {
                    if (args.Length == 1)
                    {
                        if (args[0] == "help")
                        {
                            WriteCommads();
                        }
                    }

                    if (args.Length == 2) //2 parameter required commands
                    {
                        if (args[0] == "extract")
                        {
                            ConsoleIO.FullLog = true;
                            Disassembler disassembler = new Disassembler(args[1]);
                            disassembler.Disassemble();
                            ConsoleIO.WriteLine("Sucessfully disassembled file");
                            ConsoleIO.WriteLine("Extracting files at directory: " + "\\" + disassembler.FileName);
                            disassembler.Extract();
                            ConsoleIO.WriteLine("Operation done!");
                        }
                        if (args[0] == "list")
                        {
                            ConsoleIO.FullLog = false;
                            Disassembler disassembler = new Disassembler(args[1]);
                            disassembler.Disassemble();
                            disassembler.List();
                        }
                    }

                    if (args.Length == 3) //3 parameter required commands
                    {
                        if (args[0] == "extractTo")
                        {
                            ConsoleIO.FullLog = true;
                            Disassembler disassembler = new Disassembler(args[1]);
                            disassembler.Disassemble();
                            ConsoleIO.WriteLine("Sucessfully disassembled file");
                            ConsoleIO.WriteLine("Extracting files at directory: " + args[2]);
                            disassembler.ExtractTo(args[2]);
                            ConsoleIO.WriteLine("Operation done!");
                        }
                    }
                }
                else
                {
                    ConsoleIO.WriteLine("Unknown command");
                    ConsoleIO.WriteLine("");
                    WriteCommads();
                }
            }
            else
            {
                WriteCommads();
            }


            //byte[] shizz;
            //shizz = SevenZipHelper.Compress(unpacker.DecompressedFile.Bytes);

            //byte[] test = null;
            //test = concatBytes(unpacker.CompressedFile.HeaderBytes, shizz);
            //File.WriteAllBytes(@"C:\Users\Ramda_000\Documents\Unity Test\Test\Stuff and things.unity3d", test);

            //Repacker repacker = new Repacker(@"C:\Users\Ramda_000\Documents\Git\Unity3D-Deompiler\Unity3d Decompiler\bin\Debug\Testss");
            //repacker.Repack();
            //Console.Read();
        }

        static void WriteCommads()
        {
            ConsoleIO.WriteLine("Available Commands:");
            ConsoleIO.WriteLine("extract          parameters: directory");
            ConsoleIO.WriteLine("extractTo        parameters: directory");
            ConsoleIO.WriteLine("list             parameters: directory");
            ConsoleIO.WriteLine("help             parameters: none");
        }

        static void InitializeCommands()
        {
            Commands = new List<string>();
            Commands.Add("extract");
            Commands.Add("extractTo");
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
