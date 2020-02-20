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

using System.Collections.Generic;
using Xunit;

namespace VWOSdk.Tests
{
    public class RangeBucketTests
    {
        private RangeBucket<int> GetTestRangeBucket(int maxVal = 100, bool continousRange = true)
        {
            var result = new RangeBucket<int>(maxVal, continousRange);
            List<KeyValuePair<int, int>> weightValuePairs = new List<KeyValuePair<int, int>>()
            {
                new KeyValuePair<int, int>(20, 1),
                new KeyValuePair<int, int>(20, 2),
                new KeyValuePair<int, int>(20, 3),
                new KeyValuePair<int, int>(20, 4),
                new KeyValuePair<int, int>(20, 5),
            };

            foreach (var weightKeyPair in weightValuePairs)
            {
                result.Add(weightKeyPair.Key, weightKeyPair.Value);
            }

            return result;
        }

        [Theory]
        [InlineData(100, 1, 1)]
        [InlineData(100, 2, 1)]
        [InlineData(100, 19, 1)]
        [InlineData(100, 20, 1)]
        [InlineData(100, 21, 2)]
        [InlineData(100, 22, 2)]
        [InlineData(100, 39, 2)]
        [InlineData(100, 40, 2)]
        [InlineData(100, 41, 3)]
        [InlineData(100, 42, 3)]
        [InlineData(100, 59, 3)]
        [InlineData(100, 60, 3)]
        [InlineData(100, 61, 4)]
        [InlineData(100, 62, 4)]
        [InlineData(100, 79, 4)]
        [InlineData(100, 80, 4)]
        [InlineData(100, 81, 5)]
        [InlineData(100, 82, 5)]
        [InlineData(100, 99, 5)]
        [InlineData(100, 100, 5)]
        [InlineData(100, 101, default(int))]
        [InlineData(100, -1, default(int))]
        [InlineData(100, 0, default(int))]
        [InlineData(100, -100, default(int))]
        [InlineData(100, 1000, default(int))]

        [InlineData(1000, 10, 1)]
        [InlineData(1000, 20, 1)]
        [InlineData(1000, 190, 1)]
        [InlineData(1000, 200, 1)]
        [InlineData(1000, 210, 2)]
        [InlineData(1000, 220, 2)]
        [InlineData(1000, 390, 2)]
        [InlineData(1000, 400, 2)]
        [InlineData(1000, 410, 3)]
        [InlineData(1000, 420, 3)]
        [InlineData(1000, 590, 3)]
        [InlineData(1000, 600, 3)]
        [InlineData(1000, 610, 4)]
        [InlineData(1000, 620, 4)]
        [InlineData(1000, 790, 4)]
        [InlineData(1000, 800, 4)]
        [InlineData(1000, 810, 5)]
        [InlineData(1000, 820, 5)]
        [InlineData(1000, 990, 5)]
        [InlineData(1000, 1000, 5)]
        [InlineData(1000, 1010, default(int))]
        [InlineData(1000, -10, default(int))]
        [InlineData(1000, 0, default(int))]
        [InlineData(1000, -1000, default(int))]
        [InlineData(1000, 10000, default(int))]
        public void Find_By_Hash_Should_Return_Desired_Result(int bucketMaxVal, int hashValue, int expectedResult)
        {
            var bucket = GetTestRangeBucket(bucketMaxVal);
            var result = bucket.Find(hashValue);
            Assert.Equal(expectedResult, result);
        }

        //[Theory]
        //[InlineData(100, 1, 1, 1, false)]
        //[InlineData(100, 2, 1, 1, false)]
        //[InlineData(100, 19, 1, 1, false)]
        //[InlineData(100, 20, 1, 1, false)]

        //[InlineData(100, 1, 2, 2, false)]
        //[InlineData(100, 2, 2, 2, false)]
        //[InlineData(100, 19, 2, 2, false)]
        //[InlineData(100, 20, 2, 2, false)]

        //[InlineData(100, 1, 3, 3, false)]
        //[InlineData(100, 2, 3, 3, false)]
        //[InlineData(100, 19, 3, 3, false)]
        //[InlineData(100, 20, 3, 3, false)]

        //[InlineData(100, 1, 4, 4, false)]
        //[InlineData(100, 2, 4, 4, false)]
        //[InlineData(100, 19, 4, 4, false)]
        //[InlineData(100, 20, 4, 4, false)]

        //[InlineData(100, 1, 5, 5, false)]
        //[InlineData(100, 2, 5, 5, false)]
        //[InlineData(100, 19, 5, 5, false)]
        //[InlineData(100, 20, 5, 5, false)]

        //[InlineData(100, 101, 1, default(int), false)]
        //[InlineData(100, -1, 3, default(int), false)]
        //[InlineData(100, -100, 3, default(int), false)]
        //[InlineData(100, 10000, 3, default(int), false)]

        //[InlineData(100, 1, 0, default(int), false)]
        //[InlineData(100, 2, -10, default(int), false)]
        //[InlineData(100, 3, 20, default(int), false)]
        //[InlineData(100, 4, 10, default(int), false)]
        //public void Find_By_Hash_Should_Return_Desired_Result_When_ContinousRange_Is_False(int bucketMaxVal, int hashValue, int searchValue, int expectedResult, bool continousRange = true)
        //{
        //    var bucket = GetTestRangeBucket(bucketMaxVal, continousRange);
        //    var result = bucket.Find(hashValue);
        //    Assert.Equal(expectedResult, result);
        //}

        [Theory]
        [InlineData(101, default(int))]
        [InlineData(-1, default(int))]
        [InlineData(0, default(int))]
        [InlineData(-100, default(int))]
        [InlineData(1000, default(int))]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        [InlineData(5, 5)]
        public void Find_Should_Return_Desired_Result(int searchKey, int expectedResult)
        {
            var bucket = GetTestRangeBucket();
            var result = bucket.Find(searchKey, (val) => val);
            Assert.Equal(expectedResult, result);
        }
    }
}
