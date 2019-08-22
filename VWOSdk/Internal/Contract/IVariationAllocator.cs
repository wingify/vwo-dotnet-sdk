namespace VWOSdk
{
    internal interface IVariationAllocator
    {
        Variation Allocate(UserProfileMap userProfileMap, BucketedCampaign campaign, string userId);
    }
}