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
