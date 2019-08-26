namespace VWOSdk
{
    //Common ErrorMessages among different SDKs.
    internal static class LogInfoMessage
    {
        public static void VariationRangeAllocation(string file, string campaignTestKey, string variationName, double variationWeight, double start, double end)
        {
            Log.Info($"({file}): Campaign:{campaignTestKey} having variations:{variationName} with weight:{variationWeight} got range as: ( {start} - {end} ))");
        }
        public static void VariationAllocated(string file, string userId, string campaignTestKey, string variationName)
        {
            Log.Info($"({file}): UserId:{userId} of Campaign:{campaignTestKey} got variation: {variationName}");
        }
        public static void LookingUpUserProfileService(string file, string userId, string campaignTestKey)
        {
            Log.Info($"({file}): Looked into UserProfileService for userId:{userId} and campaign test key: {campaignTestKey} successful");
        }
        public static void SavingDataUserProfileService(string file, string userId)
        {
            Log.Info($"({file}): Saving into UserProfileService for userId:{userId} successful");
        }
        public static void GotStoredVariation(string file, string variationName, string campaignTestKey, string userId)
        {
            Log.Info($"({file}): Got stored variation:{variationName} of campaign:{campaignTestKey} for userId:{userId} from UserProfileService");
        }
        public static void NoVariationAllocated(string file, string userId, string campaignTestKey)
        {
            Log.Info($"({file}): UserId:{userId} of Campaign:{campaignTestKey} did not get any variation");
        }
        public static void UserEligibilityForCampaign(string file, string userId, bool isUserPart)
        {
            Log.Info($"({file}): Is userId:{userId} part of campaign? {isUserPart}");
        }
        public static void AudienceConditionNotMet(string file, string userId)
        {
            Log.Info($"({file}): userId:{userId} does not become part of campaign because of not meeting audience conditions");
        }
        public static void ImpressionSuccess(string file, string endPoint)
        {
            Log.Info($"({file}): Impression event - {endPoint} was successfully received by VWO.");
        }
        public static void RetryFailedImpressionAfterDelay(string file, string endPoint, string retryTimeout)
        {
            Log.Info($"({file}): Failed impression event for {endPoint} will be retried after {retryTimeout} milliseconds delay");
        }
    }
}