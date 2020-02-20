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
        public static void ImpressionForTrackGoal(string file, string properties)
        {
            Log.Debug($"({file}): impression built for track-goal - {properties}");
        }
        public static void ImpressionForPushTag(string file, string properties) {
            Log.Debug($"({file}): impression built for push-tags - {properties}");
        }
    }
}
