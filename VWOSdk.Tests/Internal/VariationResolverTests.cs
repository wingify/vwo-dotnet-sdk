using Moq;
using Xunit;

namespace VWOSdk.Tests
{
    public class VariationAllocatorTests
    {
        private readonly string MockUserId = "MockUserId";
        private readonly string MockCampaignTestKey = "MockCampaignTestKey";
        private readonly string MockVariationName = "VariationName";

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
        public void Allocate_Should_Compute_Hash_When_UserProfileMap_Is_Null()
        {
            var mockUserHasher = Mock.GetUserHasher();
            Mock.SetupComputeBucketValue(mockUserHasher, returnVal: 10, outHashValue: 1234567);
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);
            UserProfileMap userProfileMap = null;
            var selectedVariation = variationResolver.Allocate(userProfileMap, GetCampaign(MockCampaignTestKey), MockUserId);
            Assert.NotNull(selectedVariation);
            Assert.Equal(MockVariationName, selectedVariation.Name);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.Is<string>((val) => MockUserId.Equals(val)), It.Is<double>((val) => 10000 == val), It.Is<double>(val => 1 == val)), Times.Once);
        }

        [Fact]
        public void Allocate_Should_Not_Compute_Hash_When_Valid_UserProfileMap_With_Valid_Variation_Is_Given()
        {
            var mockUserHasher = MockUserHasher.Get();
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);
            UserProfileMap userProfileMap = new UserProfileMap(MockUserId, MockCampaignTestKey, MockVariationName);
            var selectedVariation = variationResolver.Allocate(userProfileMap, GetCampaign(MockCampaignTestKey), MockUserId);
            Assert.NotNull(selectedVariation);
            Assert.Equal(MockVariationName, selectedVariation.Name);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void Allocate_Should_Return_Null_When_Valid_UserProfileMap_With_InValid_Variation_Is_Given()
        {
            var mockUserHasher = MockUserHasher.Get();
            VariationAllocator variationResolver = GetVariationResolver(mockUserHasher);
            UserProfileMap userProfileMap = new UserProfileMap(MockUserId, MockCampaignTestKey, MockVariationName + MockVariationName);
            var selectedVariation = variationResolver.Allocate(userProfileMap, GetCampaign(MockCampaignTestKey), MockUserId);
            Assert.Null(selectedVariation);

            mockUserHasher.Verify(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        private BucketedCampaign GetCampaign(string campaignTestKey = null, string variationName = null)
        {
            campaignTestKey = campaignTestKey ?? MockCampaignTestKey;
            variationName = variationName ?? MockVariationName;
            return new BucketedCampaign(1, 100, campaignTestKey, null, null)
            {
                Variations = GetVariations(variationName),
            };
        }

        private RangeBucket<Variation> GetVariations(string variationName)
        {
            var result = new RangeBucket<Variation>(10000);
            result.Add(100, new Variation(1, variationName, null, 100));
            return result;
        }

        private VariationAllocator GetVariationResolver(Mock<IBucketService> mockUserHasher = null)
        {
            mockUserHasher = mockUserHasher ?? MockUserHasher.Get();
            return new VariationAllocator(mockUserHasher.Object);
        }
    }
}
