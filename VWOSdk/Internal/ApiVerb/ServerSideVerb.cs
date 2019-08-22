using System;
using System.Reflection;

namespace VWOSdk
{
    internal class ServerSideVerb
    {
        private static readonly string Host = Constants.Endpoints.BASE_URL;
        private static readonly string Verb = Constants.Endpoints.SERVER_SIDE;
        private static readonly string SettingsVerb = Constants.Endpoints.ACCOUNT_SETTINGS;
        private static readonly string TrackUserVerb = Constants.Endpoints.TRACK_USER;
        private static readonly string TrackGoalVerb = Constants.Endpoints.TRACK_GOAL;
        private static readonly string file = typeof(ServerSideVerb).FullName;
        private static readonly string sdkVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        internal static ApiRequest SettingsRequest(long accountId, string sdkKey)
        {
            var settingsRequest = new ApiRequest(Method.GET)
            {
                Uri = new Uri($"{Host}/{Verb}/{SettingsVerb}?{GetQueryParamertersForSetting(accountId, sdkKey)}"),
            };
            settingsRequest.WithCaller(AppContext.ApiCaller);

            return settingsRequest;
        }

        internal static ApiRequest TrackUser(long accountId, int campaignId, int variationId, string userId, bool isDevelopmentMode)
        {
            string queryParams = GetQueryParamertersForTrackUser(accountId, campaignId, variationId, userId);
            var trackUserRequest = new ApiRequest(Method.GET, isDevelopmentMode)
            {
                Uri = new Uri($"{Host}/{Verb}/{TrackUserVerb}?{queryParams}"),
            };
            trackUserRequest.WithCaller(AppContext.ApiCaller);
            LogDebugMessage.ImpressionForTrackUser(file, queryParams);
            return trackUserRequest;
        }

        internal static ApiRequest TrackGoal(int accountId, int campaignId, int variationId, string userId, int goalId, string revenueValue, bool isDevelopmentMode)
        {
            string queryParams = GetQueryParamertersForTrackGoal(accountId, campaignId, variationId, userId, goalId, revenueValue);
            var trackUserRequest = new ApiRequest(Method.GET, isDevelopmentMode)
            {
                Uri = new Uri($"{Host}/{Verb}/{TrackGoalVerb}?{queryParams}"),
            };
            trackUserRequest.WithCaller(AppContext.ApiCaller);
            LogDebugMessage.ImpressionForTrackGoal(file, queryParams);
            return trackUserRequest;
        }

        private static string GetQueryParamertersForTrackGoal(int accountId, int campaignId, int variationId, string userId, int goalId, string revenueValue = null)
        {
            return $"{GetAccountIdQuery(accountId)}" +
                $"&{GetExperimentIdQuery(campaignId)}" +
                $"&{GetPlatformQuery()}" +
                $"&{GetCombination(variationId)}" +
                $"&{GetRandomQuery()}" +
                $"&{GetUnixTimeStamp()}" +
                $"&{GetUuidQuery(userId, accountId)}" +
                $"&{GetUserIdQuery(userId)}" +
                $"&{GetGoalIdQuery(goalId)}" +
                $"&{GetRevenueQuery(revenueValue)}" +
                $"&{GetSdkQuery()}";
        }

        private static string GetRevenueQuery(string revenueValue)
        {
            if (string.IsNullOrEmpty(revenueValue))
                return string.Empty;

            return $"r={revenueValue}";
        }

        private static string GetGoalIdQuery(int goalId)
        {
            return $"goal_id={goalId}";
        }

        private static string GetQueryParamertersForSetting(long accountId, string sdkKey)
        {
            return $"a={accountId}&i={sdkKey}&r={GetRandomNumber()}&{GetSdkQuery()}";
        }

        private static string GetSdkQuery()
        {
            return $"sdk=netstandard2.0&sdk-v={sdkVersion}";
        }

        private static string GetQueryParamertersForTrackUser(long accountId, int campaignId, int variationId, string userId)
        {
            return $"{GetAccountIdQuery(accountId)}" +
                $"&{GetExperimentIdQuery(campaignId)}" +
                $"&{GetPlatformQuery()}" +
                $"&{GetCombination(variationId)}" +
                $"&{GetRandomQuery()}" +
                $"&{GetUnixTimeStamp()}" +
                $"&{GetUuidQuery(userId, accountId)}" +
                $"&{GetUserIdQuery(userId)}" +
                $"&{GetEdQuery()}" +
                $"&{GetSdkQuery()}";
        }

        private static string GetEdQuery()
        {
            return "ed={\"p\":\"server\"}";
        }

        private static string GetUuidQuery(string userId, long accountId)
        {
            return $"u={UuidV5Helper.Compute(accountId, userId)}";
        }

        private static string GetUnixTimeStamp()
        {
            return $"sId={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        }

        private static string GetCombination(int variationId)
        {
            return $"combination={variationId}";
        }

        private static string GetUserIdQuery(string userId)
        {
            return $"uId={Uri.EscapeUriString(userId)}";
        }

        private static string GetPlatformQuery()
        {
            return $"ap={Constants.PLATFORM}";
        }

        private static string GetExperimentIdQuery(int campaignId)
        {
            return $"experiment_id={campaignId}";
        }

        private static string GetAccountIdQuery(long accountId)
        {
            return $"account_id={accountId}";
        }

        private static string GetRandomQuery()
        {
            return $"random={GetRandomNumber()}";
        }

        private static double GetRandomNumber()
        {
            Random random = new Random();
            return random.NextDouble();
        }

    }
}