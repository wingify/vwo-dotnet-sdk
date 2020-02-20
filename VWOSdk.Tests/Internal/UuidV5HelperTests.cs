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
    public class UuidV5HelperTests
    {
        [Theory]
        [InlineData(123456, "Ashley", "4C9BA81BAF53591488EB5FD5E66A98B9")]
        [InlineData(123456, "Bill", "7E74487DBCC758E79CEAC4CD50C0458D")]
        [InlineData(123456, "Emma", "881F627CD7FC5C97B0F3BE9D58A09DA6")]
        [InlineData(123456, "Faizan", "6EA28F5D5CE25F5FA8B808E30039DE09")]
        public void UuidCreator_test(long accountId, string userId, string expected)
        {
            var response = UuidV5Helper.Compute(accountId, userId);
            Assert.Equal(expected, response);
        }
    }
}
