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

        internal static Mock<IUserStorageService> GetUserStorageService()
        {
            return MockUserStorageService.Get();
        }

        internal static void SetupGet(Mock<IUserStorageService> mockUserStorageService, UserStorageMap returnValue)
        {
            MockUserStorageService.SetupGet(mockUserStorageService, returnValue);
        }

        internal static void SetupGet(Mock<IUserStorageService> mockUserStorageService, Exception exception)
        {
            MockUserStorageService.SetupGet(mockUserStorageService, exception);
        }

        internal static void SetupSet(Mock<IUserStorageService> mockUserStorageService, Exception exception)
        {
            MockUserStorageService.SetupSet(mockUserStorageService, exception);
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

        internal static void SetupIsFeatureEnabled(Mock<IValidator> mockValidator, bool returnValue)
        {
            MockValidator.SetupIsFeatureEnabled(mockValidator, returnValue);
        }

        internal static void SetupGetFeatureVariableValue(Mock<IValidator> mockValidator, bool returnValue)
        {
            MockValidator.SetupGetFeatureVariableValue(mockValidator, returnValue);
        }

        internal static void SetupPush(Mock<IValidator> mockValidator, bool returnValue)
        {
            MockValidator.SetupPush(mockValidator, returnValue);
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

        internal static Mock<ISegmentEvaluator> GetSegmentEvaluator()
        {
            return MockSegmentEvaluator.Get();
        }

        internal static void SetupResolve(Mock<ICampaignAllocator> mockCampaignResolver, BucketedCampaign allocatedCampaign, BucketedCampaign getCampaign = null)
        {
            MockCampaignAllocator.SetupResolve(mockCampaignResolver, allocatedCampaign, getCampaign);
        }

        internal static void SetupResolve(Mock<ISegmentEvaluator> mockSegmentEvaluator, bool returnValue)
        {
            MockSegmentEvaluator.SetupResolve(mockSegmentEvaluator, returnValue);
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
