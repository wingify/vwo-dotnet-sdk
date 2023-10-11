#pragma warning disable 1587
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

using System.Collections.Generic;
using System.Threading;
using Xunit;
using System;
namespace VWOSdk.Tests
{
    public class NewBucketingV2FunctionalVWOTests
    {
        public NewBucketingV2FunctionalVWOTests()
        {
            VWO.Configure(LogLevel.DEBUG);
            VWO.Configure(logger: null);
            AppContext.Configure(new ApiCaller());
        }

        [Theory]
        [InlineData("bucket_algo_with_isNB_with_isNBv2", "Ashley1", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_with_isNBv2_1", "Ashley1", "Control")]
        [InlineData("bucket_algo_with_isNB_with_isNBv2_2", "Ashley1", "Control")]
        

        public void NewBucketingV2_GetVariation_Should_Return_Desired_Output(string campaignKey, string userId, string expectedVariationName)
        {
            AppContext.Configure(new FileReaderApiCaller("SampleSettingsFileWithNBv2"));
            VWO.Configure(new Validator());
            var settings = VWO.GetSettingsFile(12345, "someuniquestuff1234567");
            Assert.NotNull(settings);
            Assert.Equal(12345, settings.AccountId);
            Assert.Equal("someuniquestuff1234567", settings.SdkKey);
            var vwoClient = VWO.Launch(settings, isDevelopmentMode: true);
            var getVariationResponse = vwoClient.GetVariation(campaignKey, userId);
            Assert.NotNull(getVariationResponse);
            Assert.NotEmpty(getVariationResponse);
            Assert.Equal(expectedVariationName, getVariationResponse);
        }
    }
}
