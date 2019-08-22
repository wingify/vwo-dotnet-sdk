using Moq;
using System;

namespace VWOSdk.Tests
{
    public class Mock
    {
        internal static Mock<IBucketService> GetUserHasher()
        {
            return MockUserHasher.Get();
        }

        internal static void SetupCompute(Mock<IBucketService> mockUserHasher, int returnVal)
        {
            MockUserHasher.SetupCompute(mockUserHasher, returnVal);
        }

        internal static void SetupComputeBucketValue(Mock<IBucketService> mockUserHasher, int returnVal, double outHashValue)
        {
            MockUserHasher.SetupComputeBucketValue(mockUserHasher, returnVal, outHashValue);
        }

        internal static Mock<IUserProfileService> GetUserProfileService()
        {
            return MockUserProfileService.Get();
        }

        internal static void SetupLookup(Mock<IUserProfileService> mockUserProfileService, UserProfileMap returnValue)
        {
            MockUserProfileService.SetupLookup(mockUserProfileService, returnValue);
        }

        internal static void SetupLookup(Mock<IUserProfileService> mockUserProfileService, Exception exception)
        {
            MockUserProfileService.SetupLookup(mockUserProfileService, exception);
        }

        internal static void SetupSave(Mock<IUserProfileService> mockUserProfileService, Exception exception)
        {
            MockUserProfileService.SetupSave(mockUserProfileService, exception);
        }

        internal static Mock<IApiCaller> GetApiCaller<T>(IApiCaller innerApiCaller = null)
        {
            return MockApiCaller.Get<T>(innerApiCaller);
        }

        internal static void SetupExecute<T>(Mock<IApiCaller> mockApiCaller, T returnValue = default(T))
        {
            MockApiCaller.SetupExecute<T>(mockApiCaller, returnValue);
        }

        internal static Mock<IValidator> GetValidator()
        {
            return MockValidator.Get();
        }
        internal static void SetupGetSettings(Mock<IValidator> mockValidator, bool returnValue)
        {
            MockValidator.SetupGetSettings(mockValidator, returnValue);
        }

        internal static void SetupActivate(Mock<IValidator> mockValidator, bool returnValue)
        {
            MockValidator.SetupActivate(mockValidator, returnValue);
        }

        internal static void SetupGetVariation(Mock<IValidator> mockValidator, bool returnValue)
        {
            MockValidator.SetupGetVariation(mockValidator, returnValue);
        }

        internal static void SetupTrack(Mock<IValidator> mockValidator, bool returnValue)
        {
            MockValidator.SetupTrack(mockValidator, returnValue);
        }

        internal static Mock<ISettingsProcessor> GetSettingsProcessor()
        {
            return MockSettingsProcessor.Get();
        }

        internal static void SetupProcessAndBucket(Mock<ISettingsProcessor> mockSettingsProcessor, AccountSettings returnValue)
        {
            MockSettingsProcessor.SetupProcessAndBucket(mockSettingsProcessor, returnValue);
        }

        internal static Mock<ICampaignAllocator> GetCampaignAllocator()
        {
            return MockCampaignAllocator.Get();
        }

        internal static void SetupResolve(Mock<ICampaignAllocator> mockCampaignResolver, BucketedCampaign returnValue)
        {
            MockCampaignAllocator.SetupResolve(mockCampaignResolver, returnValue);
        }

        internal static Mock<IVariationAllocator> GetVariationResolver()
        {
            return MockVariationResolver.Get();
        }

        internal static void SetupResolve(Mock<IVariationAllocator> mockVariationResolver, Variation variation)
        {
            MockVariationResolver.SetupResolve(mockVariationResolver, variation);
        }

        internal static void SetupSettingsFile(Mock<IValidator> mockValidator, bool returnValue)
        {
            MockValidator.SetupSettingsFile(mockValidator, returnValue);
        }
    }
}
