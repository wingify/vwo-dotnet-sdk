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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace VWOSdk
{
    public class Settings
    {


        [JsonConstructor]
        internal Settings(string sdkKey, List<Campaign> campaigns, int accountId, int version, Dictionary<string, Groups> groups, Dictionary<string, dynamic> campaignGroups, bool isEventArchEnabled=false, string collectionPrefix="", bool isNB=false)
        {
            this.SdkKey = sdkKey;
            this.Campaigns = campaigns;
            this.AccountId = accountId;
            this.Version = version;
            this.Groups = groups;
            this.CampaignGroups = campaignGroups;
            this.IsEventArchEnabled = isEventArchEnabled;
            this.collectionPrefix = collectionPrefix;
            this.isNB = isNB;
        }

        public string SdkKey { get; internal set; }
        public List<Campaign> Campaigns { get; internal set; }
        public Dictionary<string, dynamic> CampaignGroups { get; internal set; }
        public Dictionary<string, Groups> Groups { get; internal set; }
        public int AccountId { get; internal set; }
        public int Version { get; internal set; }
        public Dictionary<string, dynamic> getCampaignGroups()
        {
            return CampaignGroups;
        }
        public Dictionary<string, Groups> getGroups()
        {
            return Groups;
        }
        public List<Campaign> getCampaigns()
        {
            return Campaigns;
        }
        public bool IsEventArchEnabled { get; internal set; }
        public string collectionPrefix { get; internal set; }
        public bool isNB { get; internal set; }
    }
}