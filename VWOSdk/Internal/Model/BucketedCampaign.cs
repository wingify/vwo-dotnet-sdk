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

using System.Collections.Generic;

namespace VWOSdk
{
    internal class BucketedCampaign : Campaign
    {
        public BucketedCampaign(int id, string Name, double PercentTraffic, string Key, string Status, string Type, bool IsForcedVariationEnabled,
            bool IsBucketingSeedEnabled, Dictionary<string, dynamic> Segments = null, List<Dictionary<string, dynamic>> Variables = null,bool isOB = false, bool isOBv2 = false)
            : base(id, Name, PercentTraffic, Key, Status, Type, null, null, IsForcedVariationEnabled,IsBucketingSeedEnabled, Segments, Variables, isOB, isOBv2)
        {

        }
        public BucketedCampaign clone()
        {
            try
            {
                return (BucketedCampaign)base.MemberwiseClone();
            }
            catch
            {
                return this;
            }
        }
        public new RangeBucket<Variation> Variations { get; set; }
        public new Dictionary<string, Goal> Goals { get; set; }
        public new double Weight { get; set; }
        public new double StartRange { get; set; }
        public new double EndRange { get; set; }
    }
}
