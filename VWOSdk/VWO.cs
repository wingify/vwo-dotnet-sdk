#pragma warning disable 1587
/**
 * Copyright 2019-2020 Wingify Software Pvt. Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#pragma warning restore 1587

namespace VWOSdk
{
    public partial class VWO
    {
        private static IValidator Validator;
        private static readonly IBucketService UserHasher;
        private static readonly ICampaignAllocator CampaignAllocator;
        private static readonly IVariationAllocator VariationAllocator;
        private static readonly ISegmentEvaluator SegmentEvaluator;
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
            SegmentEvaluator = new SegmentEvaluator();
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
        public static Settings GetSettingsFile(long accountId, string sdkKey)
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
        /// <param name="userStorageService">UserStorageService to Get and Save User-assigned variations.</param>
        /// <returns>
        /// IVWOClient instance to call Activate, GetVariation and Track apis for given user and goal.
        /// </returns>
        public static IVWOClient CreateInstance(Settings settingFile, bool isDevelopmentMode = false, IUserStorageService userStorageService = null)
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

                var vwoClient = new VWO(accountSettings, Validator, userStorageService, CampaignAllocator, SegmentEvaluator, VariationAllocator, isDevelopmentMode);
                LogDebugMessage.SdkInitialized(file);
                return vwoClient;
            }
            LogErrorMessage.ProjectConfigCorrupted(file);
            return null;
        }
    }
}
