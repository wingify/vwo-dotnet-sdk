namespace VWOSdk
{
    internal interface IBucketService
    {
        double ComputeBucketValue(string userId, double maxVal, double multiplier);
        double ComputeBucketValue(string userId, double maxVal, double multiplier, out double hashValue);
    }
}