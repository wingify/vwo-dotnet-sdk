using System;
using System.Text;

namespace VWOSdk
{
    internal class Murmur32BucketService : IBucketService
    {
        private static readonly string file = typeof(Murmur32BucketService).ToString();
        private readonly Murmur.Murmur32 _murmur32;
        private readonly static double diviser = Math.Pow(2, 32);

        internal Murmur32BucketService()
        {
            this._murmur32 = Murmur.MurmurHash.Create32(seed: 1, managed: true); // returns a 128-bit algorithm using "unsafe" code with default seed
        }

        public double ComputeBucketValue(string userId, double maxVal, double multiplier, out double hashValue)
        {
            byte[] hash = this._murmur32.ComputeHash(Encoding.UTF8.GetBytes(userId));
            hashValue = BitConverter.ToUInt32(hash, 0);
            var bucketValue = Compute(hashValue, maxVal, multiplier);
            LogDebugMessage.UserHashBucketValue(file, userId, hashValue, bucketValue);
            return bucketValue;
        }

        private static double Compute(double hashValue, double maxValue, double multiplier)
        {
            double ratio = hashValue / diviser;
            double multipliedValue = (maxValue * ratio + 1) * multiplier;
            return Math.Floor(multipliedValue);
        }

        public double ComputeBucketValue(string userId, double maxVal, double multiplier)
        {
            return this.ComputeBucketValue(userId, maxVal, multiplier, out double hashValue);
        }
    }
}
