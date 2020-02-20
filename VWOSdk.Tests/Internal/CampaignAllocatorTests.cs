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
using System.Collections.Generic;
using Xunit;

namespace VWOSdk.Tests
{
    public class CampaignAllocatorTests
    {
        private readonly string MockSdkKey = "MockSdkKey";

        static CampaignAllocatorTests()
        {
            VWO.Configure(LogLevel.DEBUG);
        }

        public CampaignAllocatorTests()
        {

        }

        private readonly string MockUserId = "MockUserId";
        private readonly string MockCampaignKey = "MockCampaignKey";
        private readonly string MockVariationName = "VariationName";

        [Fact]
        public void Allocate_Should_Return_Null_When_Valid_UserStorageMap_With_InValid_Campaign_Is_Given()
        {
            var mockUserHasher = MockUserHasher.Get();
            var campaignAllocator = GetCampaignResolver(mockUserHasher);
            UserStorageMap userStorageMap = new UserStorageMap(MockUserId, MockCampaignKey + MockCampaignKey, MockVariationName);
            var selectedCampaign = campaignAllocator.Allocate(GetAccountSettings(), userStorageMap, MockCampaignKey, MockUserId);
            Assert.Null(selectedCampaign);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void Allocate_Should_Return_Null_When_Valid_UserStorageMap_With_Valid_Campaign_Is_Given_And_Campaign_Is_Not_Running()
        {
            var mockUserHasher = MockUserHasher.Get();
            var campaignResolver = GetCampaignResolver(mockUserHasher);
            UserStorageMap userStorageMap = new UserStorageMap(MockUserId, MockCampaignKey, MockVariationName);
            var selectedCampaign = campaignResolver.Allocate(GetAccountSettings(status: "NOT_RUNNING"), userStorageMap, MockCampaignKey, MockUserId);
            Assert.Null(selectedCampaign);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void Allocate_Should_Return_Null_When_InValid_CampaignKey_Is_Provided()
        {
            var mockUserHasher = MockUserHasher.Get();
            var campaignResolver = GetCampaignResolver(mockUserHasher);
            UserStorageMap userStorageMap = null;
            var selectedCampaign = campaignResolver.Allocate(GetAccountSettings(status: "RUNNING"), userStorageMap, MockCampaignKey + MockCampaignKey, MockUserId);
            Assert.Null(selectedCampaign);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void Allocate_Should_Compute_Hash_When_UserStorageMap_Is_Null()
        {
            var mockUserHasher = Mock.GetUserHasher();
            Mock.SetupCompute(mockUserHasher, returnVal: 10);
            CampaignAllocator campaignResolver = GetCampaignResolver(mockUserHasher);
            UserStorageMap userStorageMap = null;
            var selectedCampaign = campaignResolver.Allocate(GetAccountSettings(), userStorageMap, MockCampaignKey, MockUserId);
            Assert.NotNull(selectedCampaign);
            Assert.Equal(MockCampaignKey, selectedCampaign.Key);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.Is<string>((val) => MockUserId.Equals(val)), It.Is<double>((val) => 100 == val), It.Is<double>(val => 1 == val)), Times.Once);
        }

        [Fact]
        public void Allocate_Should_Return_Null_Audience_Condition_Not_Met()
        {
            var mockUserHasher = Mock.GetUserHasher();
            Mock.SetupCompute(mockUserHasher, returnVal: 80);
            CampaignAllocator campaignResolver = GetCampaignResolver(mockUserHasher);
            UserStorageMap userStorageMap = null;
            var selectedCampaign = campaignResolver.Allocate(GetAccountSettings(), userStorageMap, MockCampaignKey, MockUserId);
            Assert.Null(selectedCampaign);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.Is<string>((val) => MockUserId.Equals(val)), It.Is<double>((val) => 100 == val), It.Is<double>(val => 1 == val)), Times.Once);
        }

        private AccountSettings GetAccountSettings(string status = "RUNNING")
        {
            var campaigns = GetCampaigns(status: status);
            return new AccountSettings(MockSdkKey, campaigns, 123456, 1);
        }

        private List<BucketedCampaign> GetCampaigns(string status = "RUNNING")
        {
            var result = new List<BucketedCampaign>();
            result.Add(GetCampaign(status: status));
            return result;
        }

        private BucketedCampaign GetCampaign(string campaignKey = null, string variationName = null, string status = "RUNNING")
        {
            campaignKey = campaignKey ?? MockCampaignKey;
            variationName = variationName ?? MockVariationName;
            return new BucketedCampaign(1, 70, campaignKey, status, null)
            {
                Variations = GetVariations(variationName),
            };
        }

        private RangeBucket<Variation> GetVariations(string variationName)
        {
            var result = new RangeBucket<Variation>(10000);
            result.Add(100, new Variation(1, variationName, null, 100, false));
            return result;
        }

        private CampaignAllocator GetCampaignResolver(Mock<IBucketService> mockUserHasher = null)
        {
            mockUserHasher = mockUserHasher ?? MockUserHasher.Get();
            return new CampaignAllocator(mockUserHasher.Object);
        }
    }
}
