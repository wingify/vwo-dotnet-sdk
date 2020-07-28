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
    public partial class VWO : IVWOClient
    {
        private readonly UserStorageAdapter _userStorageService;
        private readonly ICampaignAllocator _campaignAllocator;
        private readonly IVariationAllocator _variationAllocator;
        private readonly ISegmentEvaluator _segmentEvaluator;
        private readonly AccountSettings _settings;
        private readonly IValidator _validator;
        private readonly bool _isDevelopmentMode;
        private readonly string _goalTypeToTrack;
        private readonly bool _shouldTrackReturningUser;

        internal VWO(AccountSettings settings, IValidator validator, IUserStorageService userStorageService, ICampaignAllocator campaignAllocator, ISegmentEvaluator segmentEvaluator, IVariationAllocator variationAllocator, bool isDevelopmentMode, string goalTypeToTrack = Constants.GoalTypes.ALL, bool shouldTrackReturningUser = false)
        {
            this._settings = settings;
            this._validator = validator;
            this._userStorageService = new UserStorageAdapter(userStorageService);
            this._campaignAllocator = campaignAllocator;
            this._variationAllocator = variationAllocator;
            this._isDevelopmentMode = isDevelopmentMode;
            this._segmentEvaluator = segmentEvaluator;
            this._goalTypeToTrack = goalTypeToTrack;
            this._shouldTrackReturningUser = shouldTrackReturningUser;
        }

        #region IVWOClient Methods

        /// <summary>
        /// Activates a server-side A/B test for a specified user for a server-side running campaign.
        /// </summary>
        /// <param name="campaignKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="options">Dictionary for passing extra parameters to activate</param>
        /// <returns>
        /// The name of the variation in which the user is bucketed, or null if the user doesn't qualify to become a part of the campaign.
        /// </returns>
        public string Activate(string campaignKey, string userId, Dictionary<string, dynamic> options = null)
        {
            if (options == null) options = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            if (this._validator.Activate(campaignKey, userId, options))
            {
                var campaign = this._campaignAllocator.GetCampaign(this._settings, campaignKey);
                if (campaign == null || campaign.Status != Constants.CampaignStatus.RUNNING)
                {
                    LogErrorMessage.CampaignNotRunning(typeof(IVWOClient).FullName, campaignKey, nameof(Activate));
                    return null;
                }
                if (campaign.Type != Constants.CampaignTypes.VISUAL_AB)
                {
                    LogErrorMessage.InvalidApi(typeof(IVWOClient).FullName, userId, campaignKey, campaign.Type, nameof(Activate));
                    return null;
                }
                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables, variationTargetingVariables, apiName: nameof(Activate));
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
        /// Gets the variation name assigned for the user for the campaign
        /// </summary>
        /// <param name="campaignKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="options">Dictionary for passing extra parameters to activate</param>
        /// <returns>
        /// If variation is assigned then variation name, or Null in case of user not becoming part
        /// </returns>
        public string GetVariation(string campaignKey, string userId, Dictionary<string, dynamic> options = null)
        {
            if (options == null) options = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            if (this._validator.GetVariation(campaignKey, userId, options))
            {
                var campaign = this._campaignAllocator.GetCampaign(this._settings, campaignKey);
                if (campaign == null || campaign.Status != Constants.CampaignStatus.RUNNING)
                {
                    LogErrorMessage.CampaignNotRunning(typeof(IVWOClient).FullName, campaignKey, nameof(GetVariation));
                    return null;
                }

                if (campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT)
                {
                    LogErrorMessage.InvalidApi(typeof(IVWOClient).FullName, userId, campaignKey, campaign.Type, nameof(GetVariation));
                    return null;
                }

                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables, variationTargetingVariables, apiName: nameof(GetVariation));
                if (assignedVariation.Variation != null)
                {
                    return assignedVariation.Variation.Name;
                }
                return assignedVariation.Variation?.Name;
            }
            return null;
        }

        /// For backward compatibility
        public string GetVariationName(string campaignKey, string userId, Dictionary<string, dynamic> options = null)
        {
            return this.GetVariation(campaignKey, userId, options);
        }

        /// <summary>
        /// Tracks a conversion event for a particular user for a running server-side campaign.
        /// </summary>
        /// <param name="campaignKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="goalIdentifier">The Goal key to uniquely identify a goal of a server-side campaign.</param>
        /// <param name="options">Dictionary for passing extra parameters to activate</param>
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public bool Track(string campaignKey, string userId, string goalIdentifier, Dictionary<string, dynamic> options = null)
        {
            if (options == null) options = new Dictionary<string, dynamic>();
            string revenueValue = options.ContainsKey("revenueValue") ? options["revenueValue"].ToString() : null;
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            string goalTypeToTrack = options.ContainsKey("goalTypeToTrack") ? options["goalTypeToTrack"] : null;
            bool shouldTrackReturningUser = options.ContainsKey("shouldTrackReturningUser") ? options["shouldTrackReturningUser"] : this._shouldTrackReturningUser;

            if (this._validator.Track(campaignKey, userId, goalIdentifier, revenueValue, options))
            {
                goalTypeToTrack = !string.IsNullOrEmpty(goalTypeToTrack) ? goalTypeToTrack : this._goalTypeToTrack != null ? this._goalTypeToTrack : Constants.GoalTypes.ALL;
                var campaign = this._campaignAllocator.GetCampaign(this._settings, campaignKey);
                if (campaign == null || campaign.Status != Constants.CampaignStatus.RUNNING)
                {
                    LogErrorMessage.CampaignNotRunning(typeof(IVWOClient).FullName, campaignKey, nameof(Track));
                    return false;
                }

                if (campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT)
                {
                    LogErrorMessage.InvalidApi(typeof(IVWOClient).FullName, userId, campaignKey, campaign.Type, nameof(Track));
                    return false;
                }
                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables, variationTargetingVariables, goalIdentifier: goalIdentifier, apiName: nameof(Track));
                var variationName = assignedVariation.Variation?.Name;
                var selectedGoalIdentifier = assignedVariation.Goal?.Identifier;
                if (string.IsNullOrEmpty(variationName) == false)
                {
                    if (string.IsNullOrEmpty(selectedGoalIdentifier) == false)
                    {
                        if (goalTypeToTrack != assignedVariation.Goal.Type && goalTypeToTrack != Constants.GoalTypes.ALL) {
                            return false;
                        }
                        if (!this.isGoalTriggerRequired(campaignKey, userId, goalIdentifier, variationName, shouldTrackReturningUser)) {
                            return false;
                        }
                        bool sendImpression = true;
                        if (assignedVariation.Goal.IsRevenueType() && string.IsNullOrEmpty(revenueValue))
                        {
                            sendImpression = false;
                            LogErrorMessage.TrackApiRevenueNotPassedForRevenueGoal(file, goalIdentifier, campaignKey, userId);
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
                        LogErrorMessage.TrackApiGoalNotFound(file, goalIdentifier, campaignKey, userId);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Tracks a conversion event for a particular user for a running server-side campaign.
        /// </summary>
        /// <param name="campaignKeys">Campaign keys to uniquely identify list of server-side campaigns.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="goalIdentifier">The Goal key to uniquely identify a goal of a server-side campaign.</param>
        /// <param name="options">Dictionary for passing extra parameters to activate</param>
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public Dictionary<string, bool> Track(List <string> campaignKeys, string userId, string goalIdentifier, Dictionary<string, dynamic> options = null)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (string campaignKey in campaignKeys) {
                result[campaignKey] = this.Track(campaignKey, userId, goalIdentifier, options);
            }
            return result;
        }

        /// <summary>
        /// Tracks a conversion event for a particular user for a running server-side campaign.
        /// </summary>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="goalIdentifier">The Goal key to uniquely identify a goal of a server-side campaign.</param>
        /// <param name="options">Dictionary for passing extra parameters to activate</param>
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public Dictionary<string, bool> Track(string userId, string goalIdentifier, Dictionary<string, dynamic> options = null)
        {
            if (options == null) options = new Dictionary<string, dynamic>();
            string goalTypeToTrack = options.ContainsKey("goalTypeToTrack") ? options["goalTypeToTrack"] : null;
            bool shouldTrackReturningUser = options.ContainsKey("shouldTrackReturningUser") ? options["shouldTrackReturningUser"] : this._shouldTrackReturningUser;
            goalTypeToTrack = !string.IsNullOrEmpty(goalTypeToTrack) ? goalTypeToTrack : this._goalTypeToTrack != null ? this._goalTypeToTrack : Constants.GoalTypes.ALL;

            Dictionary<string, bool> result = new Dictionary<string, bool>();
            bool campaignFound = false;
            foreach (BucketedCampaign campaign in this._settings.Campaigns) {
                foreach(KeyValuePair<string, Goal> goal in campaign.Goals) {
                    if (goal.Key != null && goalIdentifier == goal.Value.Identifier && (goalTypeToTrack == Constants.GoalTypes.ALL || goalTypeToTrack == goal.Value.Type)) {
                        campaignFound = true;
                        result[campaign.Key] = this.Track(campaign.Key, userId, goalIdentifier, options);
                    }
                }
            }
            if (!campaignFound) {
                LogErrorMessage.NoCampaignForGoalFound(file, goalIdentifier);
                return null;
            }
            return result;
        }

        /// <summary>
        /// Identifies whether the user becomes a part of feature rollout/test or not.
        /// </summary>
        /// <param name="campaignKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="options">Dictionary for passing extra parameters to activate</param>
        /// <returns>
        /// /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public bool IsFeatureEnabled(string campaignKey, string userId, Dictionary<string, dynamic> options = null)
        {
            if (options == null) options = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            if (this._validator.IsFeatureEnabled(campaignKey, userId, options))
            {
                var campaign = this._campaignAllocator.GetCampaign(this._settings, campaignKey);
                if (campaign == null || campaign.Status != Constants.CampaignStatus.RUNNING)
                {
                    LogErrorMessage.CampaignNotRunning(typeof(IVWOClient).FullName, campaignKey, nameof(IsFeatureEnabled));
                    return false;
                }
                if (campaign.Type == Constants.CampaignTypes.VISUAL_AB)
                {
                    LogErrorMessage.InvalidApi(typeof(IVWOClient).FullName, userId, campaignKey, campaign.Type, nameof(IsFeatureEnabled));
                    return false;
                }

                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables, variationTargetingVariables, apiName: nameof(IsFeatureEnabled));
                if (campaign.Type == Constants.CampaignTypes.FEATURE_TEST)
                {
                    if (assignedVariation.Variation != null)
                    {
                        var trackUserRequest = ServerSideVerb.TrackUser(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, this._isDevelopmentMode);
                        trackUserRequest.ExecuteAsync();
                        var result = assignedVariation.Variation.IsFeatureEnabled;

                        if (result)
                        {
                            LogInfoMessage.FeatureEnabledForUser(typeof(IVWOClient).FullName, campaignKey, userId, nameof(IsFeatureEnabled));
                        }
                        else
                        {
                            LogInfoMessage.FeatureNotEnabledForUser(typeof(IVWOClient).FullName, campaignKey, userId, nameof(IsFeatureEnabled));
                        }
                        return result;
                    }
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the feature variable corresponding to the variableKey passed. It typecasts the value to the corresponding value type found in settings_file
        /// </summary>
        /// <param name="campaignKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="variableKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <param name="options">Dictionary for passing extra parameters to activate</param>
        /// <returns>
        /// The name of the variation in which the user is bucketed, or null if the user doesn't qualify to become a part of the campaign.
        /// </returns>
        public dynamic GetFeatureVariableValue(string campaignKey, string variableKey, string userId, Dictionary<string, dynamic> options = null)
        {
            if (options == null) options = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            var variables = new List<Dictionary<string, dynamic>>();
            var variable = new Dictionary<string, dynamic>();

            if (this._validator.GetFeatureVariableValue(campaignKey, variableKey, userId, options))
            {
                var campaign = this._campaignAllocator.GetCampaign(this._settings, campaignKey);
                if (campaign == null || campaign.Status != Constants.CampaignStatus.RUNNING)
                {
                    LogErrorMessage.CampaignNotRunning(typeof(IVWOClient).FullName, campaignKey, nameof(GetFeatureVariableValue));
                    return null;
                }

                if (campaign.Type == Constants.CampaignTypes.VISUAL_AB)
                {
                    LogErrorMessage.InvalidApi(typeof(IVWOClient).FullName, userId, campaignKey, campaign.Type, nameof(GetFeatureVariableValue));
                    return null;
                }

                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables, variationTargetingVariables, apiName: nameof(GetFeatureVariableValue));
                if (campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT)
                {
                    variables = campaign.Variables;
                }
                else if (campaign.Type == Constants.CampaignTypes.FEATURE_TEST)
                {
                    if (!assignedVariation.Variation.IsFeatureEnabled)
                    {
                        LogInfoMessage.FeatureNotEnabledForUser(typeof(IVWOClient).FullName, campaignKey, userId, nameof(GetFeatureVariableValue));
                        assignedVariation = this.GetControlVariation(campaign, campaign.Variations.Find(1, (new VariationAllocator()).GetVariationId));
                    }
                    else
                    {
                        LogInfoMessage.FeatureEnabledForUser(typeof(IVWOClient).FullName, campaignKey, userId, nameof(GetFeatureVariableValue));
                    }
                    variables = assignedVariation.Variation.Variables;
                }

                variable = this.GetVariable(variables, variableKey);
                if (variable == null || variable.Count == 0)
                {
                    LogErrorMessage.VariableNotFound(typeof(IVWOClient).FullName, variableKey, campaignKey, campaign.Type, userId, nameof(GetFeatureVariableValue));
                    return null;
                }
                else
                {
                    LogInfoMessage.VariableFound(typeof(IVWOClient).FullName, variableKey, campaignKey, campaign.Type, variable["value"].ToString(), userId, nameof(GetFeatureVariableValue));
                }
                return this._segmentEvaluator.getTypeCastedFeatureValue(variable["value"], variable["type"]);
            }
            return null;
        }

        /// <summary>
        /// Makes a call to our server to store the tagValues
        /// </summary>
        /// <param name="tagKey">key name of the tag</param>
        /// <param name="tagValue">value of the tag</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public bool Push(string tagKey, dynamic tagValue, string userId)
        {
            if (this._validator.Push(tagKey, tagValue, userId))
            {
                if ((int)tagKey.Length > (Constants.PushApi.TAG_KEY_LENGTH))
                {
                    LogErrorMessage.TagKeyLengthExceeded(typeof(IVWOClient).FullName, tagKey, userId, nameof(Push));
                    return false;
                }

                if ((int)tagValue.Length > (Constants.PushApi.TAG_VALUE_LENGTH))
                {
                    LogErrorMessage.TagValueLengthExceeded(typeof(IVWOClient).FullName, tagValue, userId, nameof(Push));
                    return false;
                }
                var pushRequest = ServerSideVerb.PushTags(this._settings, tagKey, tagValue, userId, this._isDevelopmentMode);
                pushRequest.ExecuteAsync();
                return true;
            }
            return false;
        }


        #endregion IVWOClient Methods

        #region private Methods
        /// <summary>
        /// Allocate variation by checking UserStorageService, Campaign Traffic Allocation and compute UserHash to check variation allocation by bucketing.
        /// </summary>
        /// <param name="campaignKey"></param>
        /// <param name="userId"></param>
        /// <param name="apiName"></param>
        /// <param name="campaign"></param>
        /// <param name="customVariables"></param>
        /// <param name="variationTargetingVariables"></param>
        /// <returns>
        /// If Variation is allocated, returns UserAssignedInfo with valid details, else return Empty UserAssignedInfo.
        /// </returns>
        private UserAllocationInfo AllocateVariation(string campaignKey, string userId, BucketedCampaign campaign, Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, string apiName = null)
        {
            Variation TargettedVariation = this.FindTargetedVariation(apiName, campaign, campaignKey, userId, customVariables, variationTargetingVariables);
            if (TargettedVariation != null)
            {
                return new UserAllocationInfo(TargettedVariation, campaign);
            }

            UserStorageMap userStorageMap = this._userStorageService.GetUserMap(campaignKey, userId);
            BucketedCampaign selectedCampaign = this._campaignAllocator.Allocate(this._settings, userStorageMap, campaignKey, userId, apiName);
            if (userStorageMap != null && userStorageMap.VariationName != null)
            {
                Variation variation = this._variationAllocator.GetSavedVariation(campaign, userStorageMap.VariationName.ToString());
                return new UserAllocationInfo(variation, selectedCampaign);
            }
            if (selectedCampaign != null)
            {
                Variation variation = this._variationAllocator.Allocate(userStorageMap, selectedCampaign, userId);
                if (variation != null)
                {
                    if (campaign.Segments.Count > 0)
                    {
                        string segmentationType = Constants.SegmentationType.PRE_SEGMENTATION;
                        if (customVariables == null)
                        {
                            LogInfoMessage.NoCustomVariables(typeof(IVWOClient).FullName, userId, campaignKey, apiName);
                            customVariables = new Dictionary<string, dynamic>();
                        }
                        if (variationTargetingVariables == null)
                        {
                            variationTargetingVariables = new Dictionary<string, dynamic>();
                        }
                        if (!this._segmentEvaluator.evaluate(userId, campaignKey, segmentationType, campaign.Segments, customVariables ))
                        {
                            return new UserAllocationInfo();
                        }
                    }
                    else
                    {
                        LogInfoMessage.SkippingPreSegmentation(typeof(IVWOClient).FullName, userId, campaignKey, apiName);
                    }
                    LogInfoMessage.VariationAllocated(file, userId, campaignKey, variation.Name);
                    LogDebugMessage.GotVariationForUser(file, userId, campaignKey, variation.Name, nameof(AllocateVariation));

                    this._userStorageService.SetUserMap(userId, selectedCampaign.Key, variation.Name);
                    return new UserAllocationInfo(variation, selectedCampaign);
                }
            }

            LogInfoMessage.NoVariationAllocated(file, userId, campaignKey);
            return new UserAllocationInfo();
        }

        private Variation FindTargetedVariation(string apiName, BucketedCampaign campaign, string campaignKey, string userId, Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables)
        {
            if (campaign.IsForcedVariationEnabled)
            {
                if (variationTargetingVariables == null)
                {
                    variationTargetingVariables = new Dictionary<string, dynamic>();
                    variationTargetingVariables.Add("_vwoUserId", userId);
                }
                else
                {
                    if (variationTargetingVariables.ContainsKey("_vwoUserId"))
                    {
                        variationTargetingVariables["_vwoUserId"] = userId;
                    }
                    else
                    {
                        variationTargetingVariables.Add("_vwoUserId", userId);
                    }
                }
                List<Variation> whiteListedVariations = this.GetWhiteListedVariationsList(apiName, userId, campaign, campaignKey, customVariables, variationTargetingVariables);

                string status = Constants.WhitelistingStatus.FAILED;
                string variationString = " ";
                Variation variation = this._variationAllocator.TargettedVariation(userId, whiteListedVariations);
                if (variation != null)
                {
                    status = Constants.WhitelistingStatus.PASSED;
                    variationString = $"and variation: {variation.Name} is assigned";
                }
                LogInfoMessage.WhitelistingStatus(typeof(IVWOClient).FullName, userId, campaignKey, apiName, variationString, status);
                return variation;
            }
            LogInfoMessage.SkippingWhitelisting(typeof(IVWOClient).FullName, userId, campaignKey, apiName);
            return null;
        }

        private List<Variation> GetWhiteListedVariationsList(string apiName, string userId, BucketedCampaign campaign, string campaignKey, Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables)
        {
            List<Variation> result = new List<Variation> { };
            foreach (var variation in campaign.Variations.All())
            {
                string status = Constants.SegmentationStatus.FAILED;
                string segmentationType  = Constants.SegmentationType.WHITELISTING;
                if (variation.Segments.Count == 0)
                {
                    LogDebugMessage.SkippingSegmentation(typeof(IVWOClient).FullName, userId, campaignKey, apiName, variation.Name);
                }
                else
                {
                    if (this._segmentEvaluator.evaluate(userId, campaignKey, segmentationType, variation.Segments, variationTargetingVariables))
                    {
                        status = Constants.SegmentationStatus.PASSED;
                        result.Add(variation);
                    }
                    LogDebugMessage.SegmentationStatus(typeof(IVWOClient).FullName, userId, campaignKey, apiName, variation.Name, status);
                }

            }
            return result;
        }
        private UserAllocationInfo GetControlVariation(BucketedCampaign campaign, Variation variation)
        {
            return new UserAllocationInfo(variation, campaign);
        }

        private Dictionary<string, dynamic> GetVariable(List<Dictionary<string, dynamic>> Variables, string VariableKey)
        {
            Dictionary<string, dynamic> matchingVariable = Variables.Find(variable => variable.ContainsKey("key") && variable["key"] == VariableKey);
            return matchingVariable;
        }

        /// <summary>
        /// If variation is assigned, allocate the goal using goalIdentifier.
        /// </summary>
        /// <param name="campaignKey"></param>
        /// <param name="userId"></param>
        /// <param name="goalIdentifier"></param>
        /// <param name="campaign"></param>
        /// <param name="customVariables"></param>
        /// <param name="variationTargetingVariables"></param>
        /// <param name="apiName"></param>
        /// <returns>
        /// If Variation is allocated and goal with given identifier is found, return UserAssignedInfo with valid information, otherwise, Empty UserAssignedInfo object.
        /// </returns>
        private UserAllocationInfo AllocateVariation(string campaignKey, string userId, BucketedCampaign campaign, Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, string goalIdentifier, string apiName)
        {
            var userAllocationInfo = this.AllocateVariation(campaignKey, userId, campaign, customVariables, variationTargetingVariables, apiName);
            if (userAllocationInfo.Variation != null)
            {
                if (userAllocationInfo.Campaign.Goals.TryGetValue(goalIdentifier, out Goal goal))
                    userAllocationInfo.Goal = goal;
                else
                    LogErrorMessage.TrackApiGoalNotFound(file, goalIdentifier, campaignKey, userId);
            }
            else
            {
                LogErrorMessage.TrackApiVariationNotFound(file, campaignKey, userId);
            }
            return userAllocationInfo;
        }

        private bool isGoalTriggerRequired(string campaignKey, string userId, string goalIdentifier, string variationName, bool shouldTrackReturningUser)
        {
            UserStorageMap userMap = this._userStorageService.GetUserMap(campaignKey, userId);
            if (userMap == null) return true;
            string storedGoalIdentifier = null;
            if (userMap.GoalIdentifier != null) {
                storedGoalIdentifier = userMap.GoalIdentifier;
                string[] identifiers = storedGoalIdentifier.Split(new string[] { Constants.GOAL_IDENTIFIER_SEPERATOR }, StringSplitOptions.None);
                if (!((IList<string>)identifiers).Contains(goalIdentifier)) {
                    storedGoalIdentifier = storedGoalIdentifier + Constants.GOAL_IDENTIFIER_SEPERATOR + goalIdentifier;
                } else if (!shouldTrackReturningUser) {
                    LogInfoMessage.GoalAlreadyTracked(file, userId, campaignKey, goalIdentifier);
                    return false;
                }
            } else {
                storedGoalIdentifier = goalIdentifier;
            }
            this._userStorageService.SetUserMap(userId, campaignKey, variationName, storedGoalIdentifier);
            return true;
        }

        #endregion private Methods
    }
}
