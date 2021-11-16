﻿#pragma warning disable 1587
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VWOSdk
{
    internal interface IValidator
    {
        bool GetSettings(long accountId, string sdkKey);
        bool Activate(string campaignKey, string userId, Dictionary<string, dynamic> options = null);
        bool GetVariation(string campaignKey, string userId, Dictionary<string, dynamic> options = null);
        bool Track(string campaignKey, string userId, string goalIdentifier, string revenueValue, Dictionary<string, dynamic> options = null);
        bool IsFeatureEnabled(string campaignKey, string userId, Dictionary<string, dynamic> options = null);
        bool GetFeatureVariableValue(string campaignKey, string variableKey, string userId, Dictionary<string, dynamic> options = null);
        bool Push(dynamic tagKey, dynamic tagValue, string userId);
        bool Push(Dictionary<string, string> customDimensionMap, string userId);
        bool SettingsFile(Settings settingsFile);
    }

    internal class Validator : IValidator
    {
        private static readonly string file = typeof(Validator).FullName;
        public bool GetSettings(long accountId, string sdkKey)
        {
            var accountIdResult = ValidateWithLog(() => ValidateLong(accountId), nameof(accountId), nameof(GetSettings));
            return ValidateWithLog(() => ValidateString(sdkKey), nameof(sdkKey), nameof(GetSettings)) && accountIdResult;
        }

        public bool Activate(string campaignKey, string userId, Dictionary<string, dynamic> options = null)
        {
            var campaignKeyResult = ValidateWithLog(() => ValidateString(campaignKey), nameof(campaignKey), nameof(Activate));
            var customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            var metaData = options.ContainsKey("metaData") ? options["metaData"] : null;
            return ValidateWithLog(() => ValidateString(userId) && campaignKeyResult && (customVariables == null || customVariables is Dictionary<string, dynamic>)
            && (metaData == null || metaData is Dictionary<string, dynamic>), nameof(userId), nameof(Activate));
        }

        public bool GetVariation(string campaignKey, string userId, Dictionary<string, dynamic> options = null)
        {
            var campaignKeyResult = ValidateWithLog(() => ValidateString(campaignKey), nameof(campaignKey), nameof(GetVariation));
            var customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            var metaData = options.ContainsKey("metaData") ? options["metaData"] : null;
            return ValidateWithLog(() => ValidateString(userId) && campaignKeyResult && (customVariables == null || customVariables is Dictionary<string, dynamic>)
             && (metaData == null || metaData is Dictionary<string, dynamic>), nameof(userId), nameof(GetVariation));
        }

        public bool Track(string campaignKey, string userId, string goalIdentifier, string revenueValue, Dictionary<string, dynamic> options = null)
        {
            var result = ValidateWithLog(() => ValidateString(campaignKey), nameof(campaignKey), nameof(Track));
            result = ValidateWithLog(() => ValidateString(userId), nameof(userId), nameof(Track)) && result;
            result = ValidateWithLog(() => ValidateString(goalIdentifier), nameof(goalIdentifier), nameof(Track)) && result;
            var customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            var metaData = options.ContainsKey("metaData") ? options["metaData"] : null;
            string goalTypeToTrack = options.ContainsKey("goalTypeToTrack") ? options["goalTypeToTrack"] : null;
            result = ValidateWithLog(() => goalTypeToTrack == null || Constants.GoalTypes.VALUES.Contains(goalTypeToTrack), nameof(goalIdentifier), nameof(Track)) && result;
            return ValidateWithLog(() => ValidateNullableFloat(revenueValue) && (customVariables == null || customVariables is Dictionary<string, dynamic>)
             && (metaData == null || metaData is Dictionary<string, dynamic>), nameof(revenueValue), nameof(Track)) && result;
        }

        public bool IsFeatureEnabled(string campaignKey, string userId, Dictionary<string, dynamic> options = null)
        {
            var campaignKeyResult = ValidateWithLog(() => ValidateString(campaignKey), nameof(campaignKey), nameof(GetVariation));
            var customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            var metaData = options.ContainsKey("metaData") ? options["metaData"] : null;
            return ValidateWithLog(() => ValidateString(userId) && campaignKeyResult && (customVariables == null || customVariables is Dictionary<string, dynamic>)
            && (metaData == null || metaData is Dictionary<string, dynamic>), nameof(userId), nameof(GetVariation));
        }

        public bool GetFeatureVariableValue(string campaignKey, string variableKey, string userId, Dictionary<string, dynamic> options = null)
        {
            var campaignKeyResult = ValidateWithLog(() => ValidateString(campaignKey), nameof(campaignKey), nameof(GetVariation));
            var customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            var metaData = options.ContainsKey("metaData") ? options["metaData"] : null;
            return ValidateWithLog(() => ValidateString(userId) && campaignKeyResult && (customVariables == null || customVariables is Dictionary<string, dynamic>)
            && (metaData == null || metaData is Dictionary<string, dynamic>), nameof(userId), nameof(GetVariation));
        }

        public bool Push(dynamic tagKey, dynamic tagValue, string userId)
        {
            return ValidateWithLog(() => ValidateString(tagKey) && ValidateString(tagValue) && ValidateString(userId), nameof(tagKey), nameof(Push));
        }
        public bool Push(Dictionary<string, string> customDimensionMap, string userId)
        {
            return ValidateWithLog(() => customDimensionMap is Dictionary<string, string> && customDimensionMap.Count > 0 && ValidateString(userId), nameof(customDimensionMap), nameof(Push));
        }
        public bool SettingsFile(Settings settingsFile)
        {
            var result = NotNull(settingsFile);
            result = result && ValidateLong(settingsFile.AccountId);
            result = result && Validate(settingsFile.Campaigns);
            return result;
        }

        private bool Validate(IReadOnlyList<Campaign> campaigns)
        {
            var result = NotNull(campaigns);
            foreach (var campaign in campaigns)
            {
                result = result && Validate(campaign);
            }
            return result;
        }

        private bool Validate(Campaign campaign)
        {
            var result = NotNull(campaign);
            result = result && ValidateLong(campaign.Id);
            result = result && ValidateString(campaign.Key);
            result = result && ValidateDouble(campaign.PercentTraffic);
            result = result && ValidateString(campaign.Type);
            result = result && Validate(campaign.Variations);
            return result;
        }

        private bool Validate(IReadOnlyList<Variation> variations)
        {
            var result = NotNull(variations) && NotEmpty(variations);
            foreach (var variation in variations)
            {
                result = result && Validate(variation);
            }
            return result;
        }

        private bool Validate(Variation variation)
        {
            var result = NotNull(variation);
            result = result && ValidateLong(variation.Id);
            result = result && ValidateString(variation.Name);
            result = result && ValidateDouble(variation.Weight);
            result = result && ValidateJson(variation.Variables);
            return result;
        }

        private bool ValidateDouble(double percentTraffic)
        {
            return percentTraffic >= 0;
        }
        private bool IsValidJson(string input)
        {
            input = input.Trim();

            try
            {
                var jObject = JObject.Parse(input);
                foreach (var jo in jObject)
                {
                    string name = jo.Key;
                    JToken value = jo.Value;
                    if (value.Type == JTokenType.Undefined)
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }


            return true;
        }
        private bool ValidateJson(List<Dictionary<string, dynamic>> Variables)
        {
            if (Variables == null)
            {
                return true;
            }
            foreach (var item in Variables)
            {
                if (item.TryGetValue("type", out dynamic type))
                {
                    if (type == "json")
                    {
                        item.TryGetValue("value", out dynamic jsonText);
                        var jsonFeatureValue = JsonConvert.SerializeObject(jsonText);
                        if (!IsValidJson(jsonFeatureValue))
                        {
                            LogErrorMessage.UnableToParseJson(file, jsonText.ToString(), type);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        private bool NotEmpty<T>(IEnumerable<T> iList)
        {
            return iList.Count() > 0;
        }
        private bool NotNull(object obj)
        {
            return obj != null;
        }
        private static bool ValidateNullableFloat(string value)
        {
            if (value == null)
                return true;

            return float.TryParse(value, out float floatVal);
        }
        private static bool ValidateString(string str)
        {
            return string.IsNullOrEmpty(str) == false;
        }
        private static bool ValidateLong(long value)
        {
            return value > 0;
        }
        private static bool ValidateWithLog(Func<bool> validationFunc, string parameterName, string apiName)
        {
            bool validationResult = validationFunc.Invoke();
            if (validationResult == false)
            {
                LogErrorMessage.ApiMissingParams(file, apiName, parameterName);
            }
            return validationResult;
        }
    }
}
