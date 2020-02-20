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

using Xunit;

namespace VWOSdk.Tests
{
    public class WeightRangeTests
    {
        [Theory]
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(1, 2, 1, 4, false)]
        [InlineData(1, 2, 3, 2, false)]
        [InlineData(1, 2, 1, 2, true)]
        public void Equals_Should_Return_Desired_Results(double xFrom, double xTo, double yFrom, double yTo, bool expectedResult)
        {
            var x = new WeightRange(xFrom, xTo);
            var y = new WeightRange(yFrom, yTo);
            var result = x.Equals(y);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(1, 2, 1, 4, false)]
        [InlineData(1, 2, 3, 2, false)]
        [InlineData(1, 2, 1, 2, true)]
        public void GetHashCode_Should_Return_Desired_Results(double xFrom, double xTo, double yFrom, double yTo, bool expectedResult)
        {
            var x = new WeightRange(xFrom, xTo);
            var y = new WeightRange(yFrom, yTo);
            var xResult = x.GetHashCode();
            var yResult = y.GetHashCode();
            bool actualResult = xResult == yResult;
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
