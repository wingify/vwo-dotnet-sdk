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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;

namespace VWOSdk.Tests
{
    public class MetaDataTest
    {
        internal static Settings settings = new FileReaderApiCaller("SampleGroupSettingsFile").GetJsonContent<Settings>();
        internal static ILogWriter Logger { get; set; } = new DefaultLogWriter();
        private static IVWOClient vwoInstance { get; set; }
        [Fact]
        public void Activate_Should_Return_Null_When_Metadata_Passes_Not_Object()
        {
            VWO.Configure(new Validator());

            vwoInstance = VWO.Launch(settings, true);

            Logger.WriteLog(LogLevel.DEBUG, "should return a variation as whitelisting is satisfied for the called campaign");
            var Campaigns = settings.getCampaigns();
            var calledCampaignKey = Campaigns[2].Key;
            Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
            {{
                 "metaData", false
            }};
            var variationName = vwoInstance.Activate(calledCampaignKey, "Ashley", Options);
            Assert.Null(variationName);
        }
        [Fact]
        public void Activate_Should_Return_NotNull_When_Metadata_Passes_Object()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "should return null as other campaign satisfies the whitelisting");
            var Campaigns = settings.getCampaigns();
            var calledCampaignKey = Campaigns[2].Key;
            Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
            {{
               "metaData", new Dictionary<string, dynamic>()
              {
                  {
                     "username", "any_user"
                  }
              }
            }};
            var variationName = vwoInstance.Activate(calledCampaignKey, "Ashley", Options);
            Assert.NotNull(variationName);
        }
        [Fact]
        public void storagePassedForCalledCampaign()
        {
            VWO.Configure(new Validator());
            Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
            {{
               "metaData", new Dictionary<string, dynamic>()
              {
                  {
                     "username", "any_user"
                  }
              }
            }};
            UserStorageService._userStorageMap = null;
            vwoInstance = VWO.Launch(settings, true, userStorageService: new UserStorageService());
            Logger.WriteLog(LogLevel.DEBUG, "should return variation as storage is satisfied for the called campaign");
            var Campaigns = settings.getCampaigns();
            var campaignKey = Campaigns[2].Key;
            vwoInstance.Activate(campaignKey, "Ashley", Options);
            Assert.Equal("1", UserStorageService.getStorage().Count.ToString());
            string variationName = vwoInstance.GetVariationName(campaignKey, "Ashley", Options);
            Assert.Equal("1", UserStorageService.getStorage().Count.ToString());
            bool isGoalTracked = vwoInstance.Track(campaignKey, "Ashley", "CUSTOM", Options);
            Assert.True(isGoalTracked);
            Dictionary<string, dynamic> metaData = Options.ContainsKey("metaData") && Options["metaData"] is Dictionary<string, dynamic> ? Options["metaData"] : null;
            UserStorageService.getStorage().TryGetValue(campaignKey, out ConcurrentDictionary<string, Dictionary<string, dynamic>> userMap);
            Assert.Equal("CUSTOM", userMap["Ashley"]["GoalIdentifier"]);
            Assert.Equal(variationName, userMap["Ashley"]["VariationName"]);
            Assert.Equal(metaData, userMap["Ashley"]["MetaData"]);
        }

    }
}
