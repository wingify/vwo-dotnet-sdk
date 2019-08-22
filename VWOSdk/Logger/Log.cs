using System;

namespace VWOSdk
{
    internal static class Log
    {
        private static ILogWriter Logger { get { return AppContext.Logger; } }
        private static LogLevel LogLevel { get { return AppContext.LogLevel; } }
        
        internal static void Debug(string message)
        {
            if (LogLevel.IsLogTypeEnabled(LogLevel.DEBUG))
                TryLog(() => Logger?.WriteLog(LogLevel.DEBUG, message));
        }
        
        internal static void Error(string message)
        {
            if (LogLevel.IsLogTypeEnabled(LogLevel.ERROR))
                TryLog(() => Logger?.WriteLog(LogLevel.ERROR, message));
        }

        internal static void Info(string message)
        {
            if (LogLevel.IsLogTypeEnabled(LogLevel.INFO))
                TryLog(() => Logger?.WriteLog(LogLevel.INFO, message));
        }

        internal static void Warning(string message)
        {
            if (LogLevel.IsLogTypeEnabled(LogLevel.WARNING))
                TryLog(() => Logger?.WriteLog(LogLevel.WARNING, message));
        }

        private static void TryLog(Action logFunc)
        {
            try
            {
                logFunc?.Invoke();
            }
            catch { }
        }
    }
}