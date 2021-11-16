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
    public class UserStorageMap
    {
        public UserStorageMap() { }
        public UserStorageMap(string userId, string campaignKey, string variationName, string goalIdentifier = null, Dictionary<string, dynamic> metaData = null)
        {
            this.UserId = userId;
            this.CampaignKey = campaignKey;
            this.VariationName = variationName;
            this.GoalIdentifier = goalIdentifier;
            this.MetaData = metaData;
        }

        public string UserId { get; set; }
        public string CampaignKey { get; set; }
        public string VariationName { get; set; }
        public string GoalIdentifier { get; set; }
        public Dictionary<string, dynamic> MetaData { get; set; }
    }
}
