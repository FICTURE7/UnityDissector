using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityDissector.IO;
using UnityDissector.Unity;

namespace UnityDissector
{
    public class Program
    {
        private static class OptionHandlers
        {
            public static void HandleExtractOption(string file)
            {
                var webArchive = new WebArchive(file);
                Console.WriteLine("Openning archive...");
                webArchive.Open();
                Console.WriteLine("Extracting {0} files...", webArchive.FileCount);
                webArchive.Extract();
                Console.WriteLine("Done");
            }

            public static void HandleHelpOption(string file)
            {
                PrintUsage();
                Console.WriteLine();
                PrintOptions();
            }

            public static void HandleListOption(string file)
            {
                var webArchive = new WebArchive(file);
                Console.WriteLine("Openning archive...");
                webArchive.Open();

                Console.WriteLine(" Name".PadRight(50) + "| Size");
                Console.WriteLine("{0}{1}{0}", new string('-', 50), '+', new string('-', 6 + 10));
                for (int i = 0; i < webArchive.FileCount; i++)
                {
                    Console.WriteLine(" {0} {1}", webArchive[i].Name.PadRight(50), webArchive[i].Size.ToString());
                }
            }
        }

        private class Option
        {
            public delegate void OptionHandler(string file);

            public Option(string name, OptionHandler handler)
            {
                Name = name;
                Alias = new string[0];
                Handler = handler;
            }

            public Option(string name, OptionHandler handler, params string[] alias)
            {
                Name = name;
                Alias = alias;
                Handler = handler;
            }

            public Option(string name, string description, OptionHandler handler, params string[] alias)
            {
                Name = name;
                Description = description;
                Alias = alias;
                Handler = handler;
            }

            public string Name { get; set; }
            public string[] Alias { get; set; }
            public string Description { get; set; }

            private OptionHandler Handler { get; set; }

            public void Handle(string file)
            {
                Handler(file);
            }
        }

        private static List<Option> Options = new List<Option>(new Option[]
        {
            new Option("-help", 
                       "Prints information on how to use this thing and exits", 
                       OptionHandlers.HandleHelpOption, 
                       "h"),
            new Option("-extract", 
                       "Extracts the files inside of the specified .unity3d file.", 
                       OptionHandlers.HandleExtractOption, 
                       "ex"),
            new Option("-list", 
                       "Prints all files inside of the specified .unity3d file.", 
                       OptionHandlers.HandleListOption, 
                       "l")
        });

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                Console.WriteLine();
                PrintOptions();
                Environment.Exit(1);
            }

            var filePath = string.Empty;
            var option = (Option)null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i][0] == '-') // must be an option
                {
                    var optionName = args[i].Substring(1);
                    if (!TryGetOption(optionName, out option))
                    {
                        PrintUsage();
                        Console.WriteLine();
                        PrintOptions();

                        Console.WriteLine("ERROR: Unknown option at {0} option.", i + 1);
                        Environment.Exit(1);
                    }
                }
                else
                {
                    filePath = args[i];
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("ERROR: File does not exist {0}", filePath);
                        Environment.Exit(1);
                    }
                    if (i != args.Length - 1)
                        Console.WriteLine("WARN: Igonring extra argument(s) after {0}", i + 1);
                    break;
                }
            }
            option.Handle(filePath);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Programs that can extract compressed files from .unity3d format.");
            Console.WriteLine("Usage:\n\tUnityDissector [OPTIONS] filepath");
        }

        private static void PrintOptions()
        {
            Console.WriteLine("Options:");
            for (int i = 0; i < Options.Count; i++)
            {
                var description = Options[i].Description;
                var namesAndAlias = "-" + Options[i].Name;
                if (Options[i].Alias.Length > 0)
                    namesAndAlias += ", -" + string.Join(", -", Options[i].Alias);

                Console.WriteLine("\t{0}{1}", namesAndAlias.PadRight(30), description);
            }
        }

        private static bool TryGetOption(string optionName, out Option option)
        {
            var options = Options.Where(opt => opt.Name == optionName || opt.Alias.Contains(optionName));
            if (options.Count() > 1)
                throw new InvalidOperationException("Something went wrong when processing options.");

            if (options.Count() == 1)
            {
                option = options.First();
                return true;
            }
            else
            {
                option = null;
                return false;
            }
        }
    }
}
