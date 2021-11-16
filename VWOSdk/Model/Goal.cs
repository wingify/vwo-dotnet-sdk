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

namespace VWOSdk
{
    public class Goal
    {
        [JsonConstructor]
        internal Goal(int id, string identifier, string type, string revenueProp = null)
        {
            this.Id = id;
            this.Identifier = identifier;
            this.Type = type;
            this.RevenueProp = revenueProp;
        }
        public string Identifier { get; internal set; }
        public int Id { get; internal set; }
        public string Type { get; internal set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RevenueProp { get; set; }
        internal bool IsRevenueType()
        {
            return this.Type.Equals("REVENUE_TRACKING");
        }
        internal string GetRevenueProp()
        {
            return RevenueProp;
        }
    }
}
