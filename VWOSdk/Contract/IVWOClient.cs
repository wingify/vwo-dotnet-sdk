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
    public interface IVWOClient
    {
        /// <summary>
        /// Activates a server-side A/B test for a specified user for a server-side running campaign.
        /// </summary>
        /// <param name="campaignTestKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <returns>
        /// The name of the variation in which the user is bucketed, or null if the user doesn't qualify to become a part of the campaign.
        /// </returns>
        string Activate(string campaignTestKey, string userId);

        /// <summary>
        /// Activates a server-side A/B test for the specified user for a particular running campaign.
        /// </summary>
        /// <param name="campaignTestKey">Campaign key to uniquely identify a server-side campaign.</param>
        /// <param name="userId">User ID which uniquely identifies each user.</param>
        /// <returns>
        /// The name of the variation in which the user is bucketed, or null if the user doesn't qualify to become a part of the campaign.
        /// </returns>
        string GetVariation(string campaignTestKey, string userId);

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
        bool Track(string campaignTestKey, string userId, string goalIdentifier, string revenueValue = null);

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
        bool Track(string campaignTestKey, string userId, string goalIdentifier, int revenueValue);

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
        bool Track(string campaignTestKey, string userId, string goalIdentifier, float revenueValue);
    }
}
