using System;
using System.Collections.Generic;
using System.IO;
using Unity3DDisassembler.IO;

namespace Unity3DDisassembler
{
    class Program
    {
        //TODO: Improve this thing
        static List<string> Commands;

        static void Main(string[] args)
        {
            Console.WindowWidth = 100;

            InitializeCommands();
            if (args.Length > 0)
            {
                if (IsValidCommand(args[0]))
                {
                    if (args.Length == 1) //1 parameter required commands
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
                        else if (args[0] == "list")
                        {
                            ConsoleIO.FullLog = false;
                            Disassembler disassembler = new Disassembler(args[1]);
                            disassembler.Disassemble();
                            disassembler.List();
                        }
                        else if (args[0] == "assemble")
                        {
                            Assembler assembler = new Assembler(args[1]);
                            assembler.Assemble();
                            ConsoleIO.WriteLine("Sucessfully assembled file");
                            assembler.WriteFile("BetaAssembledFile.unity3d");
                            ConsoleIO.WriteLine("Operation done!");
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
                        else if (args[0] == "assemble")
                        {
                            Assembler assembler = new Assembler(args[1]);
                            assembler.Assemble();
                            ConsoleIO.WriteLine("Sucessfully assembled file");
                            assembler.WriteFile(args[2]);
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
        }

        //static void main()
        //{
        //    while (true)
        //    {
        //        Main(Console.ReadLine().Split(' '));
        //    }
        //}

        static void WriteCommads()
        {
            ConsoleIO.WriteLine("Available Commands:");
            ConsoleIO.WriteLine("assemble         parameters: directory");
            ConsoleIO.WriteLine("assemble         parameters: directory, fileName");
            ConsoleIO.WriteLine("extract          parameters: directory");
            ConsoleIO.WriteLine("extractTo        parameters: directory");
            ConsoleIO.WriteLine("list             parameters: directory");
            ConsoleIO.WriteLine("help             parameters: none");
        }

        static void InitializeCommands()
        {
            Commands = new List<string>();
            Commands.Add("assemble");
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
