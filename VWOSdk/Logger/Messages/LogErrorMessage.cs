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

namespace VWOSdk
{
    //Common ErrorMessages among different SDKs.
    internal static class LogErrorMessage
    {
        public static void ProjectConfigCorrupted(string file, bool disableLogs = false)
        {
            Log.Error($"({file}): config passed to Launch is not a valid object.", disableLogs);
        }
        //public static void INVALID_CONFIGURATION(string file)
        //{
        //    Log.Error($"({file}): SDK configuration or account settings or both is/are not valid.", disableLogs);
        //}
        public static void SettingsFileCorrupted(string file, bool disableLogs = false)
        {
            Log.Error($"({file}): Settings file is corrupted. Please contact VWO Support for help.", disableLogs);
        }
        public static void ApiMissingParams(string file, string apiName, string paramName, bool disableLogs = false)
        {
            Log.Error($"({file}): \"{apiName}\" API got bad parameters. Invalid parameter: {paramName}.", disableLogs);
        }
        //public static void ACTIVATE_API_CONFIG_CORRUPTED(string file)
        //{
        //    Log.Error($"({file}): \"activate\" API has corrupted configuration", disableLogs);
        //}
        //public static void GET_VARIATION_API_MISSING_PARAMS(string file)
        //{
        //    Log.Error($"({file}): \"getVariation\" API got bad parameters. It expects campaignKey(String) as first and userId(String) as second argument", disableLogs);
        //}
        //public static void GET_VARIATION_API_CONFIG_CORRUPTED(string file)
        //{
        //    Log.Error($"({file}): \"getVariation\" API has corrupted configuration", disableLogs);
        //}
        //public static void TRACK_API_MISSING_PARAMS(string file)
        //{
        //    Log.Error($"({file}): \"track\" API got bad parameters. It expects campaignKey(String) as first, userId(String) as second and goalIdentifier(String/Number) as third argument. Fourth is revenueValue(Float/Number/String) and is required for revenue goal only.", disableLogs);
        //}
        //public static void TRACK_API_CONFIG_CORRUPTED(string file)
        //{
        //    Log.Error($"({file}): \"track\" API has corrupted configuration", disableLogs);
        //}

        public static void InvalidApi(string file, string userId, string campaignKey, string campaignType, string apiName, bool disableLogs = false)
        {
            Log.Error($"({file}): {apiName} API is not valid for user ID: {userId} in campaign ID: {campaignKey} having campaign type: {campaignType}.", disableLogs);
        }
        public static void VariableNotFound(string file, string variableKey, string campaignKey, string campaignType, string userId, string apiName, bool disableLogs = false)
        {
            Log.Error($"({file}): In API: {apiName} Variable: {variableKey} not found for campaign: {campaignKey} and type: {campaignType} for user ID: {userId}.", disableLogs);
        }

        public static void TrackApiVariationNotFound(string file, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Error($"({file}): Variation not found for campaign:{campaignKey} and userId:{userId}", disableLogs);
        }
        public static void CampaignNotRunning(string file, string campaignKey, string apiName = null, bool disableLogs = false)
        {
            apiName = apiName ?? string.Empty;
            Log.Error($"({file}): API used:{apiName} - Campaign:{campaignKey} is not RUNNING. Please verify from VWO App", disableLogs);
        }
        public static void GetUserStorageServiceFailed(string file, string userId, string campaignKey, bool disableLogs = false)
        {
            Log.Error($"({file}): Get method could not provide us the stored variation for User Id: {userId} and Campaign test key: {campaignKey}. Please check your User Storage Service Get implementation.", disableLogs);
            //Log.Error($"({file}): Looking data from UserStorageService failed for userId:{userId}", disableLogs);
        }
        public static void SetUserStorageServiceFailed(string file, string userId, bool disableLogs = false)
        {
            Log.Error($"({file}): Saving data into UserStorageService failed for userId:{userId}", disableLogs);
        }
        public static void UnableToTypeCast(string file, string value, string variableType, string ofType, bool disableLogs = false)
        {
            Log.Error($"({file}): Unable to typecast value: {value} of type: {ofType} to type: {variableType}.", disableLogs);
        }
        public static void UnableToParseJson(string file, string value, string variableType, bool disableLogs = false)
        {
            Log.Error($"({file}): Unable to parse json value: {value} of type: {variableType}.", disableLogs);
        }
        public static void UnableToParseJson(string file, string value, string variableType)
        {
            Log.Error($"({file}): Unable to parse json value: {value} of type: {variableType}.");
        }
        //public static void InvalidCampaign(string file, string method)
        //{
        //    Log.Error($"({file}): Invalid campaign passed to {method} of this file", disableLogs);
        //}
        //public static void INVALID_USER_ID(string file, string userId, string method)
        //{
        //    Log.Error($"({file}): Invalid userId:{userId} passed to {method} of this file", disableLogs);
        //}
        public static void ImpressionFailed(string file, string endPoint, bool disableLogs = false)
        {
            Log.Error($"({file}): Impression event could not be sent to VWO - {endPoint}", disableLogs);
        }
        public static void BulkNotProcessed(string file, bool disableLogs = false)
        {
            Log.Error($"({file}): Batch events couldn't be received by VWO. Calling Flush Callback with error and data.", disableLogs);
        }

        internal static void TrackApiRevenueNotPassedForRevenueGoal(string file, string goalIdentifier, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Error($"({file}): Revenue value should be passed for revenue goal:{goalIdentifier} for campaign:{campaignKey} and userId:{userId}", disableLogs);
        }

        internal static void TrackApiGoalNotFound(string file, string goalIdentifier, string campaignKey, string userId, bool disableLogs = false)
        {
            Log.Error($"({file}): Goal:{goalIdentifier} not found for campaign:{campaignKey} and userId:{userId}", disableLogs);
        }

        //internal static void TrackApiGoalFound(string file, string goalIdentifier, string campaignKey, string userId)
        //{
        //    Log.Error($"({file}): Goal:{goalIdentifier} found for campaign:{campaignKey} and userId:{userId}", disableLogs);
        //}

        public static void CustomLoggerMisconfigured(string file, bool disableLogs = false)
        {
            Log.Error($"({file}): custom logger is provided but seems to have misconfigured. please check the api docs. using default logger.", disableLogs);
        }

        public static void TagKeyLengthExceeded(string file, string tagKey, string userId, string apiName, bool disableLogs = false)
        {
            Log.Error($"({file}): In API: {apiName}, the length of tagKey:{tagKey} and userID: {userId} can not be greater than 255", disableLogs);
        }

        public static void TagValueLengthExceeded(string file, string tagValue, string userId, string apiName, bool disableLogs = false)
        {
            Log.Error($"({file}): In API: {apiName}, the length of tagValue:{tagValue} and userID: {userId} can not be greater than 255", disableLogs);
        }

        public static void NoCampaignForGoalFound(string file, string goalIdentifier, bool disableLogs = false)
        {
            Log.Error($"({file}): No campaign found for goalIdentifier:{goalIdentifier}. Please verify from VWO app.", disableLogs);
        }
        public static void UnableToDisplayHttpRequest(string file, string error, bool disableLogs = false)
        {
            Log.Error($"({file}): Exception while executing http request.Error Details: {error}", disableLogs);
        }

    }

}
