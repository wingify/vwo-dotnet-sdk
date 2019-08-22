using System;
using System.Collections.Generic;
using System.Linq;

namespace VWOSdk
{
    internal interface IValidator
    {
        bool GetSettings(long accountId, string sdkKey);
        bool Activate(string campaignTestKey, string userId);
        bool GetVariation(string campaignTestKey, string userId);
        bool Track(string campaignTestKey, string userId, string goalIdentifier, string revenueValue);
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

        public bool Activate(string campaignTestKey, string userId)
        {
            var campaignTestKeyResult = ValidateWithLog(() => ValidateString(campaignTestKey), nameof(campaignTestKey), nameof(Activate));
            return ValidateWithLog(() => ValidateString(userId), nameof(userId), nameof(Activate)) && campaignTestKeyResult;
        }

        public bool GetVariation(string campaignTestKey, string userId)
        {
            var campaignTestKeyResult = ValidateWithLog(() => ValidateString(campaignTestKey), nameof(campaignTestKey), nameof(GetVariation));
            return ValidateWithLog(() => ValidateString(userId), nameof(userId), nameof(GetVariation)) && campaignTestKeyResult;
        }

        public bool Track(string campaignTestKey, string userId, string goalIdentifier, string revenueValue)
        {
            var result = ValidateWithLog(() => ValidateString(campaignTestKey), nameof(campaignTestKey), nameof(Track));
            result = ValidateWithLog(() => ValidateString(userId), nameof(userId), nameof(Track)) && result;
            result = ValidateWithLog(() => ValidateString(goalIdentifier), nameof(goalIdentifier), nameof(Track)) && result;
            
            return ValidateWithLog(() => ValidateNullableFloat(revenueValue), nameof(revenueValue), nameof(Track)) && result;
        }

        public bool SettingsFile(Settings settingsFile)
        {
            var result = NotNull(settingsFile);
            result = result && ValidateLong(settingsFile.AccountId) && ValidateString(settingsFile.SdkKey);
            result = result && Validate(settingsFile.Campaigns);
            return result;
        }

        private bool Validate(IReadOnlyList<Campaign> campaigns)
        {
            var result = NotNull(campaigns) && NotEmpty(campaigns);

            foreach(var campaign in campaigns)
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

            foreach(var variation in variations)
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
            return result;
        }

        private bool ValidateDouble(double percentTraffic)
        {
            return percentTraffic >= 0;
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