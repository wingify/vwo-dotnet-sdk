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

using System.Collections.Generic;

namespace VWOSdk
{
    //Common ErrorMessages among different SDKs.
    internal static class LogInfoMessage
    {
        public static void VariationRangeAllocation(string file, string campaignKey, string variationName, double variationWeight, double start, double end)
        {
            Log.Info($"({file}): Campaign:{campaignKey} having variations:{variationName} with weight:{variationWeight} got range as: ( {start} - {end} ))");
        }
        public static void VariationAllocated(string file, string userId, string campaignKey, string variationName)
        {
            Log.Info($"({file}): UserId:{userId} of Campaign:{campaignKey} got variation: {variationName}");
        }
        public static void LookingUpUserStorageService(string file, string userId, string campaignKey)
        {
            Log.Info($"({file}): Looked into UserStorageService for userId:{userId} and campaign test key: {campaignKey} successful");
        }
        public static void SavingDataUserStorageService(string file, string userId)
        {
            Log.Info($"({file}): Saving into UserStorageService for userId:{userId} successful");
        }
        public static void GotStoredVariation(string file, string variationName, string campaignKey, string userId)
        {
            Log.Info($"({file}): Got stored variation:{variationName} of campaign:{campaignKey} for userId:{userId} from UserStorageService");
        }
        public static void NoVariationAllocated(string file, string userId, string campaignKey)
        {
            Log.Info($"({file}): UserId:{userId} of Campaign:{campaignKey} did not get any variation");
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

        public static void NoCustomVariables(string file , string userId, string campaignKey, string apiName) {
            Log.Info($"({file}): In API: {apiName}, for UserId:{userId} preSegments/customVariables are not passed for campaign:{campaignKey} and campaign has pre-segmentation");
        }

        public static void SkippingPreSegmentation(string file , string userId, string campaignKey, string apiName) {
            Log.Info($"({file}): In API: {apiName}, Skipping pre-segmentation for UserId:{userId} as no valid segments found in campaing:{campaignKey}");
        }

        public static void FeatureEnabledForUser(string file, string campaignKey, string userId, string apiName) 
        {
            Log.Info($"({file}): In API: {apiName} Feature having Campaign:{campaignKey} for user ID:{userId} is enabled");
        }

        public static void FeatureNotEnabledForUser(string file, string campaignKey, string userId, string apiName)
        {
            Log.Info($"({file}): In API: {apiName} Feature having Campaign:{campaignKey} for user ID:{userId} is not enabled");
        }

        public static void VariableFound(string file, string variableKey, string campaignKey, string campaignType, string variableValue, string userId, string apiName)
        {
            Log.Info($"({file}): In API: {apiName} Value for variable:{variableKey} of campaign:{campaignKey} and campaign type: {campaignType} is: {variableValue} for user:{userId}");
        }

        public static void UserPassedPreSegmentation(string file, string userId, string campaignKey, Dictionary<string, dynamic> customVariables) {
            Log.Info($"({file}): UserId:{userId} of campaign:{campaignKey} with custom variables{DictionaryHelper.StringifyCustomVariables(customVariables)} passed pre segmentation");
        }

        public static void UserFailedPreSegmentation(string file, string userId, string campaignKey, Dictionary<string, dynamic> customVariables) {
            Log.Info($"({file}): UserId:{userId} of campaign:{campaignKey} with custom variables{DictionaryHelper.StringifyCustomVariables(customVariables)} failed pre segmentation");
        }
    }
}
