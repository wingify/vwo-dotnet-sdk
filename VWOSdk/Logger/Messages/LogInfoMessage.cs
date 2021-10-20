#pragma warning disable 1587
/**
 * Copyright 2019-2021 Wingify Software Pvt. Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License", disableLogs);
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
        public static void VariationRangeAllocation(string file, string campaignKey, string variationName, double variationWeight, double start, double end, bool disableLogs = false)
        {
            Log.Info($"({file}): Campaign:{campaignKey} having variations:{variationName} with weight:{variationWeight} got range as: ( {start} - {end} ))", disableLogs);
        }
        public static void VariationAllocated(string file, string userId, string campaignKey, string variationName, bool disableLogs = false)
        {
            Log.Info($"({file}): UserId:{userId} of Campaign:{campaignKey} got variation: {variationName}", disableLogs);
        }
        public static void LookingUpUserStorageService(string file, string userId, string campaignKey, bool disableLogs = false)
        {
            Log.Info($"({file}): Looked into UserStorageService for userId:{userId} and campaign test key: {campaignKey} successful", disableLogs);
        }
        public static void ReturnUserStorageData(string file, string userId, string campaignKey, bool disableLogs = false)
        {
            Log.Info($"({file}): Looked into UserStorageData for userId:{userId} and campaign test key: {campaignKey} successful", disableLogs);
        }

        public static void SavingDataUserStorageService(string file, string userId, bool disableLogs = false)
        {
            Log.Info($"({file}): Saving into UserStorageService for userId:{userId} successful", disableLogs);
        }
        public static void UserHashBucketValue(string file, string bucketingSeed, string userId, double hashValue, double bucketValue, bool disableLogs = false)
        {
            Log.Debug($"({file}): bucketing seed:{bucketingSeed} userId:{userId} having hash:{hashValue} got bucketValue:{bucketValue}", disableLogs);
        }
        public static void GotStoredVariation(string file, string variationName, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Info($"({file}): Got stored variation:{variationName} of campaign:{campaignKey} for userId:{userId} from UserStorageService", disableLogs);
        }
        public static void NoVariationAllocated(string file, string userId, string campaignKey, bool disableLogs = false)
        {
            Log.Info($"({file}): UserId:{userId} of Campaign:{campaignKey} did not get any variation", disableLogs);
        }
        public static void UserEligibilityForCampaign(string file, string userId, bool isUserPart, bool disableLogs = false)
        {
            Log.Info($"({file}): Is userId:{userId} part of campaign? {isUserPart}", disableLogs);
        }
        public static void AudienceConditionNotMet(string file, string userId, bool disableLogs = false)
        {
            Log.Info($"({file}): userId:{userId} does not become part of campaign because of not meeting audience conditions", disableLogs);
        }
        public static void ImpressionSuccess(string file, string endPoint, bool disableLogs = false)
        {
            Log.Info($"({file}): Impression event - {endPoint} was successfully received by VWO.", disableLogs);
        }

        public static void RetryFailedImpressionAfterDelay(string file, string endPoint, string retryTimeout, bool disableLogs = false)
        {
            Log.Info($"({file}): Failed impression event for {endPoint} will be retried after {retryTimeout} milliseconds delay", disableLogs);
        }

        public static void NoCustomVariables(string file, string userId, string campaignKey, string apiName, bool disableLogs = false)
        {
            Log.Info($"({file}): In API: {apiName}, for UserId:{userId} preSegments/customVariables are not passed for campaign:{campaignKey} and campaign has pre-segmentation", disableLogs);
        }

        public static void SkippingPreSegmentation(string file, string userId, string campaignKey, string apiName, bool disableLogs = false)
        {
            Log.Info($"({file}): In API: {apiName}, Skipping pre-segmentation for UserId:{userId} as no valid segments found in campaing:{campaignKey}", disableLogs);
        }

        public static void SkippingWhitelisting(string file, string userId, string campaignKey, string apiName, bool disableLogs = false)
        {
            Log.Info($"({file}): In API: {apiName}, Skipping Whitelisting for UserId:{userId} in campaing:{campaignKey}", disableLogs);
        }

        public static void WhitelistingStatus(string file, string userId, string campaignKey, string apiName, string variationString, string status, bool disableLogs = false)
        {
            Log.Info($"({file}): In API: {apiName}, Whitelisting for UserId:{userId} in campaing:{campaignKey} is: {status} {variationString}", disableLogs);
        }

        public static void FeatureEnabledForUser(string file, string campaignKey, string userId, string apiName, bool disableLogs = false)
        {
            Log.Info($"({file}): In API: {apiName} Feature having Campaign:{campaignKey} for user ID:{userId} is enabled", disableLogs);
        }

        public static void FeatureNotEnabledForUser(string file, string campaignKey, string userId, string apiName, bool disableLogs = false)
        {
            Log.Info($"({file}): In API: {apiName} Feature having Campaign:{campaignKey} for user ID:{userId} is not enabled", disableLogs);
        }

        public static void VariableFound(string file, string variableKey, string campaignKey, string campaignType, string variableValue, string userId, string apiName, bool disableLogs = false)
        {
            Log.Info($"({file}): In API: {apiName} Value for variable:{variableKey} of campaign:{campaignKey} and campaign type: {campaignType} is: {variableValue} for user:{userId}", disableLogs);
        }

        public static void UserPassedPreSegmentation(string file, string userId, string campaignKey, Dictionary<string, dynamic> customVariables, bool disableLogs = false)
        {
            Log.Info($"({file}): UserId:{userId} of campaign:{campaignKey} with custom variables{DictionaryHelper.StringifyCustomVariables(customVariables)} passed pre segmentation", disableLogs);
        }

        public static void UserFailedPreSegmentation(string file, string userId, string campaignKey, Dictionary<string, dynamic> customVariables, bool disableLogs = false)
        {
            Log.Info($"({file}): UserId:{userId} of campaign:{campaignKey} with custom variables{DictionaryHelper.StringifyCustomVariables(customVariables)} failed pre segmentation", disableLogs);
        }

        public static void GoalAlreadyTracked(string file, string userId, string campaignKey, string goalIdentifier, bool disableLogs = false)
        {
            Log.Info($"({file}): Goal:{goalIdentifier} of campaign:{campaignKey} for UserId:{userId} has already been tracked earlier. Skipping now.", disableLogs);
        }
        public static void ImpressionSuccessQueue(string file, bool disableLogs = false)
        {
            Log.Info($"({file}): Impression event was successfully pushed in queue", disableLogs);

        }
        public static void UserAlreadyTracked(string file, string userId, string campaignKey, string apiName, bool disableLogs = false)
        {

            Log.Info($"({file}): User ID:{userId} for Campaign:{campaignKey} has already been tracked earlier for {apiName} API. Skipping now.", disableLogs);

        }
        public static void CampaignNotActivated(string file, string reason, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Info($"({file}): Activate the campaign:{campaignKey} for User ID:{userId} to {reason}", disableLogs);
        }
        public static void NoDataUserStorageService(string file, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Info($"({file}): Unable to fetch data from user storage for{campaignKey} for User ID:{userId}.", disableLogs);
        }
        public static void CalledCampaignNotWinner(string file, string campaignKey, string userId, string groupName, bool disableLogs = false)
        {
            Log.Info($"({file}): Campaign:{campaignKey} does not qualify from the mutually exclusive group:{groupName} for User ID:{userId}.", disableLogs);
        }
        public static void GotEligibleCampaigns(string file, string groupName, string userId, string noOfEligibleCampaigns, string noOfGroupCampaigns, bool disableLogs = false)
        {
            Log.Info($"({file}):Got {noOfEligibleCampaigns} eligible winners out of {noOfGroupCampaigns} campaigns from the Group:{groupName} and for User ID:{userId}.", disableLogs);
        }
        public static void OtherCampaignSatisfiesWhitelistingStorage(string file, string groupName, string userId, string campaignKey, string type, bool disableLogs = false)
        {
            Log.Info($"({file}):Campaign:{campaignKey} of Group:{groupName} satisfies {type} for User ID:{userId}", disableLogs);
        }
        public static void GotWinnerCampaign(string file, string campaignKey, string userId, string groupName, bool disableLogs = false)
        {
            Log.Info($"({file}): Campaign:{campaignKey} is selected from the mutually exclusive group:{groupName} for the User ID:{userId}.", disableLogs);
        }

    }

}
