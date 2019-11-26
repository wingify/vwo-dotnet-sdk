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

using System;

namespace VWOSdk
{
    internal class UserProfileAdapter
    {
        private static readonly string file = typeof(UserProfileAdapter).FullName;
        private IUserProfileService _userProfileService;

        public UserProfileAdapter(IUserProfileService userProfileService)
        {
            this._userProfileService = userProfileService;
        }

        /// <summary>
        /// If UserProfileService is provided, Calls Lookup for given UserId and validate the result.
        /// </summary>
        /// <param name="campaignTestKey"></param>
        /// <param name="userId"></param>
        /// <returns>
        /// Returns userProfileMap if validation is success, else null.
        /// </returns>
        internal UserProfileMap GetUserMap(string campaignTestKey, string userId)
        {
            if (this._userProfileService == null)
            {
                LogDebugMessage.NoUserProfileServiceLookup(file);
                return null;
            }

            UserProfileMap userMap = TryGetUserMap(userId, campaignTestKey);

            if (userMap == null || string.IsNullOrEmpty(userMap.CampaignTestKey)
                || string.IsNullOrEmpty(userMap.VariationName) || string.IsNullOrEmpty(userMap.UserId)
                || string.Equals(userMap.UserId, userId) == false || string.Equals(userMap.CampaignTestKey, campaignTestKey) == false)
            {
                LogDebugMessage.NoStoredVariation(file, userId, campaignTestKey);
                return null;
            }

            LogInfoMessage.GotStoredVariation(file, userMap.VariationName, campaignTestKey, userId);
            LogDebugMessage.GettingStoredVariation(file, userId, campaignTestKey, userMap.VariationName);
            return userMap;
        }

        /// <summary>
        /// Calls Lookup within try to suppress any Exception from outside of SDK application.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private UserProfileMap TryGetUserMap(string userId, string campaignTestKey)
        {
            try
            {
                LogInfoMessage.LookingUpUserProfileService(file, userId, campaignTestKey);
                return this._userProfileService.Lookup(userId, campaignTestKey);
            }
            catch (Exception ex)
            {
                LogErrorMessage.LookUpUserProfileServiceFailed(file, userId, campaignTestKey);
            }

            return null;
        }

        internal void SaveUserMap(string userId, string campaignTestKey, string variationName)
        {
            if (this._userProfileService == null)
            {
                LogDebugMessage.NoUserProfileServiceSave(file);
                return;
            }

            try
            {
                LogInfoMessage.SavingDataUserProfileService(file, userId);
                this._userProfileService.Save(new UserProfileMap(userId, campaignTestKey, variationName));
                return;
            }
            catch (Exception ex)
            {
                LogErrorMessage.SaveUserProfileServiceFailed(file, userId);
            }
        }


    }
}
