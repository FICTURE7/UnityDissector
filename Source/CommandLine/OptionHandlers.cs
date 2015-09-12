using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDissector.Unity;

namespace UnityDissector.CommandLine
{
    public static class OptionHandlers
    {
        public static void HandleExtractOption(CommandLineHandler handler, string file)
        {
            var webArchive = new WebArchive(file);
            Console.WriteLine("Openning archive...");
            webArchive.Open();
            Console.WriteLine("Extracting {0} files...", webArchive.FileCount);
            webArchive.Extract();
            Console.WriteLine("Done");
        }

        public static void HandleHelpOption(CommandLineHandler handler, string file)
        {
            handler.PrintUsage();
            Console.WriteLine();
            handler.PrintOptions();
        }

        public static void HandleListOption(CommandLineHandler handler, string file)
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
}
