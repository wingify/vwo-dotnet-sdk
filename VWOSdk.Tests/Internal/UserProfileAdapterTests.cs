using System;
using Moq;
using Xunit;

namespace VWOSdk.Tests
{
    public class UserProfileAdapterTests
    {
        private readonly string MockCampaignTestKey = "MockCampaignTestKey";
        private readonly string MockUserId = "MockUserId";
        private readonly string MockVariationName = "MockVariationName";

        [Fact]
        public void GetUserMap_Should_Match_And_Return_Profile_Data_When_LookUp_Returns_Valid_Map()
        {
            var mockUserProfileService = Mock.GetUserProfileService();
            Mock.SetupLookup(mockUserProfileService, GetUserProfileMap());
            UserProfileAdapter userProfileServiceAdapter = new UserProfileAdapter(mockUserProfileService.Object);
            var result = userProfileServiceAdapter.GetUserMap(MockCampaignTestKey, MockUserId);
            Assert.NotNull(result);
            Assert.Equal(MockUserId, result.UserId);
            Assert.Equal(MockCampaignTestKey, result.CampaignTestKey);
            Assert.Equal(MockVariationName, result.VariationName);
        }

        [Fact]
        public void GetUserMap_Should_Return_Null_When_LookUp_Returns_InValid_Map()
        {
            var mockUserProfileService = Mock.GetUserProfileService();
            Mock.SetupLookup(mockUserProfileService, returnValue: null);
            UserProfileAdapter userProfileServiceAdapter = new UserProfileAdapter(mockUserProfileService.Object);
            var result = userProfileServiceAdapter.GetUserMap(MockCampaignTestKey, MockUserId);
            Assert.Null(result);
        }

        [Fact]
        public void GetUserMap_Should_Return_Null_When_LookUp_Throws_Execption()
        {
            var mockUserProfileService = Mock.GetUserProfileService();
            Mock.SetupLookup(mockUserProfileService, new Exception("Test Method Exception"));
            UserProfileAdapter userProfileServiceAdapter = new UserProfileAdapter(mockUserProfileService.Object);
            var result = userProfileServiceAdapter.GetUserMap(MockCampaignTestKey, MockUserId);
            Assert.Null(result);

            mockUserProfileService.Verify(mock => mock.Lookup(It.IsAny<string>()), Times.Once);
            mockUserProfileService.Verify(mock => mock.Lookup(It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void SaveUserMap_Should_Call_Save_With_Provided_Map()
        {
            var mockUserProfileService = Mock.GetUserProfileService();
            UserProfileAdapter userProfileServiceAdapter = new UserProfileAdapter(mockUserProfileService.Object);
            userProfileServiceAdapter.SaveUserMap(MockUserId, MockCampaignTestKey, MockVariationName);
            mockUserProfileService.Verify(mock => mock.Save(It.IsAny<UserProfileMap>()), Times.Once);
            mockUserProfileService.Verify(mock => mock.Save(It.Is<UserProfileMap>(val => Verify(val))), Times.Once);
        }

        [Fact]
        public void SaveUserMap_Should_Call_Save_With_Provided_Map_And_Should_Not_Throw_Exception_When_Service_Throws_Exception()
        {
            var mockUserProfileService = Mock.GetUserProfileService();
            Mock.SetupSave(mockUserProfileService, new Exception("Test Method Exception."));
            UserProfileAdapter userProfileServiceAdapter = new UserProfileAdapter(mockUserProfileService.Object);
            userProfileServiceAdapter.SaveUserMap(MockUserId, MockCampaignTestKey, MockVariationName);
            mockUserProfileService.Verify(mock => mock.Save(It.IsAny<UserProfileMap>()), Times.Once);
            mockUserProfileService.Verify(mock => mock.Save(It.Is<UserProfileMap>(val => Verify(val))), Times.Once);
        }

        private bool Verify(UserProfileMap val)
        {
            if(val != null)
            {
                if(val.CampaignTestKey.Equals(MockCampaignTestKey) && val.UserId.Equals(MockUserId) && val.VariationName.Equals(MockVariationName))
                    return true;
            }
            return false;
        }

        private UserProfileMap GetUserProfileMap()
        {
            return new UserProfileMap()
            {
                CampaignTestKey = MockCampaignTestKey,
                UserId = MockUserId,
                VariationName = MockVariationName
            };
        }
    }
}
