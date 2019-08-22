using System.Collections.Generic;
using Moq;
using Xunit;

namespace VWOSdk.Tests
{
    public class VWOClientTests
    {
        private readonly string MockCampaignTestKey = "MockCampaignTestKey";
        private readonly string MockUserId = "MockUserId";
        private readonly string MockRevenueValue = "0.321";
        private readonly string MockGoalIdentifier = "MockGoalIdentifier";
        private readonly string MockVariationName = "VariationName";
        private readonly string MockSdkKey = "MockSdkKey";

        [Fact]
        public void Activate_Should_Return_Null_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupActivate(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Activate(MockCampaignTestKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.Activate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Activate(It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupGetVariation(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.GetVariation(MockCampaignTestKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.GetVariation(It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupTrack(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Track(MockCampaignTestKey, MockUserId, MockGoalIdentifier, MockRevenueValue);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignTestKey.Equals(val)), 
                It.Is<string>(val => MockUserId.Equals(val)), 
                It.Is<string>(val => MockGoalIdentifier.Equals(val)), 
                It.Is<string>(val => MockRevenueValue.Equals(val))
            ), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_CampaignResolver_Returns_Null()
        {
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            Mock.SetupResolve(mockCampaignResolver, null);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver);
            var result = vwoClient.GetVariation(MockCampaignTestKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.GetVariation(It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Null_When_CampaignResolver_Returns_Null()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            Mock.SetupResolve(mockCampaignResolver, null);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver);
            var result = vwoClient.Activate(MockCampaignTestKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.Activate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Activate(It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void Track_Should_Return_False_When_CampaignResolver_Returns_Null()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            Mock.SetupResolve(mockCampaignResolver, null);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver);
            var result = vwoClient.Track(MockCampaignTestKey, MockUserId, MockGoalIdentifier, MockRevenueValue);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignTestKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => MockRevenueValue.Equals(val))
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_VariationResolver_Returns_Null()
        {
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, null);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetVariation(MockCampaignTestKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.GetVariation(It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign ,val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Null_When_VariationResolver_Returns_Null()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, null);
            
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Activate(MockCampaignTestKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.Activate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Activate(It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void Track_Should_Return_False_When_VariationResolver_Returns_Null()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, null);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignTestKey, MockUserId, MockGoalIdentifier, MockRevenueValue);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignTestKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => MockRevenueValue.Equals(val))
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
            
            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void GetVariation_Should_Return_Variation_Name_When_VariationResolver_Returns_Eligible_Variation()
        {
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetVariation(MockCampaignTestKey, MockUserId);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockValidator.Verify(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.GetVariation(It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_VariationName_When_VariationResolver_Returns_Eligible_Variation()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Activate(MockCampaignTestKey, MockUserId);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockValidator.Verify(mock => mock.Activate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Activate(It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_True_When_VariationResolver_Returns_Valid_Variation()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignTestKey, MockUserId, MockGoalIdentifier, MockRevenueValue);
            Assert.True(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignTestKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => MockRevenueValue.Equals(val))
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_Requested_Goal_Is_Revenue_Type_And_No_Revenue_Value_Is_Passed()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignTestKey, MockUserId, MockGoalIdentifier);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignTestKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => val == null)
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_True_When_Requested_Goal_Is_Revenue_Type_And_No_Revenue_Value_Is_Passed_As_Integer()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            int revenueValue = -1;
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignTestKey, MockUserId, MockGoalIdentifier, revenueValue);
            Assert.True(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignTestKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => val == revenueValue.ToString())
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_True_When_Requested_Goal_Is_Revenue_Type_And_No_Revenue_Value_Is_Passed_As_Float()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            float revenueValue = -1;
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignTestKey, MockUserId, MockGoalIdentifier, revenueValue);
            Assert.True(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignTestKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => val == revenueValue.ToString())
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_VariationResolver_Returns_Valid_Variation_And_Requested_Goal_Identifier_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(goalIdentifier: MockGoalIdentifier + MockGoalIdentifier);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignTestKey, MockUserId, MockGoalIdentifier, MockRevenueValue);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignTestKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => MockRevenueValue.Equals(val))
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.Allocate(It.IsAny<AccountSettings>(), It.IsAny<UserProfileMap>(), It.Is<string>(val => MockCampaignTestKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<string>()), Times.Once);

            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.IsAny<BucketedCampaign>(), It.IsAny<string>()), Times.Once);
            mockVariationResolver.Verify(mock => mock.Allocate(It.IsAny<UserProfileMap>(), It.Is<BucketedCampaign>(val => ReferenceEquals(selectedCampaign, val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        private bool VerifyTrackUserVerb(ApiRequest apiRequest)
        {
            if(apiRequest != null)
            {
                var url = apiRequest.Uri.ToString().ToLower();
                if(url.Contains("track-user") && url.Contains("experiment_id =-1") && url.Contains("combination=-2"))
                {
                    return true;
                }
            }
            return false;
        }

        private IVWOClient GetVwoClient(Mock<IValidator> mockValidator = null, Mock<ICampaignAllocator> mockCampaignResolver = null, Mock<IVariationAllocator> mockVariationResolver = null)
        {
            mockValidator = mockValidator ?? Mock.GetValidator();
            if (mockCampaignResolver == null)
            {
                mockCampaignResolver = Mock.GetCampaignAllocator();
                Mock.SetupResolve(mockCampaignResolver, GetCampaign());
            }

            if(mockVariationResolver == null)
            {
                mockVariationResolver = Mock.GetVariationResolver();
                Mock.SetupResolve(mockVariationResolver, GetVariation());
            }
            return new VWO(GetSettings(), mockValidator.Object, null, mockCampaignResolver.Object, mockVariationResolver.Object, true);
        }

        private AccountSettings GetSettings()
        {
            return new AccountSettings(MockSdkKey, GetCampaigns(), 123456, 1);
        }

        private List<BucketedCampaign> GetCampaigns(string status = "running")
        {
            var result = new List<BucketedCampaign>();
            result.Add(GetCampaign(status: status));
            return result;
        }

        private BucketedCampaign GetCampaign(string campaignTestKey = null, string variationName = null, string status = "running", string goalIdentifier = null)
        {
            campaignTestKey = campaignTestKey ?? MockCampaignTestKey;
            return new BucketedCampaign(-1, 100, campaignTestKey, status, null)
            {
                Variations = GetVariations(variationName),
                Goals = GetGoals(goalIdentifier)
            };
        }

        private Dictionary<string, Goal> GetGoals(string goalIdentifier = null)
        {
            goalIdentifier = goalIdentifier ?? MockGoalIdentifier;
            return new Dictionary<string, Goal>() { { goalIdentifier, GetGoal() } };
        }

        private Goal GetGoal()
        {
            return new Goal(-3, MockGoalIdentifier, "REVENUE_TRACKING");
        }

        private RangeBucket<Variation> GetVariations(string variationName = null)
        {
            var result = new RangeBucket<Variation>(10000);
            result.Add(100, GetVariation(variationName));
            return result;
        }

        private Variation GetVariation(string variationName = null)
        {
            variationName = variationName ?? MockVariationName;
            return new Variation(-2, variationName, null, 100);
        }
    }
}
