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
using Moq;
using Xunit;

namespace VWOSdk.Tests
{
    public class UserStorageAdapterTests
    {
        private readonly string MockCampaignKey = "MockCampaignKey";
        private readonly string MockUserId = "MockUserId";
        private readonly string MockVariationName = "MockVariationName";

        [Fact]
        public void GetUserMap_Should_Match_And_Return_Profile_Data_When_Get_Returns_Valid_Map()
        {
            var mockUserStorageService = Mock.GetUserStorageService();
            Mock.SetupGet(mockUserStorageService, GetUserStorageMap());
            UserStorageAdapter userStorageServiceAdapter = new UserStorageAdapter(mockUserStorageService.Object);
            var result = userStorageServiceAdapter.GetUserMap(MockCampaignKey, MockUserId);
            Assert.NotNull(result);
            Assert.Equal(MockUserId, result.UserId);
            Assert.Equal(MockCampaignKey, result.CampaignKey);
            Assert.Equal(MockVariationName, result.VariationName);
        }

        [Fact]
        public void GetUserMap_Should_Return_Null_When_Get_Returns_InValid_Map()
        {
            var mockUserStorageService = Mock.GetUserStorageService();
            Mock.SetupGet(mockUserStorageService, returnValue: null);
            UserStorageAdapter userStorageServiceAdapter = new UserStorageAdapter(mockUserStorageService.Object);
            var result = userStorageServiceAdapter.GetUserMap(MockCampaignKey, MockUserId);
            Assert.Null(result);
        }

        [Fact]
        public void GetUserMap_Should_Return_Null_When_Get_Throws_Execption()
        {
            var mockUserStorageService = Mock.GetUserStorageService();
            Mock.SetupGet(mockUserStorageService, new Exception("Test Method Exception"));
            UserStorageAdapter userStorageServiceAdapter = new UserStorageAdapter(mockUserStorageService.Object);
            var result = userStorageServiceAdapter.GetUserMap(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockUserStorageService.Verify(mock => mock.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockUserStorageService.Verify(mock => mock.Get(It.Is<string>(val => MockUserId.Equals(val)), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void SetUserMap_Should_Call_Set_With_Provided_Map()
        {
            var mockUserStorageService = Mock.GetUserStorageService();
            UserStorageAdapter userStorageServiceAdapter = new UserStorageAdapter(mockUserStorageService.Object);
            userStorageServiceAdapter.SetUserMap(MockUserId, MockCampaignKey, MockVariationName);
            mockUserStorageService.Verify(mock => mock.Set(It.IsAny<UserStorageMap>()), Times.Once);
            mockUserStorageService.Verify(mock => mock.Set(It.Is<UserStorageMap>(val => Verify(val))), Times.Once);
        }

        [Fact]
        public void SetUserMap_Should_Call_Set_With_Provided_Map_And_Should_Not_Throw_Exception_When_Service_Throws_Exception()
        {
            var mockUserStorageService = Mock.GetUserStorageService();
            Mock.SetupSet(mockUserStorageService, new Exception("Test Method Exception."));
            UserStorageAdapter userStorageServiceAdapter = new UserStorageAdapter(mockUserStorageService.Object);
            userStorageServiceAdapter.SetUserMap(MockUserId, MockCampaignKey, MockVariationName);
            mockUserStorageService.Verify(mock => mock.Set(It.IsAny<UserStorageMap>()), Times.Once);
            mockUserStorageService.Verify(mock => mock.Set(It.Is<UserStorageMap>(val => Verify(val))), Times.Once);
        }

        private bool Verify(UserStorageMap val)
        {
            if(val != null)
            {
                if(val.CampaignKey.Equals(MockCampaignKey) && val.UserId.Equals(MockUserId) && val.VariationName.Equals(MockVariationName))
                    return true;
            }
            return false;
        }

        private UserStorageMap GetUserStorageMap()
        {
            return new UserStorageMap()
            {
                CampaignKey = MockCampaignKey,
                UserId = MockUserId,
                VariationName = MockVariationName
            };
        }
    }
}
