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
    internal class Constants
    {
        internal static readonly string PLATFORM = "server";

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
            internal static readonly string TRACK_USER = "track-user";
            internal static readonly string TRACK_GOAL = "track-goal";
        }
    }
}
