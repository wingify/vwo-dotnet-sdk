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

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using System;
namespace VWOSdk.Tests
{
    public class EuTest
    {
        internal static Settings settings = new FileReaderApiCaller("SampleSettingsFile").GetJsonContent<Settings>();
        internal static ILogWriter Logger { get; set; } = new DefaultLogWriter();
        private static IVWOClient vwoInstance { get; set; }
        private static ApiRequest apiRequest;
        [Fact]
        public void TrackUser_EventArch_EuAccount_Test()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "TrackUser payload and queryParams for enabled event arch");
            ApiRequest apiRequest = ServerSideVerb.TrackUser(GetAccountSettings(IsEventArchEnabled: true, collectionPrefix: "eu01"), settings.AccountId, 20, 3, "Ashely", true, settings.SdkKey, null);
            Assert.True(apiRequest.Uri.ToString().ToLower().Contains("eu01"));
        }
        [Fact]
        public void TrackUser_EventArch_Test()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "TrackUser payload and queryParams for enabled event arch");
            ApiRequest apiRequest = ServerSideVerb.TrackUser(GetAccountSettings(IsEventArchEnabled: true), settings.AccountId, 20, 3, "Ashely", true, settings.SdkKey, null);
            Assert.False(apiRequest.Uri.ToString().ToLower().Contains("eu01"));
        }
        [Fact]
        public void TrackUser_NonEventArch_EuAccount_Test()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "TrackUser payload and queryParams for enabled event arch");
            ApiRequest apiRequest = ServerSideVerb.TrackUser(GetAccountSettings(collectionPrefix: "eu01"), settings.AccountId, 20, 3, "Ashely", true, settings.SdkKey, null);
            Assert.True(apiRequest.Uri.ToString().ToLower().Contains("eu01"));
        }
        [Fact]
        public void TrackUser_NonEventArch_Test()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "TrackUser payload and queryParams for enabled event arch");
            ApiRequest apiRequest = ServerSideVerb.TrackUser(GetAccountSettings(), settings.AccountId, 20, 3, "Ashely", true, settings.SdkKey, null);
            Assert.False(apiRequest.Uri.ToString().ToLower().Contains("eu01"));
        }
        private AccountSettings GetAccountSettings(string status = "RUNNING", bool IsEventArchEnabled = true, string collectionPrefix = "")
        {
            var campaigns = GetCampaigns(status: status);
            return new AccountSettings(settings.SdkKey, campaigns, 1, 1, null, null, IsEventArchEnabled, collectionPrefix);
        }

        private List<BucketedCampaign> GetCampaigns(string status = "RUNNING")
        {
            var result = new List<BucketedCampaign>();
            result.Add(GetCampaign(status: status));
            return result;
        }

        private BucketedCampaign GetCampaign(string campaignKey = null, string variationName = null, string status = "RUNNING")
        {
            campaignKey = campaignKey ?? settings.SdkKey;
            variationName = variationName ?? "Control";
            return new BucketedCampaign(1, "test", 70, campaignKey, status, null, false,false)
            {
                Variations = GetVariations(variationName),
            };
        }

        private RangeBucket<Variation> GetVariations(string variationName)
        {
            var result = new RangeBucket<Variation>(10000);
            result.Add(100, new Variation(1, variationName, null, 100, false));
            return result;
        }
    }
}
