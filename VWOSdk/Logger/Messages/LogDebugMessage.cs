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
        public static void NoStoredVariation(string file, string userId, string campaignKey)
        {
            Log.Debug($"({file}): No stored variation for UserId:{userId} for Campaign:{campaignKey} found in UserStorageService");
        }
        public static void NoUserStorageServiceGet(string file)
        {
            Log.Debug($"({file}): No UserStorageService to look for stored data");
        }
        public static void NoUserStorageServiceSet(string file)
        {
            Log.Debug($"({file}): No UserStorageService to set data");
        }

        public static void CheckUserEligibilityForCampaign(string file, string campaignKey, double trafficAllocation, string userId)
        {
            Log.Debug($"({file}): campaign:{campaignKey} having traffic allocation:{trafficAllocation} assigned value:{trafficAllocation} to userId:{userId}");
        }
        public static void UserHashBucketValue(string file, string userId, double hashValue, double bucketValue)
        {
            Log.Debug($"({file}): userId:{userId} having hash:{hashValue} got bucketValue:{bucketValue}");
        }
        public static void VariationHashBucketValue(string file, string userId, string campaignKey, double percentTraffic, double hashValue, double bucketValue)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignKey} having percent traffic:{percentTraffic} got hash-value:{hashValue} and bucket value:{bucketValue}");
        }
        public static void GotVariationForUser(string file, string userId, string campaignKey, string variationName, string method)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignKey} got variationName:{variationName} inside method:{method}");
        }
        public static void UserNotPartOfCampaign(string file, string userId, string campaignKey, string method)
        {
            Log.Debug($"({file}): userId:{userId} for campaign:{campaignKey} did not become part of campaign, method:{method}");
        }
        public static void UuidForUser(string file, string userId, long accountId, string desiredUuid)
        {
            Log.Debug($"({file}): Uuid generated for userId:{userId} and accountId:{accountId} is {desiredUuid}");
        }
        public static void ImpressionForTrackUser(string file, string properties)
        {
            Log.Debug($"({file}): impression built for track-user - {properties}");
        }
        public static void ImpressionForBatchEvent(string file, string properties)
        {
            Log.Debug($"({file}): impression built for track-user - {properties}");
        }
        public static void ImpressionForTrackGoal(string file, string properties)
        {
            Log.Debug($"({file}): impression built for track-goal - {properties}");
        }
        public static void ImpressionForPushTag(string file, string properties)
        {
            Log.Debug($"({file}): impression built for push-tags - {properties}");
        }
        public static void SkippingSegmentation(string file, string userId, string campaignKey, string apiName, string variationName)
        {
            Log.Debug($"({file}): In API: {apiName}, Skipping segmentation for UserId:{userId} in campaing:{campaignKey} for variation: {variationName} as no valid segment is found");
        }
        public static void SegmentationStatus(string file, string userId, string campaignKey, string apiName, string variationName, string status)
        {
            Log.Debug($"({file}): In API: {apiName}, Whitelisting for UserId:{userId} in campaing:{campaignKey} for variation: {variationName} is: {status}");
        }


        //Batch Event
        public static void RequestTimeIntervalOutOfBound(string file, int min_value, int default_value)
        {
            Log.Debug($"({file}): requestTimeInterval should be > {min_value.ToString()}. Assigning it the default value i.e {default_value.ToString()} seconds");
        }
        public static void EventsPerRequestOutOfBound(string file, int min_value, int max_value, int default_value)
        {
            Log.Debug($"({file}): eventsPerRequest should be > {min_value.ToString()} and <= {max_value.ToString()}. Assigning it the default value i.e {default_value.ToString()}");
        }

        public static void EventBatchingNotActivated(string file,string function)
        {
            Log.Debug($"({file}): Event batching is not activated for {function}  or send null");
        }
        public static void EventBatchingActivated(string file, string function)
        {
            Log.Debug($"({file}): Event added in queue for {function}");
        }
        public static void EventQueueEmpty(string file)
        {
            Log.Debug($"({file}): Event Batching queue is empty");
        }
        public static void BeforeFlushing(string file, string manually, string length, string accountId, string timer, string queue_metadata)
        {
            Log.Debug($"({file}): Flushing events queue {manually} having {length} events for account:{accountId}. {timer}, queue summary: {queue_metadata}");
        }
        public static void AfterFlushing(string file, string manually, string length, string queue_metadata)
        {
            Log.Debug($"({file}): Events queue having {length} events has been flushed {manually}, queue summary: {queue_metadata}");
        }
        public static void BatchEventLimitExceeded (string file, string endPoint, string accountId, string eventsPerRequest)
        {
            Log.Debug($"({file}): Impression event - {endPoint} failed due to exceeding payload size. Parameter eventsPerRequest in batchEvents config in launch API has value:{eventsPerRequest} for accountId:{accountId}. Please read the official documentation for knowing the size limits.");
        }
        public static void TrackApiGoalFound(string file, string goalIdentifier, string campaignKey, string userId)
        {
            Log.Debug($"({file}): Goal:{goalIdentifier} found for campaign:{campaignKey} and userId:{userId}");
        }

        public static void CampaignNotActivated(string file, string api, string campaignKey, string userId)
        {
            Log.Debug($"({file}): Campaign:{campaignKey} for User ID:{userId} is not yet activated for API:{api}. Use activate API to activate A/B test or isFeatureEnabled API to activate Feature Test.");
        }
        public static void InitIntegrationMapForGoal(string file, string api, string campaignKey, string userId)
        {
            Log.Debug($"({file}): InitIntegrationMap called for Goal  Campaign:{campaignKey} for User ID:{userId} for API:{api}.");
        }
        public static void InitIntegrationMapForVariation(string file, string api, string campaignKey, string userId)
        {
            Log.Debug($"({file}): InitIntegrationMap called for Campaign:{campaignKey} for User ID:{userId} for API:{api}.");
        }
        public static void ExecuteIntegrationsCallbackTargettedVariation(string file, string api, string campaignKey, string userId)
        {
            Log.Debug($"({file}): Execute Integrations Callback for Targetted Variation for Campaign:{campaignKey} for User ID:{userId} for API:{api}.");
        }
        public static void ExecuteIntegrationsCallbackAlreadyTracked(string file, string api, string campaignKey, string userId)
        {
            Log.Debug($"({file}): Execute Integrations Callback for Already Tracked or Saved Variation for Campaign:{campaignKey} for User ID:{userId} for API:{api}.");
        }
        public static void ExecuteIntegrationsCallbackNewVariation(string file, string api, string campaignKey, string userId)
        {
            Log.Debug($"({file}): Execute Integrations Callback for Assigned Variation for Campaign:{campaignKey} for User ID:{userId} for API:{api}.");
        }
        public static void DuplicateCall(string file, string function)
        {
            Log.Debug($"({file}): Duplicate Call for Variation for {function}");
        }
        public static void VariationNotFound(string file, string campaignKey, string userId)
        {
            Log.Debug($"({file}): Variation not found for campaign:{campaignKey} and userId:{userId}");
        }

    }
}
