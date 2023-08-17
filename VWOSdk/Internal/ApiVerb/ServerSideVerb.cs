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

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VWOSdk
{
    internal class ServerSideVerb
    {
        private static readonly string Host = Constants.Endpoints.BASE_URL;
        private static readonly string Verb = Constants.Endpoints.SERVER_SIDE;
        private static readonly string SettingsVerb = Constants.Endpoints.ACCOUNT_SETTINGS;
        private static readonly string WebhookSettingsVerb = Constants.Endpoints.WEBHOOK_SETTINGS_URL;
        private static readonly string TrackUserVerb = Constants.Endpoints.TRACK_USER;
        private static readonly string TrackGoalVerb = Constants.Endpoints.TRACK_GOAL;
        private static readonly string PushTagsVerb = Constants.Endpoints.PUSH_TAGS;
        private static readonly string EventArchVerb = Constants.Endpoints.EVENT_ARCH;
        private static readonly string BatchEventVerb = Constants.Endpoints.BATCH_EVENTS;
        private static readonly string file = typeof(ServerSideVerb).FullName;
        private static readonly string sdkVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static readonly string sdkName = Constants.SDK_NAME;
        private static readonly string TrackEventName = Constants.TRACK_EVENT_NAME;
        private static readonly string PushEventName = Constants.PUSH_EVENT_NAME;
        private static readonly string UserAgent = Constants.Visitor.USER_AGENT;
        private static readonly string IP = Constants.Visitor.IP;
        private static readonly string CUSTOM_HEADER_USER_AGENT = Constants.Visitor.CUSTOM_HEADER_USER_AGENT;
        private static readonly string CUSTOM_HEADER_IP = Constants.Visitor.CUSTOM_HEADER_IP;
        
        internal static ApiRequest SettingsRequest(long accountId, string sdkKey)
        {
            var settingsRequest = new ApiRequest(Method.GET)
            {
                Uri = new Uri($"{Host}/{Verb}/{SettingsVerb}?{GetQueryParamertersForSetting(accountId, sdkKey)}"),
            };
            settingsRequest.WithCaller(AppContext.ApiCaller);
            return settingsRequest;
        }
        internal static ApiRequest SettingsPullRequest(long accountId, string sdkKey)
        {
            var settingsRequest = new ApiRequest(Method.GET)
            {
                Uri = new Uri($"{Host}/{Verb}/{WebhookSettingsVerb}?{GetQueryParamertersForSetting(accountId, sdkKey)}"),
            };
            settingsRequest.WithCaller(AppContext.ApiCaller);
            return settingsRequest;
        }
        internal static ApiRequest TrackUser(AccountSettings settings, long accountId, int campaignId, int variationId, string userId, bool isDevelopmentMode, string sdkKey, Dictionary<string, int> usageStats, string visitorUserAgent = null, string userIpAddress = null)
        {
            //adding new params required for adding userAgent and userIpAddress to the query
            string queryParams = GetQueryParamertersForTrackUser(accountId, campaignId, variationId, userId, usageStats, visitorUserAgent, userIpAddress);
            var trackUserRequest = new ApiRequest(Method.GET, isDevelopmentMode, visitorUserAgent, userIpAddress)
            {
                Uri = new Uri($"{Host}/{getDataLocation(settings, Verb)}/{TrackUserVerb}?{queryParams}&{GetSdkKeyQuery(sdkKey)}"),
                logUri = new Uri($"{Host}/{getDataLocation(settings, Verb)}/{TrackUserVerb}?{queryParams}"),
            };
            trackUserRequest.WithCaller(AppContext.ApiCaller);
            LogDebugMessage.ImpressionForTrackUser(file, queryParams);
            return trackUserRequest;
        }
        //Event Batching
        internal static ApiRequest EventBatching(AccountSettings settings, long accountId, bool isDevelopmentMode, string sdkKey, Dictionary<string, int> usageStats)
        {
            string queryParams = GetQueryParamertersForEventBatching(accountId, usageStats);
            var trackUserRequest = new ApiRequest(Method.POST, isDevelopmentMode)
            {
                Uri = new Uri($"{Host}/{getDataLocation(settings, Verb)}/{BatchEventVerb}?{queryParams}&{GetSdkKeyQuery(sdkKey)}"),
                logUri = new Uri($"{Host}/{getDataLocation(settings, Verb)}/{BatchEventVerb}?{queryParams}"),
            };

            LogDebugMessage.ImpressionForBatchEvent(file, queryParams);
            return trackUserRequest;
        }
        private static string GetQueryParamertersForEventBatching(long accountId, Dictionary<string, int> usageStats)
        {
            return $"{withMinifiedAccountIdQuery(accountId)}" + $"{GetUsageStatsQuery(usageStats)}" + $"&{GetBatchSdkQuery()}";
        }
        private static string GetUsageStatsQuery(Dictionary<string, int> usageStats)
        {
            string QueryStats = "";
            if (usageStats != null && usageStats.Count != 0)
            {
                var listStats = new List<string>();
                foreach (var item in usageStats)
                {
                    listStats.Add(item.Key + "=" + item.Value);
                }
                listStats.Add("_l=1");
                QueryStats = "&" + string.Join("&", listStats);
            }
            return QueryStats;
        }
        private static string GetBatchSdkQuery()
        {
            return $"sd={sdkName}&sv={sdkVersion}";
        }
        private static string withMinifiedAccountIdQuery(long accountId)
        {
            return $"a={accountId}";
        }
        // End
        internal static ApiRequest TrackGoal(AccountSettings settings, int accountId, int campaignId, int variationId, string userId, int goalId,
            string revenueValue, bool isDevelopmentMode, string sdkKey, string visitorUserAgent = null, string userIpAddress = null)
        {
            string queryParams = GetQueryParamertersForTrackGoal(accountId, campaignId, variationId, userId, goalId, revenueValue, visitorUserAgent, userIpAddress);
            var trackUserRequest = new ApiRequest(Method.GET, isDevelopmentMode, visitorUserAgent, userIpAddress)
            {
                Uri = new Uri($"{Host}/{getDataLocation(settings, Verb)}/{TrackGoalVerb}?{queryParams}&{GetSdkKeyQuery(sdkKey)}"),
                logUri = new Uri($"{Host}/{getDataLocation(settings, Verb)}/{TrackGoalVerb}?{queryParams}"),
            };
            trackUserRequest.WithCaller(AppContext.ApiCaller);
            LogDebugMessage.ImpressionForTrackGoal(file, queryParams);
            return trackUserRequest;
        }
        internal static ApiRequest PushTags(AccountSettings settings, string tagKey, string tagValue, string userId, bool isDevelopmentMode, string sdkKey)
        {
            string queryParams = GetQueryParamertersForPushTag(settings, tagKey, tagValue, userId);
            var trackPushRequest = new ApiRequest(Method.GET, isDevelopmentMode)
            {
                Uri = new Uri($"{Host}/{getDataLocation(settings, Verb)}/{PushTagsVerb}?{queryParams}&{GetSdkKeyQuery(sdkKey)}"),
                logUri = new Uri($"{Host}/{getDataLocation(settings, Verb)}/{PushTagsVerb}?{queryParams}"),
            };
            trackPushRequest.WithCaller(AppContext.ApiCaller);
            LogDebugMessage.ImpressionForPushTag(file, queryParams);
            return trackPushRequest;
        }
        public static string GetQueryParamertersForTrackGoal(int accountId, int campaignId, int variationId, string userId,
            int goalId, string revenueValue, string visitorUserAgent = null, string userIpAddress = null)
        {
            return $"{GetAccountIdQuery(accountId)}" +
                $"&{GetExperimentIdQuery(campaignId)}" +
                $"&{GetPlatformQuery()}" +
                $"&{GetCombination(variationId)}" +
                $"&{GetRandomQuery()}" +
                $"&{GetUnixTimeStamp()}" +
                $"&{GetUuidQuery(userId, accountId)}" +
                $"&{GetGoalIdQuery(goalId)}" +
                $"&{GetRevenueQuery(revenueValue)}" +
                $"&{GetSdkQuery()}" +
                $"&{GetVisitorUserAgent(visitorUserAgent)}" +
                $"&{GetVisitorIP(userIpAddress)}" ;;
        }
        private static string GetQueryParamertersForPushTag(AccountSettings settings, string tagKey, string tagValue, string userId)
        {
            return $"{GetAccountIdQuery(settings.AccountId)}" +
                $"&{GetPlatformQuery()}" +
                $"&{GetRandomQuery()}" +
                $"&{GetUnixTimeStamp()}" +
                $"&{GetUuidQuery(userId, settings.AccountId)}" +
                $"&{GetUserTagQuery(tagKey, tagValue)}" +
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
            return $"sdk={sdkName}&sdk-v={sdkVersion}";
        }
        public static string GetQueryParamertersForTrackUser(long accountId, int campaignId, int variationId, string userId, Dictionary<string, int> usageStats, string visitorUserAgent, string userIpAddress)
        {
            return $"{GetAccountIdQuery(accountId)}" +
                $"&{GetExperimentIdQuery(campaignId)}" +
                $"&{GetPlatformQuery()}" +
                $"&{GetCombination(variationId)}" +
                $"&{GetRandomQuery()}" +
                $"&{GetUnixTimeStamp()}" +
                $"&{GetUuidQuery(userId, accountId)}" +
                $"&{GetEdQuery()}" +
                $"{GetUsageStatsQuery(usageStats)}" +
                $"&{GetSdkQuery()}" +
                $"&{GetVisitorUserAgent(visitorUserAgent)}" +
                $"&{GetVisitorIP(userIpAddress)}" ;
        }
        private static string GetEdQuery()
        {
            return "ed={\"p\":\"server\"}";
        }
        private static string GetSdkKeyQuery(string sdkKey)
        {
            return $"env={sdkKey}";
        }
        private static string GetUserTagQuery(string tagKey, string tagValue)
        {
            return $"tags={{\"u\":{{\"{tagKey}\":\"{tagValue}\"}}}}";
        }
        private static string GetUuidQuery(string userId, long accountId)
        {
            return $"u={UuidV5Helper.Compute(accountId, userId)}";
        }
        private static string GetUnixTimeStamp()
        {
            return $"sId={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        }
        private static string GetUnixTimeStampArchEnabled()
        {
            return $"eTime={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        }
        private static string GetUnixMsTimeStampArchEnabled()
        {
            return $"eTime={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
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
        private static string GetAccountIdQueryArchEnabled(long accountId)
        {
            return $"a={accountId}";
        }
        private static string GetRandomQuery()
        {
            return $"random={GetRandomNumber()}";
        }
        private static string GetEventName(string eventName)
        {
            return $"en={eventName}";
        }
        private static double GetRandomNumber()
        {
            Random random = new Random();
            return random.NextDouble();
        }
        private static string GetVisitorUserAgent(string visitorUserAgent = null)
        {
            return $"{UserAgent}={visitorUserAgent}";
        }
        private static string GetVisitorIP(string userIpAddress = null)
        {
            return $"{IP}={userIpAddress}";
        }
        internal async static Task<bool> TrackUserArchEnabled(AccountSettings settings, long accountId, int campaignId, int variationId, string userId, bool isDevelopmentMode, string sdkKey, Dictionary<string, int> usageStats, string visitorUserAgent = null, string userIpAddress = null)
        {
            string queryParams = GetQueryParamertersForTrackUserArchEnabled(accountId, usageStats);
            var trackUserRequest = new ApiRequest(Method.POST, isDevelopmentMode)
            {
                Uri = new Uri($"{Host}/{getDataLocation(settings, EventArchVerb)}?{queryParams}&{GetSdkKeyQuery(sdkKey)}"),
                logUri = new Uri($"{Host}/{getDataLocation(settings, EventArchVerb)}?{queryParams}"),
            };
            trackUserRequest.WithCaller(AppContext.ApiCaller);
            LogDebugMessage.ImpressionForTrackUserArchEnabled(file, accountId.ToString(), userId, campaignId.ToString());
            string PayLoad = GetTrackUserArchEnabledPayload(userId, accountId, sdkKey, sdkVersion, campaignId, variationId, visitorUserAgent, userIpAddress);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", sdkName);
            // adding respective headers to the http call
            if(visitorUserAgent != null){
                httpClient.DefaultRequestHeaders.Add(CUSTOM_HEADER_USER_AGENT, visitorUserAgent);
            }
            if(userIpAddress != null){
                httpClient.DefaultRequestHeaders.Add(CUSTOM_HEADER_IP, userIpAddress);
            }
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = new StringContent(PayLoad, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(trackUserRequest.Uri, data);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response.StatusCode < System.Net.HttpStatusCode.Ambiguous)
                {
                    LogDebugMessage.EventArchImpressionSuccess(file, accountId.ToString(), queryParams);
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                LogErrorMessage.UnableToDisplayHttpRequest(file, ex.StackTrace);
                return false;
            }
            return false;
        }
        internal async static Task<bool> TrackGoalArchEnabled(AccountSettings settings, int accountId, string goalIdentifier, string userId,
            string revenueValue, bool isDevelopmentMode, string sdkKey, Dictionary<string, int> metricMap, List<string> revenueListProp, Dictionary<string,dynamic> eventProperties, string visitorUserAgent = null, string userIpAddress = null)
        {
            string queryParams = GetQueryParamertersForTrackGoalArchEnabled(accountId, goalIdentifier);
            var trackUserRequest = new ApiRequest(Method.POST, isDevelopmentMode)
            {
                Uri = new Uri($"{Host}/{getDataLocation(settings, EventArchVerb)}?{queryParams}&{GetSdkKeyQuery(sdkKey)}"),
                logUri = new Uri($"{Host}/{getDataLocation(settings, EventArchVerb)}?{queryParams}"),
            };
            trackUserRequest.WithCaller(AppContext.ApiCaller);
            LogDebugMessage.ImpressionForTrackGoalArchEnabled(file, accountId.ToString(), userId, $"{GetCampaigns(metricMap)}", goalIdentifier);
            string PayLoad = GetGoalArchEnabledPayload(userId, accountId, sdkKey, sdkVersion, metricMap, revenueListProp, revenueValue, goalIdentifier, eventProperties, visitorUserAgent, userIpAddress);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", sdkName);
             if(visitorUserAgent != null){
                httpClient.DefaultRequestHeaders.Add(CUSTOM_HEADER_USER_AGENT, visitorUserAgent);
            }
            if(userIpAddress != null){
                httpClient.DefaultRequestHeaders.Add(CUSTOM_HEADER_IP, userIpAddress);
            }
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = new StringContent(PayLoad, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(trackUserRequest.Uri, data);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response.StatusCode < System.Net.HttpStatusCode.Ambiguous)
                {
                    LogDebugMessage.EventArchImpressionSuccess(file, accountId.ToString(), queryParams);
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                LogErrorMessage.UnableToDisplayHttpRequest(file, ex.StackTrace);
                return false;
            }
            return false;
        }
        internal async static Task<bool> PushTagsArchEnabled(AccountSettings settings, Dictionary<string, string> customDimensionMap, string userId, bool isDevelopmentMode, string sdkKey)
        {
            string queryParams = GetQueryParamertersForPushTagsArchEnabled(settings.AccountId);
            var trackUserRequest = new ApiRequest(Method.POST, isDevelopmentMode)
            {
                Uri = new Uri($"{Host}/{getDataLocation(settings, EventArchVerb)}?{queryParams}&{GetSdkKeyQuery(sdkKey)}"),
                logUri = new Uri($"{Host}/{getDataLocation(settings, EventArchVerb)}?{queryParams}"),
            };
            trackUserRequest.WithCaller(AppContext.ApiCaller);
            LogDebugMessage.ImpressionForPushTagArchEnabled(file, settings.AccountId.ToString(), userId, queryParams);
            string PayLoad = GetPushTagsArchEnabledPayload(userId, settings.AccountId, sdkKey, sdkVersion, customDimensionMap);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", sdkName);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = new StringContent(PayLoad, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(trackUserRequest.Uri, data);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response.StatusCode < System.Net.HttpStatusCode.Ambiguous)
                {
                    LogDebugMessage.EventArchImpressionSuccess(file, settings.AccountId.ToString(),queryParams);
                    return true;
                }
            }
            catch (HttpRequestException ex)
            {
                LogErrorMessage.UnableToDisplayHttpRequest(file, ex.StackTrace);
                return false;
            }
            return false;
        }
        private static string GetQueryParamertersForTrackUserArchEnabled(long accountId, Dictionary<string, int> usageStats)
        {

            return $"{GetEventName(TrackEventName)}" + "&p=FS" +
            $"&{GetAccountIdQueryArchEnabled(accountId)}" +
            $"&{GetRandomQuery()}" +
            $"&{GetUnixMsTimeStampArchEnabled()}" +
            $"{GetUsageStatsQuery(usageStats)}";
        }
        public static string GetTrackUserArchEnabledPayload(string userId, long accountId, string sdkKey, string sdkVersion, int campaignId, int variationId, string visitorUserAgent = null, string userIpAddress = null)
        {
            string payLoad = "{" +
                        "\"d\": {" +
                        "\"msgId\":\"" + UuidV5Helper.Compute(accountId, userId) + "-" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "\"," +
                        "\"visId\":\"" + UuidV5Helper.Compute(accountId, userId) + "\"," +
                        "\"sessionId\":" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "," +
                        "\"" + UserAgent + "\": \"" + visitorUserAgent + "\"," +
                        "\"" + IP + "\": \"" + userIpAddress + "\"," +
                        "\"event\": {" +
                                 "\"props\": {" +
                                            "\"sdkName\": \"" + sdkName + "\"," +
                                            "\"$visitor\": {\"props\": {\"vwo_fs_environment\": \"" + sdkKey + "\" }}," +
                                            "\"sdkVersion\": \"" + sdkVersion + "\"," +
                                            "\"id\": " + campaignId + "," +
                                            "\"variation\": " + variationId + "," +
                                            "\"isFirst\": 1" +
                                            "}," +
                                 "\"name\": \"vwo_variationShown\"," +
                                 "\"time\": " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + "" +
                                 "}," +
                      "\"visitor\": {" +
                                     "\"props\": {\"vwo_fs_environment\": \"" + sdkKey + "\"}" +
                                   "}" +
                         "}" +
                       "}";
            return payLoad;
        }
        private static string GetQueryParamertersForTrackGoalArchEnabled(long accountId, string goalIdentifier)
        {
            return $"{GetEventName(goalIdentifier)}" +
            $"&{GetAccountIdQueryArchEnabled(accountId)}" +
            $"&{GetRandomQuery()}" +
            $"&{GetUnixMsTimeStampArchEnabled()}";
        }
        public static string GetGoalArchEnabledPayload(string userId, long accountId, string sdkKey, string sdkVersion,
            Dictionary<string, int> metricMap, List<string> revenueListProp, string revenue, string goalIdentifier, Dictionary<string, dynamic> eventProperties = null, string visitorUserAgent = null, string userIpAddress = null)
        {
            string additionalParams = GetEventProp(eventProperties);
            string payLoad = "{" +
                        "\"d\": {" +
                        "\"msgId\":\"" + UuidV5Helper.Compute(accountId, userId) + "-" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "\"," +
                        "\"visId\":\"" + UuidV5Helper.Compute(accountId, userId) + "\"," +
                        "\"sessionId\":" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "," +
                        "\"" + UserAgent + "\": \"" + visitorUserAgent + "\"," +
                        "\"" + IP + "\": \"" + userIpAddress + "\"," +
                        "\"event\": {" +
                                 "\"props\": {" +
                                            "\"sdkName\":\"" + sdkName + "\"," +
                                            "\"$visitor\":{\"props\":{\"vwo_fs_environment\": \"" + sdkKey + "\" }}," +
                                            "\"sdkVersion\":\"" + sdkVersion + "\"," +
                                             "\"vwoMeta\":{\"metric\":{" + $"{GetGoal(metricMap)}" + "}" + $"{GetrevenueProp(revenueListProp, revenue)}" + "}" +
                                            "," +
                                  "\"isCustomEvent\":true "+ (additionalParams != "" ? "," : "") + additionalParams + "}," +
                                 "\"name\":\"" + goalIdentifier + "\"," +
                                 "\"time\":" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + "" +
                                 "}," +
                          "\"visitor\": {" +
                                     "\"props\":{\"vwo_fs_environment\":\"" + sdkKey + "\"}" +
                                   "}" +
                         "}" +
                       "}";
            return payLoad;

        }
        private static string GetGoal(Dictionary<string, int> metricMap)
        {
            string GoalString = "";
            if (metricMap != null && metricMap.Count != 0)
            {
                var listStats = new List<string>();
                foreach (var item in metricMap)
                {
                    listStats.Add("\"id_" + item.Key + "\"" + ":[\"g_" + item.Value + "\"]");
                }
                GoalString = string.Join(",", listStats);
            }
            return GoalString.TrimEnd(',');
        }
        private static string GetCampaigns(Dictionary<string, int> metricMap)
        {
            string GoalString = "";
            if (metricMap != null && metricMap.Count != 0)
            {
                var listStats = new List<string>();
                foreach (var item in metricMap)
                {
                    listStats.Add(item.Key);
                }
                GoalString = string.Join(";", listStats);
            }
            return GoalString.TrimEnd(';');
        }
        private static string GetrevenueProp(List<string> revenueListProp, string revenue)
        {
            string revenueProp = "";
            if (revenueListProp != null && revenueListProp.Count != 0 && revenue != null)
            {
                var listStats = new List<string>();
                foreach (var item in revenueListProp)
                {
                    listStats.Add("\"" + item + "\"" + ":" + revenue);
                }
                revenueProp = "," + string.Join(",", listStats);
            }
            return revenueProp.TrimEnd(',');
        }
        private static string GetEventProp(Dictionary<string, dynamic> eventProperties)
        {
            string eventProp= "";
            if (eventProperties != null && eventProperties.Count > 0){
                var listStats = new List<string>();
                foreach (var item in eventProperties)
                {
                    string valueString;
                    if (item.Value is string)
                    {
                        valueString = "\"" + item.Value + "\"";
                    }
                    else
                    {
                        valueString = item.Value.ToString().ToLower();
                    }
                    listStats.Add("\"" + item.Key + "\":" + valueString);
                }
                eventProp = string.Join(",",listStats);
             }
             return eventProp.TrimEnd(',');
        }
        private static string GetQueryParamertersForPushTagsArchEnabled(long accountId)
        {
            return $"{GetEventName(PushEventName)}" +
            $"&{GetAccountIdQueryArchEnabled(accountId)}" +
            $"&{GetRandomQuery()}" +
            $"&{GetUnixMsTimeStampArchEnabled()}";
        }
        public static string GetPushTagsArchEnabledPayload(string userId, long accountId, string sdkKey, string sdkVersion, Dictionary<string, string> customDimensionMap)
        {
            string payLoad = "{" +
                        "\"d\": {" +
                        "\"msgId\":\"" + UuidV5Helper.Compute(accountId, userId) + "-" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "\"," +
                        "\"visId\":\"" + UuidV5Helper.Compute(accountId, userId) + "\"," +
                        "\"sessionId\":" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "," +
                        "\"event\": {" +
                                 "\"props\": {" +
                                            "\"sdkName\": \"" + sdkName + "\"," +
                                            "\"$visitor\": {\"props\": {\"vwo_fs_environment\": \"" + sdkKey + "\"" + $"{GetTagValueMap(customDimensionMap)}" + "}}," +
                                            "\"sdkVersion\": \"" + sdkVersion + "\"," +
                                             "\"isCustomEvent\":true" +
                                            "}," +
                                 "\"name\": \"vwo_syncVisitorProp\"," +
                                 "\"time\": " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + "" +
                                 "}," +
                         "\"visitor\": {\"props\": {\"vwo_fs_environment\": \"" + sdkKey + "\"" + $"{GetTagValueMap(customDimensionMap)}" + "}}" +
                         "}" +
                       "}";
            return payLoad;
        }
        private static string GetTagValueMap(Dictionary<string, string> customDimensionMap)
        {
            string TagValues = "";
            if (customDimensionMap != null && customDimensionMap.Count != 0)
            {
                var listStats = new List<string>();
                foreach (var item in customDimensionMap)
                {
                    listStats.Add("\"" + item.Key + "\"" + ":\"" + item.Value + "\"");
                }
                TagValues = "," + string.Join(",", listStats);
            }
            return TagValues.TrimEnd(',');
        }
        public static Dictionary<string, dynamic> getEventArchQueryParams(long accountId, string sdkKey, Dictionary<string, int> usageStats)
        {
            var QueryParams = $"{GetEventName(TrackEventName)}" + "&p=FS" +
            $"&{GetAccountIdQueryArchEnabled(accountId)}" +
            $"&{GetRandomQuery()}" +
            $"&{GetUnixMsTimeStampArchEnabled()}" +
            $"&{ GetSdkKeyQuery(sdkKey)}" +
            $"{GetUsageStatsQuery(usageStats)}";
            Dictionary<string, dynamic> queryDict = new Dictionary<string, dynamic>();
            foreach (string token in QueryParams.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = token.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                    queryDict[parts[0].Trim()] = System.Web.HttpUtility.UrlDecode(parts[1]).Trim();
                else
                    queryDict[parts[0].Trim()] = "";
            }
            return queryDict;
        }
        public static Dictionary<string, dynamic> getEventArchTrackGoalParams(long accountId, string sdkKey, string goalIdentifier)
        {
            var QueryParams = $"{GetEventName(goalIdentifier)}" +
            $"&{GetAccountIdQueryArchEnabled(accountId)}" +
            $"&{GetRandomQuery()}" +
            $"&{ GetSdkKeyQuery(sdkKey)}" +
            $"&{GetUnixMsTimeStampArchEnabled()}";
            Dictionary<string, dynamic> queryDict = new Dictionary<string, dynamic>();
            foreach (string token in QueryParams.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = token.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                    queryDict[parts[0].Trim()] = System.Web.HttpUtility.UrlDecode(parts[1]).Trim();
                else
                    queryDict[parts[0].Trim()] = "";
            }
            return queryDict;
        }
        public static Dictionary<string, dynamic> getEventArchPushParams(long accountId, string sdkKey)
        {
            var QueryParams = $"{GetEventName(PushEventName)}" +
            $"&{GetAccountIdQueryArchEnabled(accountId)}" +
            $"&{GetRandomQuery()}" +
            $"&{ GetSdkKeyQuery(sdkKey)}" +
            $"&{GetUnixMsTimeStampArchEnabled()}";
            Dictionary<string, dynamic> queryDict = new Dictionary<string, dynamic>();
            foreach (string token in QueryParams.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = token.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                    queryDict[parts[0].Trim()] = System.Web.HttpUtility.UrlDecode(parts[1]).Trim();
                else
                    queryDict[parts[0].Trim()] = "";
            }
            return queryDict;
        }
        private static string getDataLocation(AccountSettings settings, string path){
            if(settings.collectionPrefix != null){
                return settings.collectionPrefix + "/" + path;
            }
            return path;
        }
    }
}
