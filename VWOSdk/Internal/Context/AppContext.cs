namespace VWOSdk
{
    internal class AppContext
    {
        private static readonly string file = typeof(AppContext).FullName;

        internal static IApiCaller ApiCaller { get; set; } = new ApiCaller();
        internal static ILogWriter Logger { get; set; } = new DefaultLogWriter();
        internal static LogLevel LogLevel { get; set; } = LogLevel.ERROR;

        internal static void Configure(IApiCaller apiCaller)
        {
            ApiCaller = apiCaller;
        }

        internal static void Configure(ILogWriter logger)
        {
            if (logger != null)
            {
                Logger = logger;
                LogDebugMessage.CustomLoggerUsed(file);
            }
            else
            {
                Logger = new DefaultLogWriter();
                LogErrorMessage.CustomLoggerMisconfigured(file);
            }
        }

        internal static void Configure(LogLevel logLevel)
        {
            LogLevel = logLevel;
            LogDebugMessage.LogLevelSet(file, logLevel);
        }
    }
}