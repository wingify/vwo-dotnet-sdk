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
using System;
using System.Collections.Generic;
using System.Linq;
namespace VWOSdk
{
    /// <summary>
    /// Build Query Params For Post.
    /// </summary>
    internal class BuildQueryParams
    {
        public string u;
        public object r;
        public long sId;
        public string t;
        public int? e = null;
        public int? c = null;
        public int eT;
        public int? g = null;
        public string ap;
        public string ed;
        public BuildQueryParams(Builder builder)
        {
            this.u = builder.u;
            this.r = builder.r;
            this.sId = builder.sId;
            this.t = builder.t;
            this.e = builder.e;
            this.c = builder.c;
            this.eT = builder.eT;
            this.g = builder.g;
            this.ap = builder.ap;
            this.ed = builder.ed;

        }
        public class Builder
        {
            public string u;
            public object r;
            public long sId;
            public string t;
            public int? e = null;
            public int? c = null;
            public int eT;
            public int? g = null;
            public string ap;
            public string ed;
            public Builder withMinifiedCampaignId(int campaignId)
            {
                this.e = campaignId;
                return this;
            }
            public Builder withAp()
            {
                this.ap = "server";
                return this;
            }
            public Builder withEd()
            {
                this.ed = "{\"p\":\"server\"}";
                return this;
            }
            public Builder withUuid(long account_id, string uId)
            {
                this.u = UuidV5Helper.Compute(account_id, uId);
                return this;
            }
            public Builder withMinifiedVariationId(int variationId)
            {
                this.c = variationId;
                return this;
            }
            public Builder withSid(long sId)
            {
                this.sId = sId;
                return this;
            }
            public Builder withMinifiedGoalId(int goal_id)
            {
                this.g = goal_id;
                return this;
            }
            public Builder withRevenue(object r)
            {
                this.r = r;
                return this;
            }
            public Builder withMinifiedEventType(int eventType)
            {
                this.eT = eventType;
                return this;
            }
            public Builder withMinifiedTags(String tagKey, String tagValue)
            {
                this.t = "{\"u\":{\"" + tagKey + "\":\"" + tagValue + "\"}}";
                return this;
            }
            public static Builder getInstance()
            {
                return new Builder();
            }
            public BuildQueryParams build()
            {
                return new BuildQueryParams(this);
            }
        }
        public IDictionary<string, dynamic> removeNullValues(BuildQueryParams val)
        {
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(val);
            var allValue = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(jsonString);
            var withoutNull = allValue.Where(f => f.Value != null).ToDictionary(x => x.Key, x => x.Value);
            return withoutNull;
        }
    }
}
