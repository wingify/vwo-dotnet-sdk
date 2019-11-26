#pragma warning disable 1587
/**
 * Copyright 2019 Wingify Software Pvt. Ltd.
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
    public partial class VWO : IVWOClient
    {
        private readonly UserProfileAdapter _userProfileService;
        private readonly ICampaignAllocator _campaignAllocator;
        private readonly IVariationAllocator _variationAllocator;
        private readonly AccountSettings _settings;
        private readonly IValidator _validator;
        private readonly bool _isDevelopmentMode;

        internal VWO(AccountSettings settings, IValidator validator, IUserProfileService userProfileService, ICampaignAllocator campaignAllocator, IVariationAllocator variationAllocator, bool isDevelopmentMode)
        {
            this._settings = settings;
            this._validator = validator;
            this._userProfileService = new UserProfileAdapter(userProfileService);
            this._campaignAllocator = campaignAllocator;
            this._variationAllocator = variationAllocator;
            this._isDevelopmentMode = isDevelopmentMode;
        }

        #region IVWOClient Methodss

        /// <summary>
        /// Activates a server-side A/B test for a specified user for a server-side running campaign.
        /// </summary>
        /// <param name="campaignTestKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <returns>
        /// The name of the variation in which the user is bucketed, or null if the user doesn't qualify to become a part of the campaign.
        /// </returns>
        public string Activate(string campaignTestKey, string userId)
        {
            if (this._validator.Activate(campaignTestKey, userId))
            {
                var assignedVariation = this.AllocateVariation(campaignTestKey, userId, apiName: nameof(Activate));
                if (assignedVariation.Variation != null)
                {
                    var trackUserRequest = ServerSideVerb.TrackUser(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, this._isDevelopmentMode);
                    trackUserRequest.ExecuteAsync();
                    return assignedVariation.Variation.Name;
                }
            }
            return null;
        }

        /// <summary>
        /// Activates a server-side A/B test for the specified user for a particular running campaign.
        /// </summary>
        /// <param name="campaignTestKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <returns>
        /// The name of the variation in which the user is bucketed, or null if the user doesn't qualify to become a part of the campaign.
        /// </returns>
        public string GetVariation(string campaignTestKey, string userId)
        {
            if (this._validator.GetVariation(campaignTestKey, userId))
            {
                var assignedVariation = this.AllocateVariation(campaignTestKey, userId, apiName: nameof(GetVariation));
                return assignedVariation.Variation?.Name;
            }
            return null;
        }

        /// <summary>
        /// Tracks a conversion event for a particular user for a running server-side campaign.
        /// </summary>
        /// <param name="campaignTestKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="goalIdentifier">The Goal key to uniquely identify a goal of a server-side campaign.</param>
        /// <param name="revenueValue">The Revenue to be tracked for a revenue-type goal.</param>
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public bool Track(string campaignTestKey, string userId, string goalIdentifier, string revenueValue = null)
        {
            if(this._validator.Track(campaignTestKey, userId, goalIdentifier, revenueValue))
            {
                var assignedVariation = this.AllocateVariation(campaignTestKey, userId, goalIdentifier: goalIdentifier, apiName: nameof(Track));
                var variationName = assignedVariation.Variation?.Name;
                var selectedGoalIdentifier = assignedVariation.Goal?.Identifier;
                if (string.IsNullOrEmpty(variationName) == false)
                {
                    if (string.IsNullOrEmpty(selectedGoalIdentifier) == false)
                    {
                        bool sendImpression = true;
                        if (assignedVariation.Goal.IsRevenueType() && string.IsNullOrEmpty(revenueValue))
                        {
                            sendImpression = false;
                            LogErrorMessage.TrackApiRevenueNotPassedForRevenueGoal(file, goalIdentifier, campaignTestKey, userId);
                        }
                        else if (assignedVariation.Goal.IsRevenueType() == false)
                        {
                            revenueValue = null;
                        }

                        if (sendImpression)
                        {
                            var trackGoalRequest = ServerSideVerb.TrackGoal(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, assignedVariation.Goal.Id, revenueValue, this._isDevelopmentMode);
                            trackGoalRequest.ExecuteAsync();
                            return true;
                        }
                    }
                    else
                    {
                        LogErrorMessage.TrackApiGoalNotFound(file, goalIdentifier, campaignTestKey, userId);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Tracks a conversion event for a particular user for a running server-side campaign.
        /// </summary>
        /// <param name="campaignTestKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="goalIdentifier">The Goal key to uniquely identify a goal of a server-side campaign.</param>
        /// <param name="revenueValue">The Revenue to be tracked for a revenue-type goal.</param>
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public bool Track(string campaignTestKey, string userId, string goalIdentifier, int revenueValue)
        {
            return this.Track(campaignTestKey, userId, goalIdentifier, revenueValue.ToString());
        }

        /// <summary>
        /// Tracks a conversion event for a particular user for a running server-side campaign.
        /// </summary>
        /// <param name="campaignTestKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="goalIdentifier">The Goal key to uniquely identify a goal of a server-side campaign.</param>
        /// <param name="revenueValue">The Revenue to be tracked for a revenue-type goal.</param>
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public bool Track(string campaignTestKey, string userId, string goalIdentifier, float revenueValue)
        {
            return this.Track(campaignTestKey, userId, goalIdentifier, revenueValue.ToString());
        }

        #endregion IVWOClient Methods

        #region private Methods
        /// <summary>
        /// Allocate variation by checking UserProfileService, Campaign Traffic Allocation and compute UserHash to check variation allocation by bucketing.
        /// </summary>
        /// <param name="campaignTestKey"></param>
        /// <param name="userId"></param>
        /// <returns>
        /// If Variation is allocated, returns UserAssignedInfo with valid details, else return Empty UserAssignedInfo.
        /// </returns>
        private UserAllocationInfo AllocateVariation(string campaignTestKey, string userId, string apiName = null)
        {
            UserProfileMap userProfileMap = this._userProfileService.GetUserMap(campaignTestKey, userId);
            BucketedCampaign selectedCampaign = this._campaignAllocator.Allocate(this._settings, userProfileMap, campaignTestKey, userId, apiName);
            if (selectedCampaign != null)
            {
                Variation variation = this._variationAllocator.Allocate(userProfileMap, selectedCampaign, userId);
                if (variation != null)
                {
                    LogInfoMessage.VariationAllocated(file, userId, campaignTestKey, variation.Name);
                    LogDebugMessage.GotVariationForUser(file, userId, campaignTestKey, variation.Name, nameof(AllocateVariation));

                    this._userProfileService.SaveUserMap(userId, selectedCampaign.Key, variation.Name);
                    return new UserAllocationInfo(variation, selectedCampaign);
                }
            }

            LogInfoMessage.NoVariationAllocated(file, userId, campaignTestKey);
            return new UserAllocationInfo();
        }

        /// <summary>
        /// If variation is assigned, allocate the goal using goalIdentifier.
        /// </summary>
        /// <param name="campaignTestKey"></param>
        /// <param name="userId"></param>
        /// <param name="goalIdentifier"></param>
        /// <returns>
        /// If Variation is allocated and goal with given identifier is found, return UserAssignedInfo with valid information, otherwise, Empty UserAssignedInfo object.
        /// </returns>
        private UserAllocationInfo AllocateVariation(string campaignTestKey, string userId, string goalIdentifier, string apiName)
        {
            var userAllocationInfo = this.AllocateVariation(campaignTestKey, userId, apiName);
            if (userAllocationInfo.Variation != null)
            {
                if (userAllocationInfo.Campaign.Goals.TryGetValue(goalIdentifier, out Goal goal))
                    userAllocationInfo.Goal = goal;
                else
                    LogErrorMessage.TrackApiGoalNotFound(file, goalIdentifier, campaignTestKey, userId);
            }
            else
                LogErrorMessage.TrackApiVariationNotFound(file, campaignTestKey, userId);
            return userAllocationInfo;
        }

        #endregion private Methods
    }
}
