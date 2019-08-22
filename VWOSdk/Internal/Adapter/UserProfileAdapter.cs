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

            UserProfileMap userMap = TryGetUserMap(userId);

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
        private UserProfileMap TryGetUserMap(string userId)
        {
            try
            {
                LogInfoMessage.LookingUpUserProfileService(file, userId);
                return this._userProfileService.Lookup(userId);
            }
            catch (Exception ex)
            {
                LogErrorMessage.LookUpUserProfileServiceFailed(file, userId);
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