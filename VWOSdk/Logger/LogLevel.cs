namespace VWOSdk
{
    /// <summary>
    /// Defines the types of logs to written from SDK.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Write only Error logs.
        /// </summary>
        ERROR = 0,
        /// <summary>
        /// Write Error and Warning logs.
        /// </summary>
        WARNING = 1,
        /// <summary>
        /// Write Error, Warning and Info logs.
        /// </summary>
        INFO = 2,
        /// <summary>
        /// Write Error, Warning, Info and Debug logs.
        /// </summary>
        DEBUG = 3
    }
}