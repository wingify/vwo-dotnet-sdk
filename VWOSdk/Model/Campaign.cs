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
    public class Campaign
    {
        [JsonConstructor]
        internal Campaign(int id, string Name, double PercentTraffic, string Key, string Status, string Type, List<Goal> goals, List<Variation> variations, bool isForcedVariationEnabled, bool isBucketingSeedEnabled, Dictionary<string, dynamic> segments = null, List<Dictionary<string, dynamic>> Variables = null, bool isOB = false, bool isOBv2= false)
        {
            this.PercentTraffic = PercentTraffic;
            this.Key = Key;
            this.Name = Name;
            this.Status = Status;
            this.Type = Type;
            this.Id = id;
            this.Goals = goals;
            this.Variations = variations;
            this.IsForcedVariationEnabled = isForcedVariationEnabled;
            this.IsBucketingSeedEnabled = isBucketingSeedEnabled;
            if (segments == null) segments = new Dictionary<string, dynamic>();
            this.Segments = segments;
            if (Variables == null) Variables = new List<Dictionary<string, dynamic>>();
            this.Variables = Variables;
            this.isOB = isOB;
            this.isOBv2 = isOBv2;
        }

        public IReadOnlyList<Goal> Goals { get; internal set; }
        public IReadOnlyList<Variation> Variations { get; internal set; }
        public int Id { get; internal set; }
        public double PercentTraffic { get; internal set; }
        public string Key { get; internal set; }
        public string Name { get; internal set; }
        public string Status { get; internal set; }
        public string Type { get; internal set; }
        public bool IsForcedVariationEnabled { get; internal set; }
        public bool IsBucketingSeedEnabled { get; internal set; }
        public Dictionary<string, dynamic> Segments { get; internal set; }
        public List<Dictionary<string, dynamic>> Variables { get; internal set; }
        public Dictionary<string, dynamic> getSegments() { return this.Segments; }
        public void setSegments(Dictionary<string, dynamic> segment)
        {
            this.Segments = segment;
        }
        public void setPercentTraffic(double PercentTraffic)
        {
            this.PercentTraffic = PercentTraffic;
        }
        public bool isOB { get; internal set; }
        public bool isOBv2 { get; internal set; }
    }
}
