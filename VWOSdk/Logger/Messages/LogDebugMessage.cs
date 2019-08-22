namespace VWOSdk
{
    //Common ErrorMessages among different SDKs.
    internal static class LogDebugMessage
    {
        public static void LogLevelSet(string file, LogLevel level)
        {
            Log.Debug($"({file}): Log level set to {level.ToString()}");
        }
        //public static void SetColoredLog(string file, string value)
        //{
        //    Log.Debug($"({file}): Colored log set to {value}");
        //}
        public static void SetDevelopmentMode(string file)
        {
            Log.Debug($"({file}): DEVELOPMENT mode is ON");
        }
        public static void ValidConfiguration(string file)
        {
            Log.Debug($"({file}): SDK configuration and account settings are valid.");
        }
        public static void CustomLoggerUsed(string file)
        {
            Log.Debug($"({file}): Custom logger used");
        }
        public static void SdkInitialized(string file)
        {
            Log.Debug($"({file}): SDK properly initialzed");
        }
        public static void SettingsFileProcessed(string file)
        {
            Log.Debug($"({file}): Settings file processed");
        }
        public static void NoStoredVariation(string file, string userId, string campaignTestKey)
        {
            Log.Debug($"({file}): No stored variation for UserId:{userId} for Campaign:{campaignTestKey} found in UserProfileService");
        }
        public static void NoUserProfileServiceLookup(string file)
        {
            Log.Debug($"({file}): No UserProfileService to look for stored data");
        }
        public static void NoUserProfileServiceSave(string file)
        {
            Log.Debug($"({file}): No UserProfileService to save data");
        }
        public static void GettingStoredVariation(string file, string userId, string campaignTestKey, string variationName)
        {
            Log.Debug($"({file}): Got stored variation for UserId:{userId} of Campaign:{campaignTestKey} as Variation: {variationName}, found in UserProfileService");
        }
        public static void CheckUserEligibilityForCampaign(string file, string campaignTestKey, double trafficAllocation, string userId)
        {
            Log.Debug($"({file}): campaign:{campaignTestKey} having traffic allocation:{trafficAllocation} assigned value:{trafficAllocation} to userId:{userId}");
        }
        public static void UserHashBucketValue(string file, string userId, double hashValue, double bucketValue)
        {
            Log.Debug($"({file}): userId:{userId} having hash:{hashValue} got bucketValue:{bucketValue}");
        }
        public static void VariationHashBucketValue(string file, string userId, string campaignTestKey, double percentTraffic, double hashValue, double bucketValue)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignTestKey} having percent traffic:{percentTraffic} got hash-value:{hashValue} and bucket value:{bucketValue}");
        }
        public static void GotVariationForUser(string file, string userId, string campaignTestKey, string variationName, string method)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignTestKey} got variationName:{variationName} inside method:{method}");
        }
        public static void UserNotPartOfCampaign(string file, string userId, string campaignTestKey, string method)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignTestKey} did not become part of campaign, method:{method}");
        }
        public static void UuidForUser(string file, string userId, long accountId, string desiredUuid)
        {
            Log.Debug($"({file}): Uuid generated for userId:{userId} and accountId:{accountId} is {desiredUuid}");
        }
        public static void ImpressionForTrackUser(string file, string properties)
        {
            Log.Debug($"({file}): impression built for track-user - {properties}");
        }
        public static void ImpressionForTrackGoal(string file, string properties)
        {
            Log.Debug($"({file}): impression built for track-goal - {properties}");
        }

    }
}