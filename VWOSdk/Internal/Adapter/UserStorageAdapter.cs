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

using System;

namespace VWOSdk
{
    internal class UserStorageAdapter
    {
        private static readonly string file = typeof(UserStorageAdapter).FullName;
        private IUserStorageService _userStorageService;

        public UserStorageAdapter(IUserStorageService userStorageService)
        {
            this._userStorageService = userStorageService;
        }

        /// <summary>
        /// If UserStorageService is provided, Calls Get for given UserId and validate the result.
        /// </summary>
        /// <param name="campaignKey"></param>
        /// <param name="userId"></param>
        /// <returns>
        /// Returns userStorageMap if validation is success, else null.
        /// </returns>
        internal UserStorageMap GetUserMap(string campaignKey, string userId)
        {
            if (this._userStorageService == null)
            {
                LogDebugMessage.NoUserStorageServiceGet(file);
                return null;
            }

            UserStorageMap userMap = TryGetUserMap(userId, campaignKey);

            if (userMap == null || string.IsNullOrEmpty(userMap.CampaignKey)
                || string.IsNullOrEmpty(userMap.VariationName) || string.IsNullOrEmpty(userMap.UserId)
                || string.Equals(userMap.UserId, userId) == false || string.Equals(userMap.CampaignKey, campaignKey) == false)
            {
                LogDebugMessage.NoStoredVariation(file, userId, campaignKey);
                return null;
            }

            LogInfoMessage.GotStoredVariation(file, userMap.VariationName, campaignKey, userId);
            return userMap;
        }

        /// <summary>
        /// Calls Get within try to suppress any Exception from outside of SDK application.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private UserStorageMap TryGetUserMap(string userId, string campaignKey)
        {
            try
            {
                LogInfoMessage.LookingUpUserStorageService(file, userId, campaignKey);
                return this._userStorageService.Get(userId, campaignKey);
            }
            catch (Exception ex)
            {
                LogErrorMessage.GetUserStorageServiceFailed(file, userId, campaignKey);
            }

            return null;
        }

        internal void SetUserMap(string userId, string campaignKey, string variationName)
        {
            if (this._userStorageService == null)
            {
                LogDebugMessage.NoUserStorageServiceSet(file);
                return;
            }

            try
            {
                LogInfoMessage.SavingDataUserStorageService(file, userId);
                this._userStorageService.Set(new UserStorageMap(userId, campaignKey, variationName));
                return;
            }
            catch (Exception ex)
            {
                LogErrorMessage.SetUserStorageServiceFailed(file, userId);
            }
        }


    }
}
