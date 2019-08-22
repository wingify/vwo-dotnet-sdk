using Moq;

namespace VWOSdk.Tests
{
    internal class MockVariationResolver
    {
        internal static Mock<IVariationAllocator> Get()
        {
            return new Mock<IVariationAllocator>();
        }

        internal static void SetupResolve(Mock<IVariationAllocator> mockVariationResolver, Variation variation)
        {
            mockVariationResolver.Setup(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()))
                .Returns(variation);
        }
    }
}