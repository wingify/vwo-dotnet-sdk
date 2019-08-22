using System;

namespace VWOSdk
{
    internal class DefaultLogWriter : ILogWriter
    {
        public void WriteLog(LogLevel logLevel, string message)
        {
            System.Diagnostics.Debug.WriteLine(ToLogMessage(logLevel, message));
        }

        private static string ToLogMessage(LogLevel logLevel, string message)
        {
            message = message ?? string.Empty;
            return $"VWO-SDK - [{logLevel}]: {DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss.fffZ")} {message}";
        }
    }
}