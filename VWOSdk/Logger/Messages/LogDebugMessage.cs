#pragma warning disable 1587
/**
 * Copyright 2019-2021 Wingify Software Pvt. Ltd.
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
    //Common ErrorMessages among different SDKs.
    internal static class LogDebugMessage
    {
        public static void LogLevelSet(string file, LogLevel level, bool disableLogs = false)
        {
            Log.Debug($"({file}): Log level set to {level.ToString()}", disableLogs);
        }
        //public static void SetColoredLog(string file, string value)
        //{
        //    Log.Debug($"({file}): Colored log set to {value}");
        //}
        public static void SetDevelopmentMode(string file, bool disableLogs = false)
        {
            Log.Debug($"({file}): DEVELOPMENT mode is ON", disableLogs);
        }
        public static void ValidConfiguration(string file, bool disableLogs = false)
        {
            Log.Debug($"({file}): SDK configuration and account settings are valid.", disableLogs);
        }
        public static void CustomLoggerUsed(string file, bool disableLogs = false)
        {
            Log.Debug($"({file}): Custom logger used", disableLogs);
        }
        public static void SdkInitialized(string file, bool disableLogs = false)
        {
            Log.Debug($"({file}): SDK properly initialzed", disableLogs);
        }
        public static void SettingsFileProcessed(string file, bool disableLogs = false)
        {
            Log.Debug($"({file}): Settings file processed", disableLogs);
        }
        public static void NoStoredVariation(string file, string userId, string campaignKey, bool disableLogs = false)
        {
            Log.Debug($"({file}): No stored variation for UserId:{userId} for Campaign:{campaignKey} found in UserStorageService", disableLogs);
        }
        public static void NoUserStorageServiceGet(string file, bool disableLogs = false)
        {
            Log.Debug($"({file}): No UserStorageService to look for stored data", disableLogs);
        }
        public static void NoUserStorageServiceSet(string file, bool disableLogs = false)
        {
            Log.Debug($"({file}): No UserStorageService to set data", disableLogs);
        }

        public static void CheckUserEligibilityForCampaign(string file, string campaignKey, double trafficAllocation, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): campaign:{campaignKey} having traffic allocation:{trafficAllocation} assigned value:{trafficAllocation} to userId:{userId}", disableLogs);
        }
        public static void UserHashBucketValue(string file, string userId, double hashValue, double bucketValue, bool disableLogs = false)
        {
            Log.Debug($"({file}): userId:{userId} having hash:{hashValue} got bucketValue:{bucketValue}", disableLogs);
        }
        public static void VariationHashBucketValue(string file, string userId, string campaignKey, double percentTraffic, double hashValue, double bucketValue, bool disableLogs = false)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignKey} having percent traffic:{percentTraffic} got hash-value:{hashValue} and bucket value:{bucketValue}", disableLogs);
        }
        public static void GotVariationForUser(string file, string userId, string campaignKey, string variationName, string method, bool disableLogs = false)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignKey} got variationName:{variationName} inside method:{method}", disableLogs);
        }
        public static void UserNotPartOfCampaign(string file, string userId, string campaignKey, string method, bool disableLogs = false)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignKey} did not become part of campaign, method:{method}", disableLogs);
        }
        public static void UuidForUser(string file, string userId, long accountId, string desiredUuid, bool disableLogs = false)
        {
            Log.Debug($"({file}): Uuid generated for userId:{userId} and accountId:{accountId} is {desiredUuid}", disableLogs);
        }
        public static void ImpressionForTrackUser(string file, string properties, bool disableLogs = false)
        {
            Log.Debug($"({file}): impression built for track-user - {properties}", disableLogs);
        }
        public static void ImpressionForBatchEvent(string file, string properties, bool disableLogs = false)
        {
            Log.Debug($"({file}): impression built for track-user - {properties}", disableLogs);
        }
        public static void ImpressionForTrackGoal(string file, string properties, bool disableLogs = false)
        {
            Log.Debug($"({file}): impression built for track-goal - {properties}", disableLogs);
        }
        public static void ImpressionForPushTag(string file, string properties, bool disableLogs = false)
        {
            Log.Debug($"({file}): impression built for push-tags - {properties}", disableLogs);
        }
        public static void SkippingSegmentation(string file, string userId, string campaignKey, string apiName, string variationName, bool disableLogs = false)
        {
            Log.Debug($"({file}): In API: {apiName}, Skipping segmentation for UserId:{userId} in campaing:{campaignKey} for variation: {variationName} as no valid segment is found", disableLogs);
        }
        public static void SegmentationStatus(string file, string userId, string campaignKey, string apiName, string variationName, string status, bool disableLogs = false)
        {
            Log.Debug($"({file}): In API: {apiName}, Whitelisting for UserId:{userId} in campaing:{campaignKey} for variation: {variationName} is: {status}", disableLogs);
        }


        //Batch Event
        public static void RequestTimeIntervalOutOfBound(string file, int min_value, int default_value, bool disableLogs = false)
        {
            Log.Debug($"({file}): requestTimeInterval should be > {min_value.ToString()}. Assigning it the default value i.e {default_value.ToString()} seconds,", disableLogs);
        }
        public static void EventsPerRequestOutOfBound(string file, int min_value, int max_value, int default_value, bool disableLogs = false)
        {
            Log.Debug($"({file}): eventsPerRequest should be > {min_value.ToString()} and <= {max_value.ToString()}. Assigning it the default value i.e {default_value.ToString()}", disableLogs);
        }

        public static void EventBatchingNotActivated(string file, string function, bool disableLogs = false)
        {
            Log.Debug($"({file}): Event batching is not activated for {function}  or send null", disableLogs);
        }
        public static void EventBatchingActivated(string file, string function, bool disableLogs = false)
        {
            Log.Debug($"({file}): Event added in queue for {function}", disableLogs);
        }
        public static void EventQueueEmpty(string file, bool disableLogs = false)
        {
            Log.Debug($"({file}): Event Batching queue is empty", disableLogs);
        }
        public static void BeforeFlushing(string file, string manually, string length, string accountId, string timer, string queue_metadata, bool disableLogs = false)
        {
            Log.Debug($"({file}): Flushing events queue {manually} having {length} events for account:{accountId}. {timer}, queue summary: {queue_metadata}", disableLogs);
        }
        public static void AfterFlushing(string file, string manually, string length, string queue_metadata, bool disableLogs = false)
        {
            Log.Debug($"({file}): Events queue having {length} events has been flushed {manually}, queue summary: {queue_metadata}", disableLogs);
        }
        public static void BatchEventLimitExceeded(string file, string endPoint, string accountId, string eventsPerRequest, bool disableLogs = false)
        {
            Log.Debug($"({file}): Impression event - {endPoint} failed due to exceeding payload size. Parameter eventsPerRequest in batchEvents config in launch API has value:{eventsPerRequest} for accountId:{accountId}. Please read the official documentation for knowing the size limits.", disableLogs);
        }
        public static void TrackApiGoalFound(string file, string goalIdentifier, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): Goal:{goalIdentifier} found for campaign:{campaignKey} and userId:{userId}", disableLogs);
        }

        public static void CampaignNotActivated(string file, string api, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): Campaign:{campaignKey} for User ID:{userId} is not yet activated for API:{api}. Use activate API to activate A/B test or isFeatureEnabled API to activate Feature Test.", disableLogs);
        }
        public static void InitIntegrationMapForGoal(string file, string api, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): InitIntegrationMap called for Goal  Campaign:{campaignKey} for User ID:{userId} for API:{api}.", disableLogs);
        }
        public static void InitIntegrationMapForVariation(string file, string api, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): InitIntegrationMap called for Campaign:{campaignKey} for User ID:{userId} for API:{api}.", disableLogs);
        }
        public static void ExecuteIntegrationsCallbackTargettedVariation(string file, string api, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): Execute Integrations Callback for Targetted Variation for Campaign:{campaignKey} for User ID:{userId} for API:{api}.", disableLogs);
        }
        public static void ExecuteIntegrationsCallbackAlreadyTracked(string file, string api, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): Execute Integrations Callback for Already Tracked or Saved Variation for Campaign:{campaignKey} for User ID:{userId} for API:{api}.", disableLogs);
        }
        public static void ExecuteIntegrationsCallbackNewVariation(string file, string api, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): Execute Integrations Callback for Assigned Variation for Campaign:{campaignKey} for User ID:{userId} for API:{api}.", disableLogs);
        }
        public static void DuplicateCall(string file, string function, bool disableLogs = false)
        {
            Log.Debug($"({file}): Duplicate Call for Variation for {function}", disableLogs);
        }
        public static void VariationNotFound(string file, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Debug($"({file}): Variation not found for campaign:{campaignKey} and userId:{userId}", disableLogs);
        }
        public static void GotEligibleCampaigns(string file, string groupName, string userId, string eligibleText, string inEligibleText, bool disableLogs = false)
        {
            Log.Debug($"({file}): Got eligible campaigns for Group Name:{groupName} and userId:{userId} .Eligible campaigns:{eligibleText}.Ineligible campaigns:{inEligibleText}.", disableLogs);
        }
    }
}
