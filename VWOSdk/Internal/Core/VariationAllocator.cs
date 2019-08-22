namespace VWOSdk
{
    internal class VariationAllocator : IVariationAllocator
    {
        private static readonly string file = typeof(VariationAllocator).FullName;
        private readonly IBucketService _userHasher;
        internal VariationAllocator(IBucketService userHasher)
        {
            this._userHasher = userHasher;
        }

        /// <summary>
        /// Allocate Variation by checking previously assigned variation if userProfileMap is provided, else by computing User Hash and matching it in bucket for eligible variation.
        /// </summary>
        /// <param name="userProfileMap"></param>
        /// <param name="campaign"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Variation Allocate(UserProfileMap userProfileMap, BucketedCampaign campaign, string userId)
        {
            if (campaign == null)
                return null;

            if (userProfileMap == null)
            {
                double maxVal = Constants.Variation.MAX_TRAFFIC_VALUE;
                double multiplier = maxVal / campaign.PercentTraffic / 100; ///This is to evenly spread all user among variations.
                var bucketValue = this._userHasher.ComputeBucketValue(userId, maxVal, multiplier, out double hashValue);
                var selectedVariation = campaign.Variations.Find(bucketValue);
                LogDebugMessage.VariationHashBucketValue(file, userId, campaign.Key, campaign.PercentTraffic, hashValue, bucketValue);
                return selectedVariation;
            }

            return campaign.Variations.Find(userProfileMap.VariationName, GetVariationName);
        }

        private string GetVariationName(Variation variation)
        {
            return variation.Name;
        }
    }
}