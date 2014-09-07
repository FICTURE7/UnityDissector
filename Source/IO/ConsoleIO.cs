using System;
using System.IO;

namespace Unity3DDisassembler.IO
{
    public class ConsoleIO
    {
        public static string Log { get; set; }
        public static string LogName = DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss");
        public static string LogPath = AppDomain.CurrentDomain.BaseDirectory + @"logs\" + LogName + ".txt";
        public static bool LogAvailable
        {
            get
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"logs\"))
                    return true;
                else
                    return false;
            }
        }
        //public static ConsoleColor InfoColor = ConsoleColor.DarkMagenta;
        //public static ConsoleColor WarningColor = ConsoleColor.DarkYellow;
        //public static ConsoleColor ErrorColor = ConsoleColor.DarkRed;

        public enum LogType
        {
            Info = 0,
            Warning = 1,
            Error = 2
        };

        public static void WriteLine(string line)
        {
            WriteDate(LogType.Info);
            LogString("CONSOLE OUT:" + line);
            Console.WriteLine(line);
        }

        public static void WriteLine(string line, LogType Type)
        {
            WriteDate(Type);
            LogString("CONSOLE OUT:" + line);
            Console.WriteLine(line);
        }

        public static void LogString(string line, LogType type)
        {
            if (!LogAvailable)
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"logs\");
            File.WriteAllText(LogPath, Log);
            Log = Log + "[" + DateTime.Now.ToString("HH:mm") + "/" + type.ToString().ToUpper() + "]: " + line + System.Environment.NewLine;
        }

        public static void LogString(string line)
        {
            if (!LogAvailable)
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"logs\");
            File.WriteAllText(LogPath, Log);
            Log = Log + "[" + DateTime.Now.ToString("HH:mm") + "/INFO]: " + line + System.Environment.NewLine;
        }

        private static void WriteColor(string line, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(line);
            Console.ResetColor();
        }

        private static void WriteDate(LogType Type)
        {
            Console.Write("[" + DateTime.Now.ToString("HH:mm") + "/" + Type.ToString().ToUpper() + "]: ");
        }
    }
}
