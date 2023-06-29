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
using System;
using System.Linq;

namespace VWOSdk
{
    internal class CampaignHelper
    {
        /// <summary>
        ///  Get bucketing seed .
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="campaign"></param>
        /// <param name="groupId"></param>
        /// <returns>
        /// Bucketing seed value.
        /// </returns>
        public static string getBucketingSeed(string userId, Campaign campaign, int? groupId, bool isNewBucketingEnabled = false)
        {
            if (groupId != null)
            {
                return groupId + "_" + userId;
            }

            if (campaign != null && (isNewBucketingEnabled || campaign.IsBucketingSeedEnabled))
            {
                return campaign.Id + "_" + userId;
            }
            else
            {
                return userId;
            }
        }
        /// <summary>
        ///  Check if the campaign is a part of mutually exclusive group.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="campaignId"></param>
        /// <returns>
        /// Group id and name for the campaign.
        /// </returns>
        public static Dictionary<string, dynamic> isPartOfGroup(Settings settings, int campaignId)
        {
            Dictionary<string, dynamic> groupDetails = new Dictionary<string, dynamic>();
            if (settings.CampaignGroups != null && settings.CampaignGroups.ContainsKey(campaignId.ToString()))
            {
                settings.getCampaignGroups().TryGetValue(campaignId.ToString(), out dynamic groupId);
                settings.getGroups().TryGetValue(groupId.ToString(), out Groups group);
                groupDetails["groupId"] = groupId;
                groupDetails["groupName"] = group.Name;
            }
            return groupDetails;
        }
        /// <summary>
        ///  Get the list of campaigns on the basis of their id.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="groupId"></param>
        /// <returns>
        /// list of campaigns
        /// </returns>
        public static List<BucketedCampaign> getGroupCampaigns(AccountSettings settings, int groupId)
        {
            List<BucketedCampaign> campaignList = new List<BucketedCampaign>();
            if (settings.getGroups().ContainsKey(groupId.ToString()))
            {
                settings.getGroups().TryGetValue(groupId.ToString(), out Groups group);
                foreach (int campaignId in group.Campaigns)
                {
                    BucketedCampaign campaign = getCampaignBasedOnId(settings, campaignId);
                    if (campaign != null)
                    {
                        campaignList.Add(campaign);
                    }
                }
            }
            return campaignList;
        }
        /// <summary>
        ///  Get the campaign on the basis of campaign id.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="campaignId"></param>
        /// <returns>
        ///  Campaign object.
        /// </returns>
        private static BucketedCampaign getCampaignBasedOnId(AccountSettings settings, int campaignId)
        {
            BucketedCampaign campaign = null;
            foreach (BucketedCampaign eachCampaign in settings.Campaigns)
            {
                if (eachCampaign.Id == campaignId)
                {
                    campaign = eachCampaign;
                    break;
                }
            }
            return campaign;
        }
        /// <summary>
        ///  Get the winning campaign on the basis of bucketing value.
        /// </summary>
        /// <param name="itemList"></param>
        /// <param name="hashValue"></param>
        /// <returns>
        /// BucketedCampaign object.
        /// </returns>
        public static BucketedCampaign getAllocatedItem(List<BucketedCampaign> itemList, double hashValue)
        {
            foreach (BucketedCampaign item in itemList)
            {
                if (hashValue >= item.StartRange && hashValue <= item.EndRange)
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        ///  Get campaign bucketing range.
        /// </summary>
        /// <param name="weight"></param>
        /// <returns>
        /// integer value.
        /// </returns>
        public static int GetCampaignBucketingRange(double weight)
        {
            if (weight == 0)
            {
                return 0;
            }
            double startRange = Convert.ToInt32(Math.Ceiling(weight * 100));
            return Convert.ToInt32(Math.Min(startRange, 10000));
        }
        /// <summary>
        /// Check for eligible and inEligible campaigns.
        /// </summary>
        /// <param name="_campaignAllocator"></param>
        /// <param name="_segmentEvaluator"></param>
        /// <param name="campaignList"></param>
        /// <param name="userId"></param>
        /// <param name="apiName"></param>
        /// <param name="customVariables"></param>
        /// <returns>
        /// Eligible and inEligible campaigns
        /// </returns>
        public static Dictionary<string, dynamic> getEligibleCampaigns(ICampaignAllocator _campaignAllocator, ISegmentEvaluator _segmentEvaluator,
            List<BucketedCampaign> campaignList, string userId, string apiName, Dictionary<string, dynamic> customVariables)
        {
            List<BucketedCampaign> eligibleCampaigns = new List<BucketedCampaign>();
            List<BucketedCampaign> inEligibleCampaigns = new List<BucketedCampaign>();
            Dictionary<string, dynamic> rCampaigns = new Dictionary<string, dynamic>();
            foreach (BucketedCampaign campaign in campaignList)
            {
                BucketedCampaign selectedCampaign = _campaignAllocator.AllocateByTrafficAllocation(userId, campaign);
                if (selectedCampaign != null && checkForPreSegmentation(_segmentEvaluator, campaign, userId, campaign.Key, apiName, customVariables, true))
                {
                    eligibleCampaigns.Add(campaign.clone());
                }
                else
                {
                    inEligibleCampaigns.Add(campaign.clone());
                }
            }
            rCampaigns["eligibleCampaigns"] = eligibleCampaigns;
            rCampaigns["inEligibleCampaigns"] = inEligibleCampaigns;
            return rCampaigns;
        }
        /// <summary>
        /// Check for pre segmentation of the selected campaign.
        /// </summary>
        /// <param name="_segmentEvaluator"></param>
        /// <param name="campaign"></param>
        /// <param name="userId"></param>
        /// <param name="campaignKey"></param>
        /// <param name="apiName"></param>
        /// <param name="customVariables"></param>
        /// <param name="disableLogs"></param>
        /// <returns>
        /// True or False
        /// </returns>
        public static bool checkForPreSegmentation(ISegmentEvaluator _segmentEvaluator, Campaign campaign, string userId, string campaignKey,
            string apiName, Dictionary<string, dynamic> customVariables, bool disableLogs)
        {
            bool isPresegmentValid = true;
            if (campaign.Segments.Count > 0)
            {
                string segmentationType = Constants.SegmentationType.PRE_SEGMENTATION;
                if (customVariables == null)
                {
                    LogInfoMessage.NoCustomVariables(typeof(IVWOClient).FullName, userId, campaignKey, apiName, disableLogs);
                    customVariables = new Dictionary<string, dynamic>();
                }
                if (!_segmentEvaluator.evaluate(userId, campaignKey, segmentationType, campaign.Segments, customVariables))
                {
                    isPresegmentValid = false;
                }
            }
            else
                LogInfoMessage.SkippingPreSegmentation(typeof(IVWOClient).FullName, userId, campaignKey, apiName, disableLogs);
            return isPresegmentValid;

        }
        /// <summary>
        ///  Get the winning campaign from the shortlisted Campaigns.
        /// </summary>
        /// <param name="campaignAllocator"></param>
        /// <param name="shortlistedCampaigns"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns>
        /// BucketedCampaign object.
        /// </returns>
        public static BucketedCampaign normalizeAndFindWinningCampaign(ICampaignAllocator campaignAllocator, List<BucketedCampaign> shortlistedCampaigns, string userId, string groupId)
        {
            int currentAllocation = 0;
            foreach (BucketedCampaign campaign in shortlistedCampaigns)
            {
                campaign.Weight = 100.0 / shortlistedCampaigns.Count;
                int stepFactor = CampaignHelper.GetCampaignBucketingRange(campaign.Weight);
                if (stepFactor != 0)
                {
                    campaign.StartRange = currentAllocation + 1;
                    campaign.EndRange = currentAllocation + stepFactor;

                    currentAllocation += stepFactor;
                }
            }
            double bucketValue = campaignAllocator.GetUserHashForCampaign(userId, Convert.ToInt32(groupId));
            BucketedCampaign winnerCampaign = getAllocatedItem(shortlistedCampaigns, bucketValue);
            return winnerCampaign;
        }
        /// <summary>
        ///  Get the winning campaign from the shortlisted Campaigns using New MEG implementation.
        /// </summary>
        /// <param name="campaignAllocator"></param>
        /// <param name="shortlistedCampaigns"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns>
        /// BucketedCampaign object.
        /// </returns>
        public static BucketedCampaign advancedAlgoFindWinningCampaign(ICampaignAllocator campaignAllocator, List<BucketedCampaign> shortlistedCampaigns, string userId, string groupId, AccountSettings settings)
        {
            BucketedCampaign winnerCampaign = null;
            bool found = false; // flag to check whether winnerCampaign has been found or not and helps to break from the outer loop

            List<int> priorityOrder = settings.getGroups()[groupId.ToString()].p != null && settings.getGroups()[groupId.ToString()].p.Count>0  ? settings.getGroups()[groupId.ToString()].p : new List<int> ();
            Dictionary<string, double> wt = settings.getGroups()[groupId.ToString()].wt != null && settings.getGroups()[groupId.ToString()].wt.Count>0? settings.getGroups()[groupId.ToString()].wt : new Dictionary<string, double>();
        
            for (int i = 0; i < priorityOrder.Count; i++)
            {
                for (int j = 0; j < shortlistedCampaigns.Count; j++)
                {
                    if (shortlistedCampaigns[j].Id == priorityOrder[i])
                    {
                        winnerCampaign = shortlistedCampaigns[j];
                        found = true;
                        break;
                    }
                }
                if(found == true)
                    break;
            }
            // If winnerCampaign not found through Priority, then go for weighted Random distribution and for that,
            // Store the list of campaigns (participatingCampaigns) out of eligibleCampaigns and their corresponding weights which are present in weightage distribution array (wt) in 2 different lists
            if (winnerCampaign == null)
            {
                List<double> weights = new List<double>();
                List<BucketedCampaign> participatingCampaignList = new List<BucketedCampaign>();

                for (int i = 0; i < shortlistedCampaigns.Count; i++)
                {
                    string campaignId = shortlistedCampaigns[i].Id.ToString();
                    if (wt.ContainsKey(campaignId))
                    {
                        weights.Add(wt[campaignId]);
                        participatingCampaignList.Add(shortlistedCampaigns[i]);
                    }
                }

                /*
                * Finding winner campaign using weighted random distribution :
                1. Calculate the sum of all weights
                2. Generate a random number between 0 and the weight sum:
                3. Iterate over the weights array and subtract each weight from the random number until the random number becomes negative. The corresponding ith value is the required value
                4. Set the ith campaign as WinnerCampaign
                */
                double weightSum = weights.Sum();
                Random random = new Random();
                int randomNumber = random.Next(1, (int)weightSum);

                double sum = 0;
                for (int i = 0; i < weights.Count; i++)
                {
                    sum += weights[i];
                    if (randomNumber < sum)
                    {
                        winnerCampaign = participatingCampaignList[i];
                        break;
                    }
                }
            }

            return winnerCampaign;
            
        }
        /// <summary>
        ///  Check for Storage and Whitelisting of the shortlisted Campaigns.
        /// </summary>
        /// <param name="_variationAllocator"></param>
        /// <param name="_userStorageService"></param>
        /// <param name="_segmentEvaluator"></param>
        /// <param name="file"></param>
        /// <param name="apiName"></param>
        /// <param name="campaignList"></param>
        /// <param name="groupName"></param>
        /// <param name="calledCampaign"></param>
        /// <param name="userId"></param>
        /// <param name="variationTargetingVariables"></param>
        /// <param name="customVariables"></param>
        /// <param name="userStorageData"></param>
        /// <param name="disableLogs"></param>
        /// <returns>
        ///True or False.
        /// </returns>
        public static bool checkForStorageAndWhitelisting(IVariationAllocator _variationAllocator, UserStorageAdapter _userStorageService, ISegmentEvaluator _segmentEvaluator,
            string file, string apiName, List<BucketedCampaign> campaignList, string groupName,
           BucketedCampaign calledCampaign, string userId,
           Dictionary<string, dynamic> variationTargetingVariables,
           Dictionary<string, dynamic> customVariables, bool isNewBucketingEnabled = false, Dictionary<string, dynamic> userStorageData = null, bool disableLogs = false)
        {
            bool otherCampaignWinner = false;
            foreach (BucketedCampaign campaign in campaignList)
            {
                if (campaign.Id.Equals(calledCampaign.Id))
                {
                    continue;
                }
                List<Variation> whiteListedVariations = GetWhiteListedVariationsList(_segmentEvaluator, apiName, userId, campaign, campaign.Key, customVariables, variationTargetingVariables, disableLogs);
                string status = Constants.WhitelistingStatus.FAILED;
                string variationString = " ";
                Variation variation = _variationAllocator.TargettedVariation(userId, campaign, whiteListedVariations, isNewBucketingEnabled);
                if (variation != null)
                {
                    status = Constants.WhitelistingStatus.PASSED;
                    variationString = $"and variation: {variation.Name} is assigned";
                    otherCampaignWinner = true;
                    LogInfoMessage.OtherCampaignSatisfiesWhitelistingStorage(file, groupName, userId, campaign.Key, "whitelisting", disableLogs);
                    break;
                }
                UserStorageMap userStorageMap = _userStorageService != null ? _userStorageService.GetUserMap(campaign.Key, userId, userStorageData) : null;
                if (userStorageMap != null && userStorageMap.VariationName != null)
                {
                    Variation storedVariation = _variationAllocator.GetSavedVariation(campaign, userStorageMap.VariationName.ToString());
                    otherCampaignWinner = true;
                    LogInfoMessage.OtherCampaignSatisfiesWhitelistingStorage(file, groupName, userId, campaign.Key, "user storage", disableLogs);
                    break;
                }
            }
            return otherCampaignWinner;
        }
        private static List<Variation> GetWhiteListedVariationsList(ISegmentEvaluator _segmentEvaluator, string apiName, string userId, BucketedCampaign campaign, string campaignKey, Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, bool disableLogs = false)
        {
            List<Variation> result = new List<Variation> { };
            foreach (var variation in campaign.Variations.All())
            {
                string status = Constants.SegmentationStatus.FAILED;
                string segmentationType = Constants.SegmentationType.WHITELISTING;
                if (variation.Segments.Count == 0)
                {
                    LogDebugMessage.SkippingSegmentation(typeof(IVWOClient).FullName, userId, campaignKey, apiName, variation.Name, disableLogs);
                }
                else
                {
                    if (_segmentEvaluator.evaluate(userId, campaignKey, segmentationType, variation.Segments, variationTargetingVariables))
                    {
                        status = Constants.SegmentationStatus.PASSED;
                        result.Add(variation);
                    }
                    LogDebugMessage.SegmentationStatus(typeof(IVWOClient).FullName, userId, campaignKey, apiName, variation.Name, status, disableLogs);
                }

            }
            return result;
        }
    }
}
