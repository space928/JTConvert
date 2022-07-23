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
        private static readonly object ConsoleWriterLock = new();

        /// <summary>
        /// Gets/sets the global verbosity of the <seealso cref="Logger"/>. Messages of a lower 
        /// verbosity than this will not be logged.
        /// </summary>
        public static VerbosityLevel Verbosity { get => verbosity; set => verbosity = value; }

        /// <summary>
        /// Shorthand for the <seealso cref="Log(object, VerbosityLevel, string, string, string)"/>
        /// method with the <seealso cref="VerbosityLevel.DEBUG"/> verbosity.
        /// </summary>
        /// <param name="message">Message to be logged to the console</param>
        public static void Debug(object message, [CallerFilePath] string caller = null, [CallerMemberName] string method = null) => Log(message, VerbosityLevel.DEBUG, caller:caller, method:method);

        /// <summary>
        /// Logs a message to the debug console.
        /// </summary>
        /// <remarks>
        /// Adds some pretty formatting and extra information like the calling class/method and the time. <para/>
        /// Messages with a verbosity lower than the <seealso cref="Logger"/>'s verbosity will not
        /// be logged.
        /// </remarks>
        /// <param name="message">Message to be logged to the console</param>
        /// <param name="level">Verbosity level to log the message with</param>
        /// <param name="prefix">Optional prefix for the message</param>
        /// <param name="caller">[Automatically filled in]</param>
        /// <param name="method">[Automatically filled in]</param>
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
