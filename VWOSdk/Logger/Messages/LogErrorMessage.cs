namespace VWOSdk
{
    //Common ErrorMessages among different SDKs.
    internal static class LogErrorMessage
    {
        public static void ProjectConfigCorrupted(string file)
        {
            Log.Error($"({file}): config passed to createInstance is not a valid object.");
        }
        //public static void INVALID_CONFIGURATION(string file)
        //{
        //    Log.Error($"({file}): SDK configuration or account settings or both is/are not valid.");
        //}
        public static void SettingsFileCorrupted(string file)
        {
            Log.Error($"({file}): Settings file is corrupted. Please contact VWO Support for help.");
        }
        public static void ApiMissingParams(string file, string apiName, string paramName)
        {
            Log.Error($"({file}): \"{apiName}\" API got bad parameters. Invalid parameter: {paramName}.");
        }
        //public static void ACTIVATE_API_CONFIG_CORRUPTED(string file)
        //{
        //    Log.Error($"({file}): \"activate\" API has corrupted configuration");
        //}
        //public static void GET_VARIATION_API_MISSING_PARAMS(string file)
        //{
        //    Log.Error($"({file}): \"getVariation\" API got bad parameters. It expects campaignTestKey(String) as first and userId(String) as second argument");
        //}
        //public static void GET_VARIATION_API_CONFIG_CORRUPTED(string file)
        //{
        //    Log.Error($"({file}): \"getVariation\" API has corrupted configuration");
        //}
        //public static void TRACK_API_MISSING_PARAMS(string file)
        //{
        //    Log.Error($"({file}): \"track\" API got bad parameters. It expects campaignTestKey(String) as first, userId(String) as second and goalIdentifier(String/Number) as third argument. Fourth is revenueValue(Float/Number/String) and is required for revenue goal only.");
        //}
        //public static void TRACK_API_CONFIG_CORRUPTED(string file)
        //{
        //    Log.Error($"({file}): \"track\" API has corrupted configuration");
        //}
        public static void TrackApiVariationNotFound(string file, string campaignTestKey, string userId)
        {
            Log.Error($"({file}): Variation not found for campaign:{campaignTestKey} and userId:{userId}");
        }
        public static void CampaignNotRunning(string file, string campaignTestKey, string apiName = null)
        {
            apiName = apiName ?? string.Empty;
            Log.Error($"({file}): API used:{apiName} - Campaign:{campaignTestKey} is not RUNNING. Please verify from VWO App");
        }
        public static void LookUpUserProfileServiceFailed(string file, string userId, string campaignTestKey)
        {
            Log.Error($"({file}): Lookup method could not provide us the stored variation for User Id: {userId} and Campaign test key: {campaignTestKey}. Please check your User Profile Service lookup implementation.");
            //Log.Error($"({file}): Looking data from UserProfileService failed for userId:{userId}");
        }
        public static void SaveUserProfileServiceFailed(string file, string userId)
        {
            Log.Error($"({file}): Saving data into UserProfileService failed for userId:{userId}");
        }
        //public static void InvalidCampaign(string file, string method)
        //{
        //    Log.Error($"({file}): Invalid campaign passed to {method} of this file");
        //}
        //public static void INVALID_USER_ID(string file, string userId, string method)
        //{
        //    Log.Error($"({file}): Invalid userId:{userId} passed to {method} of this file");
        //}
        public static void ImpressionFailed(string file, string endPoint)
        {
            Log.Error($"({file}): Impression event could not be sent to VWO - {endPoint}");
        }

        internal static void TrackApiRevenueNotPassedForRevenueGoal(string file, string goalIdentifier, string campaignTestKey, string userId)
        {
            Log.Error($"({file}): Revenue value should be passed for revenue goal:{goalIdentifier} for campaign:{campaignTestKey} and userId:{userId}");
        }

        internal static void TrackApiGoalNotFound(string file, string goalIdentifier, string campaignTestKey, string userId)
        {
            Log.Error($"({file}): Goal:{goalIdentifier} not found for campaign:{campaignTestKey} and userId:{userId}");
        }

        public static void CustomLoggerMisconfigured(string file)
        {
            Log.Error($"({file}): custom logger is provided but seems to have misconfigured. please check the api docs. using default logger.");
        }

    }
}