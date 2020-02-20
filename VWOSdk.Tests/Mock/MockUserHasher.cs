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

using Moq;

namespace VWOSdk.Tests
{
    internal class MockUserHasher
    {
        public static Mock<IBucketService> Get()
        {
            var mock = new Mock<IBucketService>();
            return mock;
        }

        internal static void SetupCompute(Mock<IBucketService> mockUserHasher, int returnVal)
        {
            mockUserHasher.Setup(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns(returnVal);
        }

        internal static void SetupComputeBucketValue(Mock<IBucketService> mockUserHasher, int returnVal, double outHashValue)
        {
            SetupCompute(mockUserHasher, returnVal);
            mockUserHasher.Setup(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), out outHashValue))
                .Returns<string, double, double, double>((userid, maxVal, multiplier, hashvalue) => mockUserHasher.Object.ComputeBucketValue(userid, maxVal, multiplier));
        }
    }
}
