﻿#pragma warning disable 1587
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
    internal class Constants
    {
        internal static readonly string PLATFORM = "server";
        internal static readonly string GOAL_IDENTIFIER_SEPERATOR = "_vwo_";
        internal static readonly string DECISION_TYPES = "CAMPAIGN_DECISION";
        internal static readonly string SDK_NAME = "netstandard2.0";
        internal static readonly string TRACK_EVENT_NAME = "vwo_variationShown";
        internal static readonly string PUSH_EVENT_NAME = "vwo_syncVisitorProp";
        internal static class Campaign
        {
            internal static readonly string STATUS_RUNNING = "RUNNING";
            internal static readonly double MAX_TRAFFIC_PERCENT = 100;
        }

        internal static class Variation
        {
            internal static readonly double MAX_TRAFFIC_VALUE = 10000;
        }

        public static class Endpoints
        {
            internal static readonly string BASE_URL = "https://dev.visualwebsiteoptimizer.com";
            internal static readonly string SERVER_SIDE = "server-side";
            internal static readonly string ACCOUNT_SETTINGS = "settings";
            internal static readonly string WEBHOOK_SETTINGS_URL = "pull";
            internal static readonly string TRACK_USER = "track-user";
            internal static readonly string TRACK_GOAL = "track-goal";
            internal static readonly string PUSH_TAGS = "push";
            internal static readonly string BATCH_EVENTS = "batch-events";
            internal static readonly string EVENT_ARCH = "events/t";
        }

        public static class CampaignStatus
        {
            internal static readonly string RUNNING = "RUNNING";
        }

        public static class SegmentationType
        {
            internal static readonly string WHITELISTING = "whitelisting";
            internal static readonly string PRE_SEGMENTATION = "pre-segmentation";
        }

        public static class SegmentationStatus
        {
            internal static readonly string PASSED = "PASSED";
            internal static readonly string FAILED = "FAILED";
        }

        public static class WhitelistingStatus
        {
            internal static readonly string PASSED = "PASSED";
            internal static readonly string FAILED = "FAILED";
        }

        public static class CampaignTypes
        {
            internal static readonly string VISUAL_AB = "VISUAL_AB";
            internal static readonly string FEATURE_TEST = "FEATURE_TEST";
            internal static readonly string FEATURE_ROLLOUT = "FEATURE_ROLLOUT";
            internal static readonly string ACTIVATE = "Activate";
            internal static readonly string IS_FEATURE_ENABLED = "IsFeatureEnabled";
            internal static readonly string TRACK = "Track";

        }

        public static class PushApi
        {
            internal static readonly int TAG_KEY_LENGTH = 255;
            internal static readonly int TAG_VALUE_LENGTH = 255;
        }

        public static class OperatorTypes
        {
            internal const string AND = "and";
            internal const string OR = "or";
            internal const string NOT = "not";
        }

        public static class OperandTypes
        {
            internal const string CUSTOM_VARIABLE = "custom_variable";
            internal const string USER = "user";
        }

        public static class OperandValueTypesName
        {
            internal const string REGEX = "regex";
            internal const string WILDCARD = "wildcard";
            internal const string LOWER = "lower";
            internal const string EQUALS = "equals";
        }

        public static class OperandValueTypes
        {
            internal const string LOWER = "lower";
            internal const string CONTAINS = "contains";
            internal const string STARTS_WITH = "starts_with";
            internal const string ENDS_WITH = "ends_with";
            internal const string REGEX = "regex";
            internal const string EQUALS = "equals";
        }

        public static class OperandValueBooleanTypes
        {
            internal const string TRUE = "true";
            internal const string FALSE = "false";
        }

        public static class DotnetVariableTypes
        {
            internal static Dictionary<string, string> VALUES = new Dictionary<string, string>() {
                {"string", "String"},
                {"integer", "Int"},
                {"double", "Double"},
                {"boolean", "Bool"},
                {"json", "Json"}
            };
        }

        public static class VariableTypes
        {
            internal const string STRING = "string";
            internal const string INTEGER = "integer";
            internal const string DOUBLE = "double";
            internal const string BOOLEAN = "boolean";
            internal const string JSON = "json";
        }

        public static class GoalTypes
        {
            internal const string REVENUE = "REVENUE_TRACKING";
            internal const string CUSTOM = "CUSTOM_GOAL";
            internal const string ALL = "ALL";

            internal static List<string> VALUES =  new List<string>() {
                "REVENUE_TRACKING", "CUSTOM_GOAL", "ALL"
            };
        }
    }
}
