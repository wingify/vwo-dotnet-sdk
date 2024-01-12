
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
using System.Text;

namespace VWOSdk
{
    /// <summary>
    ///
    /// </summary>
    public partial class VWO : IVWOClient
    {
        internal static readonly int RandomAlgo = 1;
        private readonly UserStorageAdapter _userStorageService;
        private readonly ICampaignAllocator _campaignAllocator;
        private readonly IVariationAllocator _variationAllocator;
        private readonly ISegmentEvaluator _segmentEvaluator;
        private readonly AccountSettings _settings;
        private readonly IValidator _validator;
        private readonly bool _isDevelopmentMode;
        private readonly string _goalTypeToTrack;
        private readonly BatchEventData _BatchEventData;
        private readonly BatchEventQueue _BatchEventQueue;
        private readonly RedisConfig _redisConfig;
        private static readonly string sdkVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //Integration
        private Dictionary<string, dynamic> integrationsMap;
        private readonly HookManager _HookManager;
        private readonly Dictionary<string, int> _usageStats = new Dictionary<string, int>();
        internal VWO(AccountSettings settings, IValidator validator, IUserStorageService userStorageService,
            ICampaignAllocator campaignAllocator, ISegmentEvaluator segmentEvaluator,
            IVariationAllocator variationAllocator, bool isDevelopmentMode, BatchEventData batchEventData,
            string goalTypeToTrack = Constants.GoalTypes.ALL, HookManager hookManager = null, RedisConfig redisConfig = null,
            Dictionary<string, int> usageStats = null)
        {
            this._settings = settings;
            this._validator = validator;
            this._redisConfig = redisConfig;
            //this._userStorageService = userStorageService != null ? new UserStorageAdapter(userStorageService) : null;
            this._userStorageService = userStorageService != null ? new UserStorageAdapter(userStorageService) : ( redisConfig != null ? new UserStorageAdapter(new RedisUserStorageService (redisConfig)) : null );
            
            this._campaignAllocator = campaignAllocator;
            this._variationAllocator = variationAllocator;
            this._isDevelopmentMode = isDevelopmentMode;
            this._segmentEvaluator = segmentEvaluator;
            this._goalTypeToTrack = goalTypeToTrack;
            this._BatchEventData = batchEventData;
            this._usageStats = usageStats;
            this._BatchEventQueue = batchEventData != null ? new BatchEventQueue(settings, batchEventData, settings.SdkKey, this._settings.AccountId, isDevelopmentMode, this._usageStats) : null;
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
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            Dictionary<string, dynamic> metaData = options.ContainsKey("metaData") && options["metaData"] is Dictionary<string, dynamic> ? options["metaData"] : null;
            string visitorUserAgent = options.ContainsKey("userAgent") ? options["userAgent"] : null;
            string userIpAddress = options.ContainsKey("userIpAddress") ? options["userIpAddress"] : null;
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
                    variationTargetingVariables, apiName: nameof(Activate), userStorageData, metaData);
                if (assignedVariation.Variation != null)
                {
                    if (assignedVariation.DuplicateCall)
                    {
                        LogInfoMessage.UserAlreadyTracked(typeof(IVWOClient).FullName, userId, campaignKey, nameof(Activate));
                        LogDebugMessage.DuplicateCall(typeof(IVWOClient).FullName, nameof(Activate));
                        return assignedVariation.Variation.Name;
                    }

                    if (this._BatchEventData != null)
                    {
                        LogDebugMessage.EventBatchingActivated(typeof(IVWOClient).FullName, nameof(Activate));
                        this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForTrackingUser(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, this._isDevelopmentMode, visitorUserAgent, userIpAddress));
                    }
                    else
                    {

                        LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(Activate));
                        if (_settings.IsEventArchEnabled)
                        {
                            LogDebugMessage.ActivatedEventArchEnabled(typeof(IVWOClient).FullName, nameof(Activate));
                            var response = ServerSideVerb.TrackUserArchEnabled(this._settings, this._settings.AccountId, assignedVariation.Campaign.Id,
                           assignedVariation.Variation.Id, userId, this._isDevelopmentMode, _settings.SdkKey, this._usageStats, visitorUserAgent, userIpAddress);
                        }
                        else
                        {
                            var trackUserRequest = ServerSideVerb.TrackUser(this._settings, this._settings.AccountId,
                            assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, this._isDevelopmentMode, _settings.SdkKey, this._usageStats, visitorUserAgent, userIpAddress);
                            trackUserRequest.ExecuteAsync();
                        }


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
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            Dictionary<string, dynamic> metaData = options.ContainsKey("metaData") && options["metaData"] is Dictionary<string, dynamic> ? options["metaData"] : null;
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
                apiName: nameof(GetVariation), userStorageData, metaData);
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
            Dictionary<string, dynamic> metaData = options.ContainsKey("metaData") && options["metaData"] is Dictionary<string, dynamic> ? options["metaData"] : null;
            Dictionary<string,dynamic> eventProperties = options.ContainsKey("eventProperties") && options["eventProperties"] is Dictionary<string, dynamic> ? options["eventProperties"] : null;
            Dictionary<string, int> metricMap = new Dictionary<string, int>();
            List<string> revenuePropList = new List<string>();
            string visitorUserAgent = options.ContainsKey("userAgent") ? options["userAgent"] : null;
            string userIpAddress = options.ContainsKey("userIpAddress") ? options["userIpAddress"] : null;
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
                         variationTargetingVariables, goalIdentifier: goalIdentifier, apiName: nameof(Track), userStorageData, metaData);
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
                        if (!this.isGoalTriggerRequired(assignedVariation, campaignKey, userId, goalIdentifier, variationName, userStorageData, metaData))
                        {
                            return false;
                        }
                        bool sendImpression = true;
                        if (assignedVariation.Goal.IsRevenueType() && string.IsNullOrEmpty(revenueValue))
                        {
                            // mca implementation
                                if(this._settings.IsEventArchEnabled){
                                /* 
                                If it's a metric of type - value of an event property and calculation logic is first Value (mca != -1)
                                */
                                    if(assignedVariation.Goal.mca != -1) {
                                        /*
                                        In this case it is expected that goal will have revenueProp
                                        Error should be logged if eventProperties is not Defined ` OR ` eventProperties does not have revenueProp key
                                        */
                                        if(eventProperties == null || !eventProperties.ContainsKey(assignedVariation.Goal.GetRevenueProp())) {
                                            sendImpression = false;
                                            LogErrorMessage.TrackApiRevenueNotPassedForRevenueGoal(file, goalIdentifier, campaignKey, userId);
                                        }
                                    }
                                    else {
                                    /*
                                    here mca == -1 so there could only be 2 scenarios, 
                                    1. If revenueProp is defined then eventProperties should have revenueProp key
                                    2. if revenueProp is not defined then it's a metric of type - Number of times an event has been triggered.
                                    */
                                    if (assignedVariation.Goal.GetRevenueProp() != null) {
                                        if (!string.IsNullOrEmpty(assignedVariation.Goal.GetRevenueProp())) {
                                            // Error should be logged if eventProperties is not defined OR eventProperties does not have revenueProp key.
                                            if (eventProperties == null || !eventProperties.ContainsKey(assignedVariation.Goal.GetRevenueProp()))
                                            {
                                                sendImpression = false;
                                                LogErrorMessage.TrackApiRevenueNotPassedForRevenueGoal(file, goalIdentifier, campaignKey, userId);
                                            }
                                        }
                                     }
                                    }
                                } else {
                                    sendImpression = false;
                                    LogErrorMessage.TrackApiRevenueNotPassedForRevenueGoal(file, goalIdentifier, campaignKey, userId);
                                }
                        }
                        else if (assignedVariation.Goal.IsRevenueType() && !string.IsNullOrEmpty(assignedVariation.Goal.GetRevenueProp()))
                        {
                            revenuePropList.Add(assignedVariation.Goal.GetRevenueProp());
                            LogDebugMessage.TrackApiRevenuePropFoundForRevenueGoal(file, goalIdentifier, campaignKey, userId);
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
                                if(_settings.IsEventArchEnabled && assignedVariation.Goal.GetRevenueProp()!= null && eventProperties!= null && eventProperties.ContainsKey(assignedVariation.Goal.GetRevenueProp())){
                                    revenueValue = eventProperties[assignedVariation.Goal.GetRevenueProp()].ToString();
                                }
                                this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForTrackingGoal(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id,
                                userId, assignedVariation.Goal.Id, revenueValue, this._isDevelopmentMode, visitorUserAgent, userIpAddress));
                            }
                            else
                            {
                                LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(Track));
                                if (_settings.IsEventArchEnabled)
                                {
                                    LogDebugMessage.ActivatedEventArchEnabled(typeof(IVWOClient).FullName, nameof(Track));
                                    metricMap.Add(assignedVariation.Campaign.Id.ToString(), assignedVariation.Goal.Id);
                                    var response = ServerSideVerb.TrackGoalArchEnabled(this._settings, this._settings.AccountId, goalIdentifier, userId, revenueValue,
                                    this._isDevelopmentMode, _settings.SdkKey, metricMap, revenuePropList, eventProperties, visitorUserAgent, userIpAddress);
                                }
                                else
                                {
                                    var trackGoalRequest = ServerSideVerb.TrackGoal(this._settings, this._settings.AccountId, assignedVariation.Campaign.Id,
                                    assignedVariation.Variation.Id, userId, assignedVariation.Goal.Id, revenueValue, this._isDevelopmentMode, _settings.SdkKey, visitorUserAgent, userIpAddress);
                                    trackGoalRequest.ExecuteAsync();
                                }

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
        ///<param name="campaignKeys">Campaign keys to uniquely identify list of server-side campaigns.</param>
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
            if (options == null) options = new Dictionary<string, dynamic>();
            string revenueValue = options.ContainsKey("revenueValue") ? options["revenueValue"].ToString() : null;
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            string goalTypeToTrack = options.ContainsKey("goalTypeToTrack") ? options["goalTypeToTrack"] : null;
            Dictionary<string, dynamic> userStorageData = options.ContainsKey("userStorageData") ? options["userStorageData"] : null;
            Dictionary<string, dynamic> metaData = options.ContainsKey("metaData") && options["metaData"] is Dictionary<string, dynamic> ? options["metaData"] : null;
            Dictionary<string,dynamic> eventProperties = options.ContainsKey("eventProperties") && options["eventProperties"] is Dictionary<string, dynamic> ? options["eventProperties"] : null;
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            Dictionary<string, int> metricMap = new Dictionary<string, int>();
            List<string> revenuePropList = new List<string>();
            string visitorUserAgent = options.ContainsKey("userAgent") ? options["userAgent"] : null;
            string userIpAddress = options.ContainsKey("userIpAddress") ? options["userIpAddress"] : null;
            foreach (string campaignKey in campaignKeys)
            {
                if (this._validator.Track(campaignKey, userId, goalIdentifier, revenueValue, options))
                {
                    bool sendImpression = true;
                    goalTypeToTrack = !string.IsNullOrEmpty(goalTypeToTrack) ? goalTypeToTrack : this._goalTypeToTrack != null ? this._goalTypeToTrack : Constants.GoalTypes.ALL;
                    var campaign = this._campaignAllocator.GetCampaign(this._settings, campaignKey);
                    if (campaign == null || campaign.Status != Constants.CampaignStatus.RUNNING)
                    {
                        LogErrorMessage.CampaignNotRunning(typeof(IVWOClient).FullName, campaignKey, nameof(Track));
                        sendImpression = false;
                        result[campaignKey] = false;
                    }

                    if (campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT)
                    {
                        LogErrorMessage.InvalidApi(typeof(IVWOClient).FullName, userId, campaignKey, campaign.Type, nameof(Track));
                        sendImpression = false;
                        result[campaignKey] = false;
                    }
                    var assignedVariation = this.AllocateVariation(campaignKey, userId, campaign, customVariables,
                             variationTargetingVariables, goalIdentifier: goalIdentifier, apiName: nameof(Track), userStorageData, metaData);
                    var variationName = assignedVariation.Variation?.Name;
                    var selectedGoalIdentifier = assignedVariation.Goal?.Identifier;
                    if (string.IsNullOrEmpty(variationName) == false)
                    {
                        if (string.IsNullOrEmpty(selectedGoalIdentifier) == false)
                        {
                            if (goalTypeToTrack != assignedVariation.Goal.Type && goalTypeToTrack != Constants.GoalTypes.ALL)
                            {
                                sendImpression = false;
                                result[campaignKey] = false;
                            }
                            if (!this.isGoalTriggerRequired(assignedVariation , campaignKey, userId, goalIdentifier, variationName, userStorageData, metaData))
                            {
                                sendImpression = false;
                                result[campaignKey] = false;
                            }

                            if (assignedVariation.Goal.IsRevenueType() && string.IsNullOrEmpty(revenueValue))
                            {
                                // mca implementation
                                if(this._settings.IsEventArchEnabled){
                                /* 
                                If it's a metric of type - value of an event property and calculation logic is first Value (mca != -1)
                                */
                                    if(assignedVariation.Goal.mca != -1) {
                                        /*
                                        In this case it is expected that goal will have revenueProp
                                        Error should be logged if eventProperties is not Defined ` OR ` eventProperties does not have revenueProp key
                                        */

                                        if(eventProperties == null || !eventProperties.ContainsKey(assignedVariation.Goal.GetRevenueProp())) {
                                            sendImpression = false;
                                            result[campaignKey] = false;
                                            LogErrorMessage.TrackApiRevenueNotPassedForRevenueGoal(file, goalIdentifier, campaignKey, userId);
                                        }
                                    }
                                    else {
                                    /*
                                    here mca == -1 so there could only be 2 scenarios, 
                                    1. If revenueProp is defined then eventProperties should have revenueProp key
                                    2. if revenueProp is not defined then it's a metric of type - Number of times an event has been triggered.
                                    */
                                    if (assignedVariation.Goal.GetRevenueProp() != null) {
                                        if (!string.IsNullOrEmpty(assignedVariation.Goal.GetRevenueProp())) {
                                            // Error should be logged if eventProperties is not defined OR eventProperties does not have revenueProp key.
                                            if (eventProperties == null || !eventProperties.ContainsKey(assignedVariation.Goal.GetRevenueProp()))
                                            {
                                                sendImpression = false;
                                                result[campaignKey] = false;
                                                LogErrorMessage.TrackApiRevenueNotPassedForRevenueGoal(file, goalIdentifier, campaignKey, userId);
                                            }
                                        }
                                     }
                                    }
                                } else {
                                    sendImpression = false;
                                    result[campaignKey] = false;
                                    LogErrorMessage.TrackApiRevenueNotPassedForRevenueGoal(file, goalIdentifier, campaignKey, userId);
                                }
                            }
                            else if (assignedVariation.Goal.IsRevenueType() && !string.IsNullOrEmpty(assignedVariation.Goal.GetRevenueProp()))
                            {
                                revenuePropList.Add(assignedVariation.Goal.GetRevenueProp());
                                LogDebugMessage.ActivatedEventArchEnabled(typeof(IVWOClient).FullName, nameof(Track));
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
                                    if(_settings.IsEventArchEnabled && assignedVariation.Goal.GetRevenueProp()!= null && eventProperties!= null && eventProperties.ContainsKey(assignedVariation.Goal.GetRevenueProp())){
                                        revenueValue = eventProperties[assignedVariation.Goal.GetRevenueProp()];
                                    }
                                    this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForTrackingGoal(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id,
                                    userId, assignedVariation.Goal.Id, revenueValue, this._isDevelopmentMode, visitorUserAgent, userIpAddress));
                                }
                                else
                                {
                                    LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(Track));
                                    if (_settings.IsEventArchEnabled)
                                    {
                                        if (!metricMap.TryGetValue(assignedVariation.Campaign.Id.ToString(), out int hasVal))
                                        {
                                            metricMap.Add(assignedVariation.Campaign.Id.ToString(), assignedVariation.Goal.Id);
                                        }

                                    }
                                    else if (!this._isDevelopmentMode)
                                    {
                                        var trackGoalRequest = ServerSideVerb.TrackGoal(this._settings, this._settings.AccountId, assignedVariation.Campaign.Id,
                                        assignedVariation.Variation.Id, userId, assignedVariation.Goal.Id, revenueValue, this._isDevelopmentMode, _settings.SdkKey, visitorUserAgent, userIpAddress);
                                        trackGoalRequest.ExecuteAsync();
                                    }
                                }
                                LogDebugMessage.TrackApiGoalFound(file, goalIdentifier, campaignKey, userId);
                                result[campaignKey] = true;
                            }
                        }
                        else
                        {
                            LogErrorMessage.TrackApiGoalNotFound(file, goalIdentifier, campaignKey, userId);
                        }
                    }
                }
            }
            if (_settings.IsEventArchEnabled && metricMap.Count > 0)
            {
                var response = ServerSideVerb.TrackGoalArchEnabled(this._settings, this._settings.AccountId,
                goalIdentifier, userId, revenueValue, this._isDevelopmentMode, _settings.SdkKey, metricMap, revenuePropList, eventProperties, visitorUserAgent, userIpAddress);
                LogDebugMessage.ActivatedEventArchEnabled(typeof(IVWOClient).FullName, nameof(Track));
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
            goalTypeToTrack = !string.IsNullOrEmpty(goalTypeToTrack) ? goalTypeToTrack : this._goalTypeToTrack != null ?
                this._goalTypeToTrack : Constants.GoalTypes.ALL;
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            bool campaignFound = false;
            List<string> campaignKeys = new List<string>();
            foreach (BucketedCampaign campaign in this._settings.Campaigns)
            {
                foreach (KeyValuePair<string, Goal> goal in campaign.Goals)
                {
                    if (goal.Key != null && goalIdentifier == goal.Value.Identifier &&
                        (goalTypeToTrack == Constants.GoalTypes.ALL || goalTypeToTrack == goal.Value.Type))
                    {
                        campaignFound = true;
                        campaignKeys.Add(campaign.Key);
                    }
                }
            }
            result = this.Track(campaignKeys, userId, goalIdentifier, options);
            if (!campaignFound && result.Count == 0)
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
            Dictionary<string, dynamic> customVariables = options.ContainsKey("customVariables") ? options["customVariables"] : null;
            Dictionary<string, dynamic> variationTargetingVariables = options.ContainsKey("variationTargetingVariables") ? options["variationTargetingVariables"] : null;
            Dictionary<string, dynamic> metaData = options.ContainsKey("metaData") && options["metaData"] is Dictionary<string, dynamic> ? options["metaData"] : null;
            string visitorUserAgent = options.ContainsKey("userAgent") ? options["userAgent"] : null;
            string userIpAddress = options.ContainsKey("userIpAddress") ? options["userIpAddress"] : null;
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
                    variationTargetingVariables, apiName: nameof(IsFeatureEnabled), userStorageData, metaData);
                if (assignedVariation.Variation != null)
                {
                    if (assignedVariation.DuplicateCall)
                    {
                        LogInfoMessage.UserAlreadyTracked(typeof(IVWOClient).FullName, userId, campaignKey, nameof(IsFeatureEnabled));
                        LogDebugMessage.DuplicateCall(typeof(IVWOClient).FullName, nameof(IsFeatureEnabled));
                        return true;
                    }
                    if (campaign.Type == Constants.CampaignTypes.FEATURE_TEST || campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT)
                    {
                        var result = campaign.Type == Constants.CampaignTypes.FEATURE_ROLLOUT ? true : assignedVariation.Variation.IsFeatureEnabled;

                        if (this._BatchEventData != null)
                        {
                            LogDebugMessage.EventBatchingActivated(typeof(IVWOClient).FullName, nameof(IsFeatureEnabled));
                            this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForTrackingUser(this._settings.AccountId, assignedVariation.Campaign.Id, assignedVariation.Variation.Id, userId, this._isDevelopmentMode, visitorUserAgent, userIpAddress));
                        }
                        else
                        {
                            LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(IsFeatureEnabled));
                            if (_settings.IsEventArchEnabled)
                            {
                                LogDebugMessage.ActivatedEventArchEnabled(typeof(IVWOClient).FullName, nameof(IsFeatureEnabled));
                                var response = ServerSideVerb.TrackUserArchEnabled(this._settings, this._settings.AccountId, assignedVariation.Campaign.Id,
                                assignedVariation.Variation.Id, userId, this._isDevelopmentMode, _settings.SdkKey, this._usageStats, visitorUserAgent, userIpAddress);
                            }
                            else
                            {
                                var trackUserRequest = ServerSideVerb.TrackUser(this._settings, this._settings.AccountId, assignedVariation.Campaign.Id,
                                    assignedVariation.Variation.Id, userId, this._isDevelopmentMode, _settings.SdkKey, this._usageStats, visitorUserAgent, userIpAddress);
                                trackUserRequest.ExecuteAsync();
                            }
                        }

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
                }
                else
                {
                    LogDebugMessage.VariationNotFound(typeof(IVWOClient).FullName, campaignKey, userId);
                    return false;
                }
                return false;
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
                    if (_settings.IsEventArchEnabled)
                    {
                        LogDebugMessage.ActivatedEventArchEnabled(typeof(IVWOClient).FullName, nameof(Push));
                        Dictionary<string, string> customDimensionMap = new Dictionary<string, string>();
                        customDimensionMap.Add(tagKey, tagValue);
                        var response = ServerSideVerb.PushTagsArchEnabled(this._settings, customDimensionMap, userId, this._isDevelopmentMode, _settings.SdkKey);
                    }
                    else
                    {
                        var pushRequest = ServerSideVerb.PushTags(this._settings, tagKey, tagValue, userId, this._isDevelopmentMode, _settings.SdkKey);
                        pushRequest.ExecuteAsync();
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Makes a call to our server to store the tagValues
        /// </summary>
        /// <param name="customDimensionMap">Dictionary for passing key tag pairs</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <returns>
        /// A boolean value based on whether the impression was made to the VWO server.
        /// True, if an impression event is successfully being made to the VWO server for report generation.
        /// False, If userId provided is not part of campaign or when unexpected error comes and no impression call is made to the VWO server.
        /// </returns>
        public bool Push(Dictionary<string, string> customDimensionMap, string userId)
        {
            if (this._validator.Push(customDimensionMap, userId))
            {
                foreach (var item in customDimensionMap)
                {
                    if ((item.Key.Equals(" ") && item.Value.Equals(" ")))
                    {
                        LogErrorMessage.TagKeyValueInvalid(typeof(IVWOClient).FullName, item.Key, item.Value, userId, nameof(Push));
                        return false;
                    }
                    if ((int)item.Key.Length > (Constants.PushApi.TAG_KEY_LENGTH))
                    {
                        LogErrorMessage.TagKeyLengthExceeded(typeof(IVWOClient).FullName, item.Key, userId, nameof(Push));
                        return false;
                    }
                    if ((int)item.Value.Length > (Constants.PushApi.TAG_VALUE_LENGTH))
                    {
                        LogErrorMessage.TagValueLengthExceeded(typeof(IVWOClient).FullName, item.Value, userId, nameof(Push));
                        return false;
                    }
                }
                if (this._BatchEventData != null)
                {
                    foreach (var item in customDimensionMap)
                    {
                        LogDebugMessage.EventBatchingActivated(typeof(IVWOClient).FullName, nameof(Push));
                        this._BatchEventQueue.addInQueue(HttpRequestBuilder.EventForPushTags(this._settings.AccountId, item.Key, item.Value, userId, this._isDevelopmentMode));
                    }
                }
                else
                {
                    LogDebugMessage.EventBatchingNotActivated(typeof(IVWOClient).FullName, nameof(Push));
                    if (_settings.IsEventArchEnabled)
                    {
                        LogDebugMessage.ActivatedEventArchEnabled(typeof(IVWOClient).FullName, nameof(Push));
                        var response = ServerSideVerb.PushTagsArchEnabled(this._settings, customDimensionMap, userId, this._isDevelopmentMode, _settings.SdkKey);
                    }
                    else
                    {
                        foreach (var item in customDimensionMap)
                        {
                            var pushRequest = ServerSideVerb.PushTags(this._settings, item.Key, item.Value, userId, this._isDevelopmentMode, _settings.SdkKey);
                            pushRequest.ExecuteAsync();
                        }

                    }
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
        /// <param name="metaData"></param>
        /// <param name="initIntegration"></param>
        /// <returns>
        /// If Variation is allocated, returns UserAssignedInfo with valid details, else return Empty UserAssignedInfo.
        /// </returns>
        private UserAllocationInfo AllocateVariation(string campaignKey, string userId, BucketedCampaign campaign,
        Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, string apiName = null,
        Dictionary<string, dynamic> userStorageData = null, Dictionary<string, dynamic> metaData = null, bool initIntegration = false)
        {
            Dictionary<string, dynamic> integrationsMap = new Dictionary<string, dynamic>();
            dynamic groupId = 0, groupName = "";
            Variation TargettedVariation = this.FindTargetedVariation(apiName, campaign, campaignKey, userId, customVariables, variationTargetingVariables, this._settings.isNB, this._settings.isNBv2);
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
            IDictionary<string, dynamic> groupDetails = CampaignHelper.isPartOfGroup(this._settings, campaign.Id);
            if (groupDetails.ContainsKey("groupId"))
            {
                groupDetails.TryGetValue("groupId", out groupId);
                groupDetails.TryGetValue("groupName", out groupName);
                integrationsMap.Add("groupId", groupId);
                integrationsMap.Add("groupName", groupName);
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
            else if (userStorageMap == null && this._userStorageService != null)
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
            bool isPreSegmentationValid = CampaignHelper.checkForPreSegmentation(this._segmentEvaluator, campaign, userId, campaignKey, apiName, customVariables, false);
            if (!(isPreSegmentationValid && selectedCampaign != null))
            {
                return new UserAllocationInfo();
            }
            if (groupDetails.ContainsKey("groupId"))
            {
                List<BucketedCampaign> campaignList = CampaignHelper.getGroupCampaigns(this._settings, Convert.ToInt32(groupId));
                if (campaignList.Count == 0)
                    return new UserAllocationInfo();
                if (CampaignHelper.checkForStorageAndWhitelisting(this._variationAllocator, this._userStorageService, this._segmentEvaluator, file, apiName, campaignList, groupName, campaign, userId,
              variationTargetingVariables, customVariables,this._settings.isNB, this._settings.isNBv2, userStorageData,  true))
                {
                    LogInfoMessage.CalledCampaignNotWinner(file, campaignKey, userId, groupName.ToString());
                    return new UserAllocationInfo();
                }
                // Whether normalised/random implementation has to be done or advanced
                int megAlgoNumber = this._settings.getGroups()[groupId.ToString()].et != 0 ? this._settings.getGroups()[groupId.ToString()].et : RandomAlgo ;

                Dictionary<string, dynamic> processedCampaigns = CampaignHelper.getEligibleCampaigns(this._campaignAllocator, this._segmentEvaluator, campaignList, userId, apiName, customVariables);
                processedCampaigns.TryGetValue("eligibleCampaigns", out dynamic eligibleCampaigns);
                processedCampaigns.TryGetValue("inEligibleCampaigns", out dynamic inEligibleCampaigns);
                StringBuilder eligibleCampaignKeys = new StringBuilder();
                foreach (BucketedCampaign eachCampaign in eligibleCampaigns)
                {
                    eligibleCampaignKeys.Append(eachCampaign.Key).Append(", ");
                }
                StringBuilder inEligibleCampaignKeys = new StringBuilder();
                foreach (BucketedCampaign eachCampaign in inEligibleCampaigns)
                {
                    inEligibleCampaignKeys.Append(eachCampaign.Key).Append(", ");
                }
                string finalInEligibleCampaignKeys = inEligibleCampaignKeys.ToString();
                string finalEligibleCampaignKeys = eligibleCampaignKeys.ToString();

                LogDebugMessage.GotEligibleCampaigns(file, groupName.ToString(), userId, finalEligibleCampaignKeys.TrimEnd(new char[] { ',', ' ' }), finalInEligibleCampaignKeys.TrimEnd(new char[] { ',', ' ' }));
                LogInfoMessage.GotEligibleCampaigns(file, groupName.ToString(), userId, ((List<BucketedCampaign>)eligibleCampaigns).Count.ToString(), campaignList.Count.ToString());
                if (((List<BucketedCampaign>)eligibleCampaigns).Count == 1)
                {
                    return evaluateTrafficAndGetVariation(((List<BucketedCampaign>)eligibleCampaigns)[0], userStorageMap, customVariables, campaignKey, apiName,
                        variationTargetingVariables, initIntegration, userId, metaData);
                }
                else if (((List<BucketedCampaign>)eligibleCampaigns).Count > 1)
                {
                    BucketedCampaign winnerCampaign = null;
                    if(megAlgoNumber == 1){
                        winnerCampaign = CampaignHelper.normalizeAndFindWinningCampaign(this._campaignAllocator, (List<BucketedCampaign>)eligibleCampaigns,
                        userId, groupId.ToString());
                    }
                    else
                    {
                        winnerCampaign = CampaignHelper.advancedAlgoFindWinningCampaign(this._campaignAllocator, (List<BucketedCampaign>)eligibleCampaigns,
                        userId, groupId.ToString(),this._settings);
                    }
                    if (winnerCampaign != null && winnerCampaign.Id.Equals(campaign.Id))
                    {
                        LogInfoMessage.GotWinnerCampaign(file, campaignKey, userId, groupName.ToString());
                        return evaluateTrafficAndGetVariation(winnerCampaign, userStorageMap, customVariables, campaignKey, apiName, variationTargetingVariables, initIntegration, userId, metaData);
                    }
                    else
                    {
                        LogInfoMessage.CalledCampaignNotWinner(file, campaignKey, userId, groupName.ToString());
                        return new UserAllocationInfo();
                    }
                }
                else
                    return new UserAllocationInfo();
            }
            else
            {
                return evaluateTrafficAndGetVariation(campaign, userStorageMap, customVariables, campaignKey, apiName, variationTargetingVariables, initIntegration, userId, metaData);
            }
        }
        /// <summary>
        /// Get variation for the winning campaign.
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="userStorageMap"></param>
        /// <param name="customVariables"></param>
        /// <param name="campaignKey"></param>
        /// <param name="apiName"></param>
        /// <param name="variationTargetingVariables"></param>
        /// <param name="initIntegration"></param>
        /// <param name="userId"></param>
        /// <param name="metaData"></param>
        /// <param name="disableLogs"></param>
        /// <returns>
        /// If Variation is allocated, returns UserAssignedInfo with valid details, else return Empty UserAssignedInfo.
        /// </returns>
        private UserAllocationInfo evaluateTrafficAndGetVariation(BucketedCampaign campaign, UserStorageMap userStorageMap, Dictionary<string, dynamic> customVariables,
           string campaignKey, string apiName, Dictionary<string, dynamic> variationTargetingVariables, bool initIntegration, string userId, Dictionary<string, dynamic> metaData = null, bool disableLogs = false)

        {
            if (campaign != null)
            {
                Variation variation = this._variationAllocator.Allocate(userStorageMap, campaign, userId, this._settings.isNB, this._settings.isNBv2, this._settings.AccountId);
                if (variation != null)
                {
                    LogInfoMessage.VariationAllocated(file, userId, campaignKey, variation.Name);
                    LogDebugMessage.GotVariationForUser(file, userId, campaignKey, variation.Name, nameof(AllocateVariation));
                    if (this._userStorageService != null)
                    {
                        this._userStorageService.SetUserMap(userId, campaign.Key, variation.Name, null, metaData);
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
                    return new UserAllocationInfo(variation, campaign);
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
        private bool isCampaignActivated(string apiName, string userId, Campaign campaign, bool disableLogs = false)
        {
            if (!apiName.Equals(Constants.CampaignTypes.ACTIVATE, StringComparison.InvariantCultureIgnoreCase)
              && !apiName.Equals(Constants.CampaignTypes.IS_FEATURE_ENABLED, StringComparison.InvariantCultureIgnoreCase))
            {
                LogDebugMessage.CampaignNotActivated(file, apiName, campaign.Key, userId, disableLogs);
                LogInfoMessage.CampaignNotActivated(file, apiName.Equals(Constants.CampaignTypes.TRACK, StringComparison.InvariantCultureIgnoreCase) ? "track it" : "get the decision/value", campaign.Key, userId, disableLogs);
                return false;
            }
            return true;
        }
        private Variation FindTargetedVariation(string apiName, BucketedCampaign campaign, string campaignKey, string userId, Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, bool isNewBucketingEnabled = false, bool isNewBucketingv2Enabled = false, bool disableLogs = false)
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
                List<Variation> whiteListedVariations = this.GetWhiteListedVariationsList(apiName, userId, campaign, campaignKey, customVariables, variationTargetingVariables, disableLogs);

                string status = Constants.WhitelistingStatus.FAILED;
                string variationString = " ";
                Variation variation = this._variationAllocator.TargettedVariation(userId, campaign, whiteListedVariations, isNewBucketingEnabled, isNewBucketingv2Enabled);
                if (variation != null)
                {
                    status = Constants.WhitelistingStatus.PASSED;
                    variationString = $"and variation: {variation.Name} is assigned";
                }
                LogInfoMessage.WhitelistingStatus(typeof(IVWOClient).FullName, userId, campaignKey, apiName, variationString, status, disableLogs);
                return variation;
            }
            LogInfoMessage.SkippingWhitelisting(typeof(IVWOClient).FullName, userId, campaignKey, apiName, disableLogs);
            return null;
        }
        private List<Variation> GetWhiteListedVariationsList(string apiName, string userId, BucketedCampaign campaign, string campaignKey, Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, bool disableLogs = false)
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
                    if (this._segmentEvaluator.evaluate(userId, campaignKey, segmentationType, variation.Segments, variationTargetingVariables))
                    {
                        status = Constants.SegmentationStatus.PASSED;
                        result.Add(variation);
                    }
                    LogDebugMessage.SegmentationStatus(typeof(IVWOClient).FullName, userId, campaignKey, apiName, variation.Name, status, disableLogs);
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
        /// <param name="metaData"></param>
        /// <returns>
        /// If Variation is allocated and goal with given identifier is found, return UserAssignedInfo with valid information, otherwise, Empty UserAssignedInfo object.
        /// </returns>
        private UserAllocationInfo AllocateVariation(string campaignKey, string userId, BucketedCampaign campaign,
        Dictionary<string, dynamic> customVariables, Dictionary<string, dynamic> variationTargetingVariables, string goalIdentifier, string apiName,
        Dictionary<string, dynamic> userStorageData = null, Dictionary<string, dynamic> metaData = null)
        {
            if (_HookManager != null)
            {
                initIntegrationMap(campaign, apiName, userId, goalIdentifier, customVariables, variationTargetingVariables);
                LogDebugMessage.InitIntegrationMapForGoal(file, apiName, campaign.Key, userId);
            }
            var userAllocationInfo = this.AllocateVariation(campaignKey, userId, campaign, customVariables,
                variationTargetingVariables, apiName, userStorageData, metaData, true);
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
        private bool isGoalTriggerRequired(UserAllocationInfo assignedVariation, string campaignKey, string userId, string goalIdentifier, string variationName,
                   Dictionary<string, dynamic> userStorageData = null, Dictionary<string, dynamic> metaData = null)
        {
            UserStorageMap userMap = this._userStorageService != null ? this._userStorageService.GetUserMap(campaignKey, userId, userStorageData) : null;

            if (userMap == null) return true;
            string storedGoalIdentifier = null;
            if (userMap.GoalIdentifier != null)
            {
                storedGoalIdentifier = userMap.GoalIdentifier;
                string[] identifiers = storedGoalIdentifier.Split(new string[] { Constants.GOAL_IDENTIFIER_SEPERATOR }, StringSplitOptions.None);
                bool isMCA = assignedVariation.Goal.IsRevenueType() && (assignedVariation.Goal.mca!=null && assignedVariation.Goal.mca==-1);

                if (((IList<string>)identifiers).Contains(goalIdentifier) && !isMCA && !(assignedVariation.Goal.hasProps == true ))
                {
                    LogInfoMessage.GoalAlreadyTracked(file, userId, campaignKey, goalIdentifier);
                    return false;
                }
                else if (!(((IList<string>)identifiers).Contains(goalIdentifier)))
                {
                    storedGoalIdentifier = storedGoalIdentifier + Constants.GOAL_IDENTIFIER_SEPERATOR + goalIdentifier;
                    
                }
            }
            else
            {
                storedGoalIdentifier = goalIdentifier;
            }
            if (this._userStorageService != null)
            {
                this._userStorageService.SetUserMap(userId, campaignKey, variationName, storedGoalIdentifier, metaData);
            }

            return true;
        }

        #endregion private Methods
    }
}
