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

using System.Collections.Generic;
using System;

namespace VWOSdk
{
    internal class VariationAllocator : IVariationAllocator
    {
        private static readonly string file = typeof(VariationAllocator).FullName;
        private readonly IBucketService _userHasher;
        internal VariationAllocator(IBucketService userHasher = null)
        {
            this._userHasher = userHasher;
        }

        /// <summary>
        /// Allocate Variation by checking previously assigned variation if userStorageMap is provided, else by computing User Hash and matching it in bucket for eligible variation.
        /// </summary>
        /// <param name="userStorageMap"></param>
        /// <param name="campaign"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Variation Allocate(UserStorageMap userStorageMap, BucketedCampaign campaign, string userId)
        {
            if (campaign == null)
                return null;

            if (userStorageMap == null)
            {
                double maxVal = Constants.Variation.MAX_TRAFFIC_VALUE;
                double multiplier = maxVal / campaign.PercentTraffic / 100; ///This is to evenly spread all user among variations.
                var bucketValue = this._userHasher.ComputeBucketValue(userId, maxVal, multiplier, out double hashValue);
                var selectedVariation = campaign.Variations.Find(bucketValue);
                LogDebugMessage.VariationHashBucketValue(file, userId, campaign.Key, campaign.PercentTraffic, hashValue, bucketValue);
                return selectedVariation;
            }

            return campaign.Variations.Find(userStorageMap.VariationName, GetVariationName);
        }

        public Variation TargettedVariation(string userId, List<Variation> whiteListedVariations)
        {
            int whiteListedVariationsLength = whiteListedVariations.Count;
            RangeBucket<Variation> whiteListedVariationsList = new RangeBucket<Variation>();
            Variation targettedVariation;
            if (whiteListedVariationsLength == 0)
            {
                return null;
            }
            else if (whiteListedVariationsLength == 1)
            {
                targettedVariation = whiteListedVariations[0];
            }
            else
            {
                whiteListedVariations = ScaleVariations(whiteListedVariations);
                whiteListedVariationsList = GetVariationAllocationRanges(whiteListedVariations);
                double maxVal = Constants.Variation.MAX_TRAFFIC_VALUE;
                double multiplier = 1;
                var bucketValue = this._userHasher.ComputeBucketValue(userId, Constants.Campaign.MAX_TRAFFIC_PERCENT, multiplier);
                targettedVariation = whiteListedVariationsList.Find(bucketValue);
            }
            return targettedVariation;
        }

        public RangeBucket<Variation> GetVariationAllocationRanges(List<Variation> variations)
        {
            int currentAllocation = 0;
            RangeBucket<Variation> bucket = new RangeBucket<Variation>(Constants.Variation.MAX_TRAFFIC_VALUE);
            foreach (var variation in variations)
            {
                int stepFactor = GetVariationBucketingRange(variation.Weight);
                if (stepFactor != 0)
                {
                    bucket.AddNew(variation, currentAllocation + 1, currentAllocation + stepFactor);
                    currentAllocation += stepFactor;
                }
                else
                {
                    bucket.AddNew(variation, -1, -1);
                }
            }
            return bucket;
        }

        public int GetVariationBucketingRange(double weight)
        {
            if (weight == 0)
            {
                return 0;
            }
            double startRange = Convert.ToInt32(Math.Ceiling(weight * 100));
            return Convert.ToInt32(Math.Min(startRange, 10000));
        }

        public List<Variation> ScaleVariations(List<Variation> variations)
        {
            double weightSum = 0.0;
            List<Variation> clonedWhiteListedVariations = new List<Variation> {};
            foreach (var variation in variations)
            {
                weightSum += variation.Weight;
                clonedWhiteListedVariations.Add(variation.CloneJson());
            }

            if (weightSum == 0)
            {
                double normalizedWeight = 100.0 / variations.Count;
                foreach (var variation in clonedWhiteListedVariations)
                {
                    variation.Weight = normalizedWeight;
                }
            }
            else
            {
                foreach (var variation in clonedWhiteListedVariations)
                {
                    variation.Weight = (variation.Weight / weightSum) * 100;
                }
            }
            return clonedWhiteListedVariations;
        }

        public Variation GetSavedVariation(BucketedCampaign campaign, string variationName)
        {
            return campaign.Variations.Find(variationName, GetVariationName);
        }

        private string GetVariationName(Variation variation)
        {
            return variation.Name;
        }
        internal int GetVariationId(Variation variation)
        {
            return variation.Id;
        }
    }
}
