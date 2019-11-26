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
    public interface IUserProfileService
    {
        /// <summary>
        /// Lookup previously allocated Campaign and Variation.
        /// </summary>
        /// <param name="userId">UserId for the user to fetch details.</param>
        /// <param name="campaignTestKey">Campaign Key to look up.</param>
        /// <returns></returns>
        UserProfileMap Lookup(string userId, string campaignTestKey);

        /// <summary>
        /// Save allocated Campaign and Variation.
        /// </summary>
        /// <param name="userProfileMap">User details with UserId, Campaign and Variation.</param>
        void Save(UserProfileMap userProfileMap);
    }
}
