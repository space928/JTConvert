using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JTConvert
{
    public static class Logger
    {
#if DEBUG
        private static VerbosityLevel verbosity = VerbosityLevel.DEBUG;
#else
        private static VerbosityLevel verbosity = VerbosityLevel.INFO;
#endif
        private static readonly object ConsoleWriterLock = new object();

        public static VerbosityLevel Verbosity { get => verbosity; set => verbosity = value; }

        public static void Log(object message, VerbosityLevel level = VerbosityLevel.INFO, string prefix = null, [CallerFilePath]string caller = null, [CallerMemberName]string method = null)
        {
            if (level < verbosity) 
                return;
            lock (ConsoleWriterLock)
            {
                Console.ForegroundColor = verbosityToColour[(int)level];
                Console.WriteLine($"[{DateTime.Now:G}] [{Path.GetFileNameWithoutExtension(caller)}::{method}]{(string.IsNullOrEmpty(prefix) ? "" : $" [{prefix}]")} {message}");
            }
        }

        private static readonly ConsoleColor[] verbosityToColour = 
        { 
            ConsoleColor.Gray, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Red 
        };

        public enum VerbosityLevel
        {
            DEBUG = 0,
            INFO = 1,
            WARN = 2,
            ERROR = 3,
        }
    }
}
