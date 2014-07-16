using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3dDecompiler
{
    class ConsoleIO
    {
        public static void WriteInfo(string line)
        {
            WriteColor("Info: ", ConsoleColor.DarkMagenta);
            Console.WriteLine(line);
        }

        public static void WriteInfo(long line)
        {
            WriteColor("Info: ", ConsoleColor.DarkMagenta);
            Console.WriteLine(line);
        }

        public static void WriteWarning(string line)
        {
            WriteColor("Warning: ", ConsoleColor.DarkYellow);
            Console.WriteLine(line);
        }

        public static void WriteWarning(long line)
        {
            WriteColor("Warning: ", ConsoleColor.DarkYellow);
            Console.WriteLine(line);
        }

        public static void WriteError(string line)
        {
            WriteColor("Error: ", ConsoleColor.DarkRed);
            Console.WriteLine(line);
        }

        public static void WriteError(long line)
        {
            WriteColor("Error: ", ConsoleColor.DarkRed);
            Console.WriteLine(line);
        }

        static void WriteColor(string line, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(line);
            Console.ResetColor();
        }
    }
}
