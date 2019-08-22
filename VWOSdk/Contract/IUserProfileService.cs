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
