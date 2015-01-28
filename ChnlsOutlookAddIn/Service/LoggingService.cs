#region

using System;
using System.ComponentModel;
using System.Diagnostics;

#endregion

namespace chnls.Service
{

    internal class LoggingService
    {

        private static EventLog _eventLog;
        private static bool _loggingEnabled = true;

        private static void Init()
        {
            if (null != _eventLog)
            {
                return;
            }
            _eventLog = new EventLog { Source = "Outlook" };
            ((ISupportInitialize)(_eventLog)).BeginInit();
            if (!EventLog.SourceExists(_eventLog.Source))
            {
                _loggingEnabled = false;
                Error("Unable to start logging, the source 'Outlook' does not exist");
            }
            ((ISupportInitialize)(_eventLog)).EndInit();
        }

        public static void Debug(string message)
        {
            Log(EventLogEntryType.Information, "DEBUG:" + message, null);
        }

        public static void Info(string message)
        {
            Log(EventLogEntryType.Information, message, null);
        }

        internal static void Warn(string message)
        {
            Log(EventLogEntryType.Warning, message, null);
        }

        internal static void Error(string message)
        {
            Log(EventLogEntryType.Error, message, null);
        }

        internal static void Error(string message, Exception ex)
        {
            Log(EventLogEntryType.Error, message, ex);
        }


        private static void Log(EventLogEntryType level, string message, Exception ex)
        {
            Init();
            System.Diagnostics.Debug.WriteLine(level + ": " + message + (ex != null ? "\n\t" + ex.Message : ""));

            if (message.Length > 5000)
            {
                message = message.Substring(0, 5000);
            }
            if (null != ex)
            {
                message += "\n" + ex;
            }
            if (!_loggingEnabled) return;
            try
            {
                _eventLog.WriteEntry(message, level);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }
    }
}