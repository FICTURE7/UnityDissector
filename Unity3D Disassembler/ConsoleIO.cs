using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity3DDisassembler
{
    class ConsoleIO
    {
        private static string log;

        private static string time_started = DateTime.Now.ToString("t").Replace(':', '.');

        public enum LogType
        {
            Info = 0,
            Warning = 1,
            Error = 3
        };

        public static void WriteInfo(string line)
        {
            WriteColor("Info: ", ConsoleColor.DarkMagenta);
            Log("CONSOLE OUT:" + line);
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
            Log("CONSOLE OUT:" + line);
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
            Log("CONSOLE OUT:" + line);
            Console.WriteLine(line);
        }

        public static void WriteError(long line)
        {
            WriteColor("Error: ", ConsoleColor.DarkRed);
            Console.WriteLine(line);
        }

        public static void Log(string line, LogType type)
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Console Debug\");
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Console Debug\" + time_started + ".txt";
            File.WriteAllText(path, log);
            log = log + "[" + DateTime.Now.ToString("t") + "/" + type.ToString().ToUpper() + "] " +  line + System.Environment.NewLine;
        }

        public static void Log(string line)
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Console Debug\");
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Console Debug\" + time_started + ".txt";
            File.WriteAllText(path, log);
            log = log + "[" + DateTime.Now.ToString("t") + "/" + "INFO" + "] " + line + System.Environment.NewLine;
        }

        static void WriteColor(string line, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(line);
            Console.ResetColor();
        }
    }
}
