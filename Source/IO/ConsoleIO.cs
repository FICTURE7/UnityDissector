using System;
using System.IO;

namespace Unity3DDisassembler.IO
{
    public class ConsoleIO
    {
        public static bool FullLog { get; set; }
        public static string OutLog { get; set; }
        public static string OutLogName = DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss");
        public static string OutLogPath = AppDomain.CurrentDomain.BaseDirectory + @"logs\" + OutLogName + ".txt";
        public static bool OutLogAvailable
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
            Console.WriteLine(line);
            if (!FullLog) Log("[Console]" + line);
        }

        public static void WriteLine(string line, LogType Type)
        {
            WriteDate(Type);
            Console.WriteLine(line);
            if (!FullLog) Log("[Console]" + line);
        }

        public static void Log(string line)
        {
            if (!OutLogAvailable)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"logs\");
            }
            if (FullLog)
            {
                WriteDate(LogType.Info);
                Console.WriteLine(line);
            }

            OutLog = OutLog + "[" + DateTime.Now.ToString("HH:mm") + "/INFO]" + line + System.Environment.NewLine;
            File.WriteAllText(OutLogPath, OutLog);
        }

        public static void Log(string line, LogType type)
        {
            if (!OutLogAvailable)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"logs\");
            }
            if (FullLog)
            {
                WriteDate(type);
                Console.WriteLine(line);
            }

            OutLog = OutLog + "[" + DateTime.Now.ToString("HH:mm") + "/" + type.ToString().ToUpper() + "]" + line + System.Environment.NewLine;
            File.WriteAllText(OutLogPath, OutLog);
        }

        private static void WriteDate(LogType Type)
        {
            Console.Write("[" + DateTime.Now.ToString("HH:mm") + "/" + Type.ToString().ToUpper() + "]: ");
        }
    }
}
