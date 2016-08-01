using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityDissector.CommandLine;
using UnityDissector.IO;
using UnityDissector.Unity;

namespace UnityDissector
{
    public class Program
    {
        public static CommandLineHandler CommandLineHandler { get; set; }
        public static void Main(string[] args)
        {
            CommandLineHandler = new CommandLineHandler()
            {
                Description = "Program that extracts compressed files from .unity3d format.",
                Options = new List<Option>(new Option[]
                {
                    new Option("-help", 
                               "Prints information on how to use this thing and exits.", 
                               OptionHandlers.HandleHelpOption, 
                               "h"),
                    new Option("-extract", 
                               "Extracts the files inside of the specified .unity3d file and exits.", 
                               OptionHandlers.HandleExtractOption, 
                               "ex"),
                    new Option("-list", 
                               "Prints all files inside of the specified .unity3d file and exits.", 
                               OptionHandlers.HandleListOption, 
                               "l")
                })
            };

            CommandLineHandler.HandleArguments(args);
        }
    }
}
