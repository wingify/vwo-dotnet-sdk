namespace VWOSdk
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// Write Log Message for given LogLevel.
        /// </summary>
        /// <param name="logLevel">LogLevel for Log message.</param>
        /// <param name="message">Log Message.</param>
        void WriteLog(LogLevel logLevel, string message);
    }
}