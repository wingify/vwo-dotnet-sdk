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

namespace VWOSdk
{
    internal class CampaignAllocator : ICampaignAllocator
    {
        private static readonly string file = typeof(CampaignAllocator).FullName;
        private readonly IBucketService _userHasher;
        internal CampaignAllocator(IBucketService userHasher)
        {
            this._userHasher = userHasher;
        }

        /// <summary>
        /// Allocate Campaign based on userStorageMap, trafficAllocation by computing userHash for userId and provided CampaignTKey.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userStorageMap"></param>
        /// <param name="campaignKey"></param>
        /// <param name="userId"></param>
        /// <param name="apiName">Api name which called this implementation, Activate/GetVariation/Track. This is for logging purpose.</param>
        /// <returns></returns>
        public BucketedCampaign Allocate(AccountSettings settings, UserStorageMap userStorageMap, string campaignKey, string userId, string apiName = null)
        {
            BucketedCampaign allocatedCampaign = null;
            BucketedCampaign requestedCampaign = settings.Campaigns.Find((campaign) => campaign.Key.Equals(campaignKey));
            if (requestedCampaign != null)
            {
                allocatedCampaign = AllocateCampaign(userId, campaignKey, userStorageMap, requestedCampaign);

                if (allocatedCampaign != null)
                {
                    if (allocatedCampaign.Status.Equals(Constants.Campaign.STATUS_RUNNING, System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        LogInfoMessage.UserEligibilityForCampaign(file, userId, true);
                        return allocatedCampaign;
                    }
                }
            }

            LogErrorMessage.CampaignNotRunning(file, campaignKey, apiName);

            LogInfoMessage.UserEligibilityForCampaign(file, userId, false);
            LogDebugMessage.UserNotPartOfCampaign(file, userId, campaignKey, nameof(Allocate));
            return null;
        }
        /// <summary>
        /// Get Campaign From Settings using campaignKey
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="campaignKey"></param>
        /// <returns></returns>
        public BucketedCampaign GetCampaign(AccountSettings settings, string campaignKey)
        {
            BucketedCampaign requestedCampaign = settings.Campaigns.Find((campaign) => campaign.Key.Equals(campaignKey));
            return requestedCampaign;
        }

        /// <summary>
        /// Allocate Campaign based on userStorageMap, of if userStorageMap is not present based on trafficAllocation.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="campaignKey"></param>
        /// <param name="userStorageMap"></param>
        /// <param name="requestedCampaign"></param>
        /// <returns></returns>
        private BucketedCampaign AllocateCampaign(string userId, string campaignKey, UserStorageMap userStorageMap, BucketedCampaign requestedCampaign)
        {
            BucketedCampaign allocatedCampaign = null;
            LogDebugMessage.CheckUserEligibilityForCampaign(file, campaignKey, requestedCampaign.PercentTraffic, userId);
            if (userStorageMap == null)
            {
                allocatedCampaign = AllocateByTrafficAllocation(userId, requestedCampaign);
            }
            else if (userStorageMap.CampaignKey.Equals(requestedCampaign.Key))
            {
                allocatedCampaign = requestedCampaign;
            }
            return allocatedCampaign;
        }

        /// <summary>
        /// Compute userHash and check for traffic allocation for given campaign.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requestedCampaign"></param>
        /// <returns></returns>
        private BucketedCampaign AllocateByTrafficAllocation(string userId, BucketedCampaign requestedCampaign)
        {
            var selectedCampaign = requestedCampaign;
            var userHash = this._userHasher.ComputeBucketValue(userId, Constants.Campaign.MAX_TRAFFIC_PERCENT, 1);
            if (requestedCampaign.PercentTraffic < userHash)
            {
                selectedCampaign = null;
                LogInfoMessage.AudienceConditionNotMet(file, userId);
            }
            return selectedCampaign;
        }
    }
}
