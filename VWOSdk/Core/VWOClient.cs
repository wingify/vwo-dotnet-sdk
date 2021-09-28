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
using System.Reflection;

namespace VWOSdk
{
    /// <summary>
    ///
    /// </summary>
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
        private readonly BatchEventData _BatchEventData;
        private readonly BatchEventQueue _BatchEventQueue;
        private static readonly string sdkVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //Integration
        private Dictionary<string, dynamic> integrationsMap;      
        private readonly HookManager _HookManager;
        //UsageStats
        private readonly Dictionary<string, int> _usageStats = new Dictionary<string, int>();       
        internal VWO(AccountSettings settings, IValidator validator, IUserStorageService userStorageService,
            ICampaignAllocator campaignAllocator, ISegmentEvaluator segmentEvaluator,
            IVariationAllocator variationAllocator, bool isDevelopmentMode, BatchEventData batchEventData,
            string goalTypeToTrack = Constants.GoalTypes.ALL, bool shouldTrackReturningUser = false, HookManager hookManager = null,
            Dictionary<string, int> usageStats=null)
        {
            this._settings = settings;
            this._validator = validator;
            this._userStorageService = userStorageService != null ? new UserStorageAdapter(userStorageService) : null;
            this._campaignAllocator = campaignAllocator;
            this._variationAllocator = variationAllocator;
            this._isDevelopmentMode = isDevelopmentMode;
            this._segmentEvaluator = segmentEvaluator;
            this._goalTypeToTrack = goalTypeToTrack;
            this._shouldTrackReturningUser = shouldTrackReturningUser;
            this._BatchEventData = batchEventData;
            this._usageStats = usageStats;
            this._BatchEventQueue = batchEventData != null ? new BatchEventQueue(batchEventData, settings.SdkKey, this._settings.AccountId, isDevelopmentMode, this._usageStats) : null;
            this._HookManager = hookManager;
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

            Dictionary<string, dynamic> userStorageData = options.ContainsKey("userStorageData") ? options["userStorageData"] : null;

            bool shouldTrackReturningUser = options.ContainsKey("shouldTrackReturningUser") ? options["shouldTrackReturningUser"] : this._shouldTrackReturningUser;

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

                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables,
                    variationTargetingVariables, apiName: nameof(Activate), userStorageData, shouldTrackReturningUser);
                if (assignedVariation.Variation != null)
                {
                    if (assignedVariation.DuplicateCall && !shouldTrackReturningUser)
                    {
                        LogInfoMessage.UserAlreadyTracked(typeof(IVWOClient).FullName, userId, campaignKey, nameof(Activate));
                        LogDebugMessage.DuplicateCall(typeof(IVWOClient).FullName, nameof(Activate));
                        return assignedVariation.Variation.Name;
                    }

                    if (this._BatchEventData != null)
                    {
                        LogDebugMessage.EventBatchingActivated(typeof(IVWOClient).FullName, nameof(Activate));
                        this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForTrackingUser(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, this._isDevelopmentMode));
                    }
                    else
                    {
                        LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(Activate));
                        var trackUserRequest = ServerSideVerb.TrackUser(this._settings.AccountId,
                            assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, this._isDevelopmentMode, _settings.SdkKey, this._usageStats);
                        trackUserRequest.ExecuteAsync();
                    }

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

            Dictionary<string, dynamic> userStorageData = options.ContainsKey("userStorageData") ? options["userStorageData"] : null;
            // bool shouldTrackReturningUser = options.ContainsKey("shouldTrackReturningUser") ? options["shouldTrackReturningUser"] : this._shouldTrackReturningUser;
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
                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables, variationTargetingVariables,
                apiName: nameof(GetVariation), userStorageData);
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
            Dictionary<string, dynamic> userStorageData = options.ContainsKey("userStorageData") ? options["userStorageData"] : null;
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
                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables,
                         variationTargetingVariables, goalIdentifier: goalIdentifier, apiName: nameof(Track), userStorageData, shouldTrackReturningUser);
                var variationName = assignedVariation.Variation?.Name;
                var selectedGoalIdentifier = assignedVariation.Goal?.Identifier;
                if (string.IsNullOrEmpty(variationName) == false)
                {
                    if (string.IsNullOrEmpty(selectedGoalIdentifier) == false)
                    {
                        if (goalTypeToTrack != assignedVariation.Goal.Type && goalTypeToTrack != Constants.GoalTypes.ALL)
                        {
                            return false;
                        }
                        if (!this.isGoalTriggerRequired(campaignKey, userId, goalIdentifier, variationName, shouldTrackReturningUser, userStorageData))
                        {
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
                            if (this._BatchEventData != null)
                            {
                                LogDebugMessage.EventBatchingActivated(typeof(IVWOClient).FullName, nameof(Track));
                                this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForTrackingGoal(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id,
                                userId, assignedVariation.Goal.Id, revenueValue, this._isDevelopmentMode));
                            }
                            else
                            {
                                LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(Track));
                                var trackGoalRequest = ServerSideVerb.TrackGoal(this._settings.AccountId, assignedVariation.Campaign.Id,
                                    assignedVariation.Variation.Id, userId, assignedVariation.Goal.Id, revenueValue, this._isDevelopmentMode, _settings.SdkKey);
                                trackGoalRequest.ExecuteAsync();
                            }
                            LogDebugMessage.TrackApiGoalFound(file, goalIdentifier, campaignKey, userId);
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
        public Dictionary<string, bool> Track(List<string> campaignKeys, string userId, string goalIdentifier, Dictionary<string, dynamic> options = null)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            foreach (string campaignKey in campaignKeys)
            {
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
            ////  bool shouldTrackReturningUser = options.ContainsKey("shouldTrackReturningUser") ? options["shouldTrackReturningUser"] : this._shouldTrackReturningUser;
            goalTypeToTrack = !string.IsNullOrEmpty(goalTypeToTrack) ? goalTypeToTrack : this._goalTypeToTrack != null ?
                this._goalTypeToTrack : Constants.GoalTypes.ALL;

            Dictionary<string, bool> result = new Dictionary<string, bool>();
            bool campaignFound = false;
            foreach (BucketedCampaign campaign in this._settings.Campaigns)
            {
                foreach (KeyValuePair<string, Goal> goal in campaign.Goals)
                {
                    if (goal.Key != null && goalIdentifier == goal.Value.Identifier &&
                        (goalTypeToTrack == Constants.GoalTypes.ALL || goalTypeToTrack == goal.Value.Type))
                    {
                        campaignFound = true;
                        result[campaign.Key] = this.Track(campaign.Key, userId, goalIdentifier, options);
                    }
                }
            }
            if (!campaignFound)
            {
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
            Dictionary<string, dynamic> userStorageData = options.ContainsKey("userStorageData") ? options["userStorageData"] : null;
            bool shouldTrackReturningUser = options.ContainsKey("shouldTrackReturningUser") ? options["shouldTrackReturningUser"] : this._shouldTrackReturningUser;
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
                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables,
                    variationTargetingVariables, apiName: nameof(IsFeatureEnabled), userStorageData);
                if (assignedVariation.Variation != null)
                {
                    if (assignedVariation.DuplicateCall && !shouldTrackReturningUser)
                    {
                        LogDebugMessage.DuplicateCall(typeof(IVWOClient).FullName, nameof(IsFeatureEnabled));
                        return false;
                    }
                    if (campaign.Type == Constants.CampaignTypes.FEATURE_TEST || campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT)
                    {
                        var result = campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT ? true : assignedVariation.Variation.IsFeatureEnabled;

                        if (result)
                        {
                            if (this._BatchEventData != null)
                            {
                                LogDebugMessage.EventBatchingActivated(typeof(IVWOClient).FullName, nameof(IsFeatureEnabled));
                                this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForTrackingUser(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, this._isDevelopmentMode));
                            }
                            else
                            {
                                LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(IsFeatureEnabled));
                                var trackUserRequest = ServerSideVerb.TrackUser(this._settings.AccountId, assignedVariation.Campaign.Id,
                                    assignedVariation.Variation.Id, userId, this._isDevelopmentMode, _settings.SdkKey,this._usageStats);
                                trackUserRequest.ExecuteAsync();
                            }
                            LogInfoMessage.FeatureEnabledForUser(typeof(IVWOClient).FullName, campaignKey, userId, nameof(IsFeatureEnabled));
                        }
                        else
                        {
                            LogInfoMessage.FeatureNotEnabledForUser(typeof(IVWOClient).FullName, campaignKey, userId, nameof(IsFeatureEnabled));
                        }
                        return result;
                    }

                }
                else
                {
                    LogDebugMessage.VariationNotFound(typeof(IVWOClient).FullName, campaignKey, userId);
                    return false;
                }
                return true;
            }
            else
                return false;
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
        public dynamic GetFeatureVariableValue(string campaignKey, string variableKey, string userId,
            Dictionary<string, dynamic> options = null)
        {
            if (options == null) options = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            var variables = new List<Dictionary<string, dynamic>>();
            var variable = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> userStorageData = options.ContainsKey("userStorageData") ? options["userStorageData"] : null;

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

                var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables, variationTargetingVariables,
                     apiName: nameof(GetFeatureVariableValue), userStorageData);
                if (campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT)
                {
                    variables = campaign.Variables;
                }
                else if (campaign.Type == Constants.CampaignTypes.FEATURE_TEST)
                {
                    if (assignedVariation.Variation != null)
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
                    else
                    {
                        LogDebugMessage.VariationNotFound(typeof(IVWOClient).FullName, campaignKey, userId);
                    }
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
                if (this._BatchEventData != null)
                {

                    LogDebugMessage.EventBatchingActivated(typeof(IVWOClient).FullName, nameof(Push));
                    this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForPushTags(this._settings.AccountId, tagKey, tagValue, userId, this._isDevelopmentMode));

                }
                else
                {
                    LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(Push));
                    var pushRequest = ServerSideVerb.PushTags(this._settings, tagKey, tagValue, userId, this._isDevelopmentMode, _settings.SdkKey);
                    pushRequest.ExecuteAsync();
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Makes a call to our server to flush Events
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server.
        /// False, when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        /// </summary>
        public bool FlushEvents()
        {
            bool response = false;
            if (_BatchEventQueue != null)
            {
                response = _BatchEventQueue.flush(true);

            }
            return response;
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
        /// <param name="userStorageData"></param>
        /// <param name="shouldTrackReturningUser"></param>
        /// <param name="initIntegration"></param>
        /// <returns>
        /// If Variation is allocated, returns UserAssignedInfo with valid details, else return Empty UserAssignedInfo.
        /// </returns>

        private UserAllocationInfo AllocateVariation(string campaignKey, string userId, BucketedCampaign campaign,
        Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, string apiName = null,
        Dictionary<string, dynamic> userStorageData = null, bool shouldTrackReturningUser = false, bool initIntegration = false)
        {
            Variation TargettedVariation = this.FindTargetedVariation(apiName, campaign, campaignKey, userId, customVariables,
                variationTargetingVariables);
            if (TargettedVariation != null)
            {
                if (_HookManager != null)
                {
                    if (!initIntegration)
                    {
                        initIntegrationMap(campaign, apiName, userId, null, customVariables, variationTargetingVariables);
                        LogDebugMessage.InitIntegrationMapForVariation(file, apiName, campaignKey, userId);
                    }
                    executeIntegrationsCallback(false, campaign, TargettedVariation, true);
                    LogDebugMessage.ExecuteIntegrationsCallbackTargettedVariation(file, apiName, campaignKey, userId);
                }

                return new UserAllocationInfo(TargettedVariation, campaign);
            }
            UserStorageMap userStorageMap = this._userStorageService != null ? this._userStorageService.GetUserMap(campaignKey, userId, userStorageData) : null;
            BucketedCampaign selectedCampaign = this._campaignAllocator.Allocate(this._settings, userStorageMap, campaignKey, userId, apiName);
            if (userStorageMap != null && userStorageMap.VariationName != null)
            {
                Variation variation = this._variationAllocator.GetSavedVariation(campaign, userStorageMap.VariationName.ToString());
                LogInfoMessage.GotStoredVariation(file, userStorageMap.VariationName.ToString(), campaignKey, userId);
                if (_HookManager != null)
                {
                    if (!initIntegration)
                    {
                        initIntegrationMap(campaign, apiName, userId, null, customVariables, variationTargetingVariables);
                        LogDebugMessage.InitIntegrationMapForVariation(file, apiName, campaignKey, userId);
                    }
                    executeIntegrationsCallback(true, campaign, variation, false);
                    LogDebugMessage.ExecuteIntegrationsCallbackAlreadyTracked(file, apiName, campaignKey, userId);
                }
                return new UserAllocationInfo(variation, selectedCampaign, true);
            }
            else if (userStorageMap != null && userStorageMap.VariationName == null)
            {
                LogInfoMessage.NoVariationAllocated(file, userId, campaignKey);
                return new UserAllocationInfo();
            }
            else if (userStorageMap == null && this._userStorageService != null && !shouldTrackReturningUser)
            {
                if (!isCampaignActivated(apiName, userId, campaign))
                {
                    return new UserAllocationInfo();
                }
                else
                {
                    LogInfoMessage.NoDataUserStorageService(file, campaignKey, userId);
                }
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
                        if (!this._segmentEvaluator.evaluate(userId, campaignKey, segmentationType, campaign.Segments, customVariables))
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
                    if (this._userStorageService != null)
                    {
                        this._userStorageService.SetUserMap(userId, selectedCampaign.Key, variation.Name);
                    }
                    if (_HookManager != null)
                    {
                        if (!initIntegration)
                        {
                            initIntegrationMap(campaign, apiName, userId, null, customVariables, variationTargetingVariables);
                            LogDebugMessage.InitIntegrationMapForVariation(file, apiName, campaignKey, userId);
                        }
                        executeIntegrationsCallback(false, campaign, variation, false);
                        LogDebugMessage.ExecuteIntegrationsCallbackNewVariation(file, apiName, campaignKey, userId);
                    }
                    return new UserAllocationInfo(variation, selectedCampaign);
                }
                else
                {
                    LogDebugMessage.VariationNotFound(file, campaignKey, userId);
                }
            }
            LogInfoMessage.NoVariationAllocated(file, userId, campaignKey);
            return new UserAllocationInfo();
        }
        private void executeIntegrationsCallback(bool fromUserStorage, Campaign campaign, Variation variation, bool isUserWhitelisted)
        {
            if (variation != null)
            {
                integrationsMap.Add("fromUserStorageService", fromUserStorage);
                integrationsMap.Add("isUserWhitelisted", isUserWhitelisted);
                if (campaign.Type.Equals(Constants.CampaignTypes.FEATURE_ROLLOUT))
                {
                    integrationsMap.Add("isFeatureEnabled", true);
                }
                else
                {
                    if (campaign.Type.Equals(Constants.CampaignTypes.FEATURE_TEST))
                    {
                        integrationsMap.Add("isFeatureEnabled", variation.IsFeatureEnabled);
                    }
                    integrationsMap.Add("variationName", variation.Name);
                    integrationsMap.Add("variationId", variation.Id);
                }
                _HookManager.execute(integrationsMap);
            }
        }

        private void initIntegrationMap(Campaign campaign, String apiName, String userId, String goalIdentifier,
                                       Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables)
        {
            integrationsMap = new Dictionary<string, dynamic>();
            integrationsMap.Add("campaignId", campaign.Id);
            integrationsMap.Add("campaignKey", campaign.Key);
            integrationsMap.Add("campaignName", campaign.Name);
            integrationsMap.Add("campaignType", campaign.Type);
            integrationsMap.Add("customVariables", customVariables == null ? new Dictionary<string, dynamic>() : customVariables);
            integrationsMap.Add("event", Constants.DECISION_TYPES);
            integrationsMap.Add("goalIdentifier", goalIdentifier);
            integrationsMap.Add("isForcedVariationEnabled", campaign.IsForcedVariationEnabled);
            integrationsMap.Add("sdkVersion", sdkVersion);
            integrationsMap.Add("source", apiName);
            integrationsMap.Add("userId", userId);
            integrationsMap.Add("variationTargetingVariables", variationTargetingVariables == null ? new Dictionary<string, dynamic>() : variationTargetingVariables);
            integrationsMap.Add("vwoUserId", BuildQueryParams.Builder.getInstance().withUuid(this._settings.AccountId, userId).u.ToString());
        }
        private bool isCampaignActivated(string apiName, string userId, Campaign campaign)
        {
            if (!apiName.Equals(Constants.CampaignTypes.ACTIVATE, StringComparison.InvariantCultureIgnoreCase)
              && !apiName.Equals(Constants.CampaignTypes.IS_FEATURE_ENABLED, StringComparison.InvariantCultureIgnoreCase))
            {
                LogDebugMessage.CampaignNotActivated(file, apiName, campaign.Key, userId);
                LogInfoMessage.CampaignNotActivated(file, apiName.Equals(Constants.CampaignTypes.TRACK, StringComparison.InvariantCultureIgnoreCase) ? "track it" : "get the decision/value", campaign.Key, userId);
                return false;
            }
            return true;
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
                Variation variation = this._variationAllocator.TargettedVariation(userId, campaign, whiteListedVariations);
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
                string segmentationType = Constants.SegmentationType.WHITELISTING;
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
        /// <param name="userStorageData"></param>
        /// <param name="shouldTrackReturningUser"></param>
        /// <returns>
        /// If Variation is allocated and goal with given identifier is found, return UserAssignedInfo with valid information, otherwise, Empty UserAssignedInfo object.
        /// </returns>

        private UserAllocationInfo AllocateVariation(string campaignKey, string userId, BucketedCampaign campaign,
        Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, string goalIdentifier, string apiName,
        Dictionary<string, dynamic> userStorageData = null, bool shouldTrackReturningUser = false)
        {
            if (_HookManager != null)
            {
                initIntegrationMap(campaign, apiName, userId, goalIdentifier, customVariables, variationTargetingVariables);
                LogDebugMessage.InitIntegrationMapForGoal(file, apiName, campaign.Key, userId);
            }
            var userAllocationInfo = this.AllocateVariation(campaignKey, userId, campaign, customVariables,
                variationTargetingVariables, apiName, userStorageData, shouldTrackReturningUser, true);
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

        private bool isGoalTriggerRequired(string campaignKey, string userId, string goalIdentifier, string variationName, bool shouldTrackReturningUser,
                   Dictionary<string, dynamic> userStorageData = null)
        {
            UserStorageMap userMap = this._userStorageService != null ? this._userStorageService.GetUserMap(campaignKey, userId, userStorageData) : null;

            if (userMap == null) return true;
            string storedGoalIdentifier = null;
            if (userMap.GoalIdentifier != null)
            {
                storedGoalIdentifier = userMap.GoalIdentifier;
                string[] identifiers = storedGoalIdentifier.Split(new string[] { Constants.GOAL_IDENTIFIER_SEPERATOR }, StringSplitOptions.None);
                if (!((IList<string>)identifiers).Contains(goalIdentifier))
                {
                    storedGoalIdentifier = storedGoalIdentifier + Constants.GOAL_IDENTIFIER_SEPERATOR + goalIdentifier;
                }
                else if (!shouldTrackReturningUser)
                {
                    LogInfoMessage.GoalAlreadyTracked(file, userId, campaignKey, goalIdentifier);
                    return false;
                }
            }
            else
            {
                storedGoalIdentifier = goalIdentifier;
            }
            if (this._userStorageService != null)
            {
                this._userStorageService.SetUserMap(userId, campaignKey, variationName, storedGoalIdentifier);
            }

            return true;
        }

        #endregion private Methods

        /// <summary>
        /// Used for Unit Test
        /// </summary>
        /// <returns>
        /// BatchEventQueue Object.
        /// </returns>
        public BatchEventQueue getBatchEventQueue()
        {
            return this._BatchEventQueue;
        }
    }
}
