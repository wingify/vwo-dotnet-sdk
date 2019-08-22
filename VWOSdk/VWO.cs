namespace VWOSdk
{
    public partial class VWO
    {
        private static IValidator Validator;
        private static readonly IBucketService UserHasher;
        private static readonly ICampaignAllocator CampaignAllocator;
        private static readonly IVariationAllocator VariationAllocator;
        private static ISettingsProcessor SettingsProcessor;
        private static readonly string file = typeof(VWO).FullName;

        /// <summary>
        /// Static Constructor to init default dependencies on application load.
        /// </summary>
        static VWO()
        {
            Validator = new Validator();
            UserHasher = new Murmur32BucketService();
            CampaignAllocator = new CampaignAllocator(UserHasher);
            VariationAllocator = new VariationAllocator(UserHasher);
            SettingsProcessor = new SettingsProcessor();
        }

        /// <summary>
        /// Configure Custom logger for application.
        /// </summary>
        /// <param name="logger">Custom logger instance to log from within the sdk. Null means default logger will be used.</param>
        public static void Configure(ILogWriter logger)
        {
            AppContext.Configure(logger);
        }

        /// <summary>
        /// Configure LogLevel for application.
        /// </summary>
        /// <param name="logLevel">Defines the level of logs. LogLevel can be ERROR, WARNING, INFO and DEBUG.</param>
        public static void Configure(LogLevel logLevel)
        {
            AppContext.Configure(logLevel);
        }

        /// <summary>
        /// This is for internal unit testing purpose. Not to be exposed publicly.
        /// </summary>
        /// <param name="validator"></param>
        internal static void Configure(IValidator validator)
        {
            Validator = validator;
        }
        /// <summary>
        /// This is for internal unit testing purpose. Not to be exposed publicly.
        /// </summary>
        /// <param name="settingsProcessor"></param>
        internal static void Configure(ISettingsProcessor settingsProcessor)
        {
            SettingsProcessor = settingsProcessor;
        }

        /// <summary>
        /// Fetch SettingsFile for provided accountId and sdkKey.
        /// </summary>
        /// <param name="accountId">ID for VWO Account.</param>
        /// <param name="sdkKey">SdkKey for Server-Side application.</param>
        /// <returns>
        /// Fetch Settings for valid accountId and sdkKey.
        /// Null for invalid parameters, unable to connect to VWO, etc.
        /// </returns>
        public static Settings GetSettings(long accountId, string sdkKey)
        {
            if (Validator.GetSettings(accountId, sdkKey))
            {
                ApiRequest apiRequest = ServerSideVerb.SettingsRequest(accountId, sdkKey);
                var settings = apiRequest.Execute<Settings>();
                if(settings == null)
                {
                    LogErrorMessage.SettingsFileCorrupted(file);
                }
                return settings;
            }
            return default(Settings);
        }

        /// <summary>
        /// Instantiate a VWOClient to call Activate, GetVariation and Track apis for given user and goal.
        /// </summary>
        /// <param name="settingFile">Settings as provided by GetSettings call.</param>
        /// <param name="isDevelopmentMode">When running in development or non-production mode. This ensures no operations are tracked on VWO account.</param>
        /// <param name="userProfileService">UserProfileService to Lookup and Save User-assigned variations.</param>
        /// <returns>
        /// IVWOClient instance to call Activate, GetVariation and Track apis for given user and goal.
        /// </returns>
        public static IVWOClient Instantiate(Settings settingFile, bool isDevelopmentMode = false, IUserProfileService userProfileService = null)
        {
            if (Validator.SettingsFile(settingFile))
            {
                LogDebugMessage.ValidConfiguration(file);
                AccountSettings accountSettings = SettingsProcessor.ProcessAndBucket(settingFile);
                LogDebugMessage.SettingsFileProcessed(file);
                if (accountSettings == null)
                    return null;

                if(isDevelopmentMode)
                    LogDebugMessage.SetDevelopmentMode(file);

                var vwoClient = new VWO(accountSettings, Validator, userProfileService, CampaignAllocator, VariationAllocator, isDevelopmentMode);
                LogDebugMessage.SdkInitialized(file);
                return vwoClient;
            }
            LogErrorMessage.ProjectConfigCorrupted(file);
            return null;
        }
    }
}