#pragma warning disable 1587
/**
 * Copyright 2019-2020 Wingify Software Pvt. Ltd.
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

namespace VWOSdk
{
    internal class SettingsProcessor : ISettingsProcessor
    {
        private static readonly string file = typeof(SettingsProcessor).FullName;
        public AccountSettings ProcessAndBucket(Settings settings)
        {
            try
            {
                var campaigns = Process(settings.Campaigns);
                return new AccountSettings(settings.SdkKey, campaigns, settings.AccountId, settings.Version);
            }
            catch (Exception exception)
            {
            }
            LogErrorMessage.ProjectConfigCorrupted(file);
            return null;
        }

        private List<BucketedCampaign> Process(IReadOnlyList<Campaign> campaigns)
        {
            List<BucketedCampaign> bucket = new List<BucketedCampaign>();
            foreach (var campaign in campaigns)
            {
                bucket.Add(Process(campaign));
            }
            return bucket;
        }

        private BucketedCampaign Process(Campaign campaign)
        {
            return new BucketedCampaign(campaign.Id, campaign.PercentTraffic, campaign.Key, campaign.Status, campaign.Type, campaign.Segments, campaign.Variables)
            {
                Goals = ToDictionary(campaign.Goals, (goal) => goal.Identifier),
                Variations = Bucket(campaign.Variations, campaign.Key)
            };
        }

        private Dictionary<TKey, TVal> ToDictionary<TKey, TVal>(IEnumerable<TVal> enumerable, Func<TVal, TKey> keySelector)
        {
            var result = new Dictionary<TKey, TVal>();
            if (enumerable != null)
            {
                foreach (var item in enumerable)
                {
                    result[keySelector(item)] = item;
                }
            }
            return result;
        }

        private RangeBucket<Variation> Bucket(IReadOnlyList<Variation> variations, string campaignKey)
        {
            RangeBucket<Variation> bucket = new RangeBucket<Variation>(Constants.Variation.MAX_TRAFFIC_VALUE);
            foreach (var variation in variations)
            {
                bucket.Add(variation.Weight, variation, out double start, out double end);
                LogInfoMessage.VariationRangeAllocation(file, campaignKey, variation.Name, variation.Weight, start, end);
            }
            return bucket;
        }
    }
}
