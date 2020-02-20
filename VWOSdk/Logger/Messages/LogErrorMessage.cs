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
        //    Log.Error($"({file}): \"getVariation\" API got bad parameters. It expects campaignKey(String) as first and userId(String) as second argument");
        //}
        //public static void GET_VARIATION_API_CONFIG_CORRUPTED(string file)
        //{
        //    Log.Error($"({file}): \"getVariation\" API has corrupted configuration");
        //}
        //public static void TRACK_API_MISSING_PARAMS(string file)
        //{
        //    Log.Error($"({file}): \"track\" API got bad parameters. It expects campaignKey(String) as first, userId(String) as second and goalIdentifier(String/Number) as third argument. Fourth is revenueValue(Float/Number/String) and is required for revenue goal only.");
        //}
        //public static void TRACK_API_CONFIG_CORRUPTED(string file)
        //{
        //    Log.Error($"({file}): \"track\" API has corrupted configuration");
        //}

        public static void InvalidApi(string file, string userId, string campaignKey, string campaignType, string apiName) {
            Log.Error($"({file}): {apiName} API is not valid for user ID: {userId} in campaign ID: {campaignKey} having campaign type: {campaignType}.");
        }
        public static void VariableNotFound(string file,string variableKey, string campaignKey, string campaignType, string userId, string apiName) {
            Log.Error($"({file}): In API: {apiName} Variable: {variableKey} not found for campaign: {campaignKey} and type: {campaignType} for user ID: {userId}.");
        }
        
        public static void TrackApiVariationNotFound(string file, string campaignKey, string userId)
        {
            Log.Error($"({file}): Variation not found for campaign:{campaignKey} and userId:{userId}");
        }
        public static void CampaignNotRunning(string file, string campaignKey, string apiName = null)
        {
            apiName = apiName ?? string.Empty;
            Log.Error($"({file}): API used:{apiName} - Campaign:{campaignKey} is not RUNNING. Please verify from VWO App");
        }
        public static void GetUserStorageServiceFailed(string file, string userId, string campaignKey)
        {
            Log.Error($"({file}): Get method could not provide us the stored variation for User Id: {userId} and Campaign test key: {campaignKey}. Please check your User Storage Service Get implementation.");
            //Log.Error($"({file}): Looking data from UserStorageService failed for userId:{userId}");
        }
        public static void SetUserStorageServiceFailed(string file, string userId)
        {
            Log.Error($"({file}): Saving data into UserStorageService failed for userId:{userId}");
        }
        public static void UnableToTypeCast(string file, string value, string variableType, string ofType)
        {
            Log.Error($"({file}): Unable to typecast value: {value} of type: {ofType} to type: {variableType}.");
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

        internal static void TrackApiRevenueNotPassedForRevenueGoal(string file, string goalIdentifier, string campaignKey, string userId)
        {
            Log.Error($"({file}): Revenue value should be passed for revenue goal:{goalIdentifier} for campaign:{campaignKey} and userId:{userId}");
        }

        internal static void TrackApiGoalNotFound(string file, string goalIdentifier, string campaignKey, string userId)
        {
            Log.Error($"({file}): Goal:{goalIdentifier} not found for campaign:{campaignKey} and userId:{userId}");
        }

        public static void CustomLoggerMisconfigured(string file)
        {
            Log.Error($"({file}): custom logger is provided but seems to have misconfigured. please check the api docs. using default logger.");
        }

        public static void TagKeyLengthExceeded(string file, string tagKey, string userId,  string apiName) {
            Log.Error($"({file}): In API: {apiName}, the length of tagKey:{tagKey} and userID: {userId} can not be greater than 255");
        }

        public static void TagValueLengthExceeded(string file, string tagValue, string userId, string apiName) {
            Log.Error($"({file}): In API: {apiName}, the length of tagValue:{tagValue} and userID: {userId} can not be greater than 255");
        }

    }
}
