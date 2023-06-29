﻿#pragma warning disable 1587
/**
 * Copyright 2019-2021 Wingify Software Pvt. Ltd.
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

using Newtonsoft.Json.Linq;
using Moq;
using Xunit;

namespace VWOSdk.Tests
{
    public class VariationAllocatorTests
    {
        internal static Settings SampleSettingsFileWithNewBucketing = new FileReaderApiCaller("SampleSettingsFileWithNewBucketing").GetJsonContent<Settings>();
        private readonly string MockUserId = "MockUserId";
        private readonly string MockCampaignKey = "MockCampaignKey";
        private readonly string MockVariationName = "VariationName";
        private static IVWOClient vwoInstance { get; set; }

        public VariationAllocatorTests()
        {

        }

        [Fact]
        public void Allocate_Should_Return_Null_When_SelectedCampaign_Is_Null()
        {
            var mockUserHasher = MockUserHasher.Get();
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);
            var selectedVariation = variationResolver.Allocate(null, null, MockUserId);
            Assert.Null(selectedVariation);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void Allocate_Should_Compute_Hash_When_UserStorageMap_Is_Null()
        {
            var mockUserHasher = Mock.GetUserHasher();
            Mock.SetupComputeBucketValue(mockUserHasher, returnVal: 10, outHashValue: 1234567);
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);
            UserStorageMap userStorageMap = null;
            var selectedVariation = variationResolver.Allocate(userStorageMap, GetCampaign(MockCampaignKey), MockUserId);
            Assert.NotNull(selectedVariation);
            Assert.Equal(MockVariationName, selectedVariation.Name);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.Is<string>((val) => MockUserId.Equals(val)), It.Is<double>((val) => 10000 == val), It.Is<double>(val => 1 == val)), Times.Once);
        }

        [Fact]
        public void Allocate_Should_Compute_Hash_On_CampaignId_UserId_When_IsBucketingSeed_True()
        {
            var mockUserHasher = Mock.GetUserHasher();
            var campaignId = 1;
            var userId = "Varun";
            Mock.SetupSeedComputeBucketValue(mockUserHasher, returnVal: 10, outHashValue: 1234567);
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);
            UserStorageMap userStorageMap = null;
            var selectedVariation = variationResolver.Allocate(userStorageMap, GetCampaign(MockCampaignKey, null, true, campaignId), userId);
            Assert.NotNull(selectedVariation);
            Assert.Equal(MockVariationName, selectedVariation.Name);
            var BucketingSeed = campaignId.ToString() + "_" + userId;
            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.Is<string>((val) => BucketingSeed.Equals(BucketingSeed)), It.Is<string>((val) => userId.Equals(val)), It.Is<double>((val) => 10000 == val), It.Is<double>(val => 1 == val)), Times.Once);
        }
        [Fact]
        public void Allocate_Should_Compute_Hash_On_UserId_When_IsBucketingSeed_False()
        {
            var mockUserHasher = Mock.GetUserHasher();
            var campaignId = 1;
            var userId = "Anup";
            Mock.SetupComputeBucketValue(mockUserHasher, returnVal: 10, outHashValue: 1234567);
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);         
            UserStorageMap userStorageMap = null;
            variationResolver.Allocate(userStorageMap, GetCampaign(MockCampaignKey, null, false, campaignId), userId);        
            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.Is<string>((val) => userId.Equals(val)), It.Is<double>((val) => 10000 == val), It.Is<double>(val => 1 == val)), Times.Once);
        }       
        [Fact]
        public void Allocate_Should_Not_Compute_Hash_When_Valid_UserStorageMap_With_Valid_Variation_Is_Given()
        {
            var mockUserHasher = MockUserHasher.Get();
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);
            UserStorageMap userStorageMap = new UserStorageMap(MockUserId, MockCampaignKey, MockVariationName);
            var selectedVariation = variationResolver.Allocate(userStorageMap, GetCampaign(MockCampaignKey), MockUserId);
            Assert.NotNull(selectedVariation);
            Assert.Equal(MockVariationName, selectedVariation.Name);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void Allocate_Should_Return_Null_When_Valid_UserStorageMap_With_InValid_Variation_Is_Given()
        {
            var mockUserHasher = MockUserHasher.Get();
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);
            UserStorageMap userStorageMap = new UserStorageMap(MockUserId, MockCampaignKey, MockVariationName + MockVariationName);
            var selectedVariation = variationResolver.Allocate(userStorageMap, GetCampaign(MockCampaignKey), MockUserId);
            Assert.Null(selectedVariation);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }
    
        private BucketedCampaign GetCampaign(string campaignKey = null, string variationName = null, bool isBucketingSeed = false, int campaignId = 0)
        {
            campaignKey = campaignKey ?? MockCampaignKey;
            variationName = variationName ?? MockVariationName;
            return new BucketedCampaign(campaignId, "test", 100, campaignKey, null, null, false, isBucketingSeed)
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

        private VariationAllocator GetVariationResolver(Mock<IBucketService> mockUserHasher = null)
        {
            mockUserHasher = mockUserHasher ?? MockUserHasher.Get();
            return new VariationAllocator(mockUserHasher.Object);
        }
    }
}
