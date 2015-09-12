using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace UnityDissector.CommandLine
{
    public class CommandLineHandler
    {
        public CommandLineHandler()
        {
            Options = new List<Option>();
        }

        public string Description { get; set; }
        public List<Option> Options { get; set; }

        public void HandleArguments(string[] args)
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
            option.Handle(this, filePath);
        }

        public void PrintUsage()
        {
            Console.WriteLine("\t{0}\n", Description);
            Console.WriteLine("Usage:\n\tUnityDissector [OPTIONS] filepath");
        }

        public void PrintOptions()
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

        private bool TryGetOption(string optionName, out Option option)
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
