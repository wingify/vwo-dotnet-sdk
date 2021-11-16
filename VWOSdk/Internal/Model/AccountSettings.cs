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

using System.Collections.Generic;

namespace VWOSdk
{
    internal class AccountSettings : Settings
    {
        public AccountSettings(string sdkKey, List<BucketedCampaign> campaigns, int accountId, int version, Dictionary<string, Groups> groups, Dictionary<string, dynamic> campaignGroups,bool isEventArchEnabled=false) : base(sdkKey, null, accountId, version,groups,campaignGroups, isEventArchEnabled)
        {
            this.Campaigns = campaigns;
            this.CampaignGroups = CampaignGroups;
            this.Groups = groups;
        }
        public new List<BucketedCampaign> Campaigns { get; set; }
        public new Dictionary<string, dynamic> CampaignGroups { get; set; }
        public new Dictionary<string, Groups> Groups { get; set; }
    }
}
