using Moq;

namespace VWOSdk.Tests
{
    internal class MockCampaignAllocator
    {
        internal static Mock<ICampaignAllocator> Get()
        {
            return new Mock<ICampaignAllocator>();
        }

        internal static void SetupResolve(Mock<ICampaignAllocator> mockCampaignResolver, BucketedCampaign returnValue)
        {
            mockCampaignResolver.Setup(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(returnValue);
        }
    }
}