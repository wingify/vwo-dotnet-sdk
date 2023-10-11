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
    public class NewBucketingFunctionalVWOTests
    {
        public NewBucketingFunctionalVWOTests()
        {
            VWO.Configure(LogLevel.DEBUG);
            VWO.Configure(logger: null);
            AppContext.Configure(new ApiCaller());
        }

        [Theory]
        [InlineData("bucket_algo_without_seed", "Ashley", "Control")]
        [InlineData("bucket_algo_without_seed", "Bill", "Control")]
        [InlineData("bucket_algo_without_seed", "Chris", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Dominic", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Emma", "Control")]
        [InlineData("bucket_algo_without_seed", "Faizan", "Control")]
        [InlineData("bucket_algo_without_seed", "Gimmy", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Harry", "Control")]
        [InlineData("bucket_algo_without_seed", "Ian", "Control")]
        [InlineData("bucket_algo_without_seed", "John", "Control")]
        [InlineData("bucket_algo_without_seed", "King", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Lisa", "Control")]
        [InlineData("bucket_algo_without_seed", "Mona", "Control")]
        [InlineData("bucket_algo_without_seed", "Nina", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Olivia", "Control")]
        [InlineData("bucket_algo_without_seed", "Pete", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Queen", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Robert", "Control")]
        [InlineData("bucket_algo_without_seed", "Sarah", "Control")]
        [InlineData("bucket_algo_without_seed", "Tierra", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Una", "Control")]
        [InlineData("bucket_algo_without_seed", "Varun", "Control")]
        [InlineData("bucket_algo_without_seed", "Will", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Xin", "Control")]
        [InlineData("bucket_algo_without_seed", "You", "Variation-1")]
        [InlineData("bucket_algo_without_seed", "Zeba", "Control")]

        [InlineData("bucket_algo_with_seed", "Ashley", "Control")]
        [InlineData("bucket_algo_with_seed", "Bill", "Control")]
        [InlineData("bucket_algo_with_seed", "Chris", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Dominic", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Emma", "Control")]
        [InlineData("bucket_algo_with_seed", "Faizan", "Control")]
        [InlineData("bucket_algo_with_seed", "Gimmy", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Harry", "Control")]
        [InlineData("bucket_algo_with_seed", "Ian", "Control")]
        [InlineData("bucket_algo_with_seed", "John", "Control")]
        [InlineData("bucket_algo_with_seed", "King", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Lisa", "Control")]
        [InlineData("bucket_algo_with_seed", "Mona", "Control")]
        [InlineData("bucket_algo_with_seed", "Nina", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Olivia", "Control")]
        [InlineData("bucket_algo_with_seed", "Pete", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Queen", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Robert", "Control")]
        [InlineData("bucket_algo_with_seed", "Sarah", "Control")]
        [InlineData("bucket_algo_with_seed", "Tierra", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Una", "Control")]
        [InlineData("bucket_algo_with_seed", "Varun", "Control")]
        [InlineData("bucket_algo_with_seed", "Will", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Xin", "Control")]
        [InlineData("bucket_algo_with_seed", "You", "Variation-1")]
        [InlineData("bucket_algo_with_seed", "Zeba", "Control")]

        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Ashley", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Bill", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Chris", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Dominic", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Emma", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Faizan", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Gimmy", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Harry", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Ian", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "John", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "King", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Lisa", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Mona", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Nina", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Olivia", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Pete", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Queen", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Robert", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Sarah", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Tierra", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Una", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Varun", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Will", "Control")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Xin", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "You", "Variation-1")]
        [InlineData("bucket_algo_with_seed_with_isNB_with_isOB", "Zeba", "Control")]


        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Ashley", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Bill", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Chris", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Dominic", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Emma", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Faizan", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Gimmy", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Harry", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Ian", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "John", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "King", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Lisa", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Mona", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Nina", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Olivia", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Pete", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Queen", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Robert", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Sarah", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Tierra", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Una", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Varun", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Will", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Xin", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "You", "Variation-1")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Zeba", "Control")]

        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Ashley", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Bill", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Chris", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Dominic", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Emma", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Faizan", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Gimmy", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Harry", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Ian", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "John", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "King", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Lisa", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Mona", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Nina", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Olivia", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Pete", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Queen", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Robert", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Sarah", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Tierra", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Una", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Varun", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Will", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Xin", "Control")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "You", "Variation-1")]
        [InlineData("bucket_algo_without_seed_with_isNB_and_without_isOB", "Zeba", "Control")]


        [InlineData("bucket_algo_with_isNB_and_without_isOB", "Ashley", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB_1", "Ashley", "Control")]
        [InlineData("bucket_algo_with_isNB_and_without_isOB_2", "Ashley", "Control")]
        
        public void NewBucketing_GetVariation_Should_Return_Desired_Output(string campaignKey, string userId, string expectedVariationName)
        {
            AppContext.Configure(new FileReaderApiCaller("SampleSettingsFileWithNewBucketing"));
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
