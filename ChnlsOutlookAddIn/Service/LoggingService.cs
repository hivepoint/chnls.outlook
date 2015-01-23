#region

using System;

#endregion

namespace chnls.Service
{
    internal enum LogLevel
    {
        Error,
        Info,
        Warn,
        Debug
    }

    internal class LoggingService
    {
        public static void Debug(string message)
        {
            Log(LogLevel.Debug, message, null);
        }

        public static void Info(string message)
        {
            Log(LogLevel.Info, message, null);
        }

        internal static void Warn(string message)
        {
            Log(LogLevel.Warn, message, null);
        }

        internal static void Error(string message)
        {
            Log(LogLevel.Error, message, null);
        }

        internal static void Error(string message, Exception ex)
        {
            Log(LogLevel.Error, message, ex);
        }


        private static void Log(LogLevel level, string message, Exception ex)
        {
            /*if (message.Length > 1000)
            {
                message = message.Substring(0, 1000) + "...";
            }*/
            System.Diagnostics.Debug.WriteLine(level + ": " + message + (ex != null ? "\n\t" + ex.Message : ""));
        }
    }
}