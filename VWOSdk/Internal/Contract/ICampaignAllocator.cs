namespace VWOSdk
{
    internal interface ICampaignAllocator
    {
        BucketedCampaign Allocate(AccountSettings settings, UserProfileMap userProfileMap, string campaignTestKey, string userId, string apiName = null);
    }
}
