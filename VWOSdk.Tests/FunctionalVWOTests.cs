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

using System.Collections.Generic;
using System.Threading;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace VWOSdk.Tests
{
    public class FunctionalVWOTests
    {
        public FunctionalVWOTests()
        {
            VWO.Configure(LogLevel.DEBUG);
            VWO.Configure(logger: null);
            AppContext.Configure(new ApiCaller());
        }

        [Theory]
        [InlineData("DEV_TEST_1", "Ashley", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "Bill", true, "Control")]
        [InlineData("DEV_TEST_1", "Chris", false, null)]
        [InlineData("DEV_TEST_1", "Dominic", false, null)]
        [InlineData("DEV_TEST_1", "Emma", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "Faizan", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "Gimmy", false, null)]
        [InlineData("DEV_TEST_1", "Harry", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "Ian", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "John", true, "Control")]
        [InlineData("DEV_TEST_1", "King", false, null)]
        [InlineData("DEV_TEST_1", "Lisa", true, "Control")]
        [InlineData("DEV_TEST_1", "Mona", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "Nina", false, null)]
        [InlineData("DEV_TEST_1", "Olivia", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "Pete", false, null)]
        [InlineData("DEV_TEST_1", "Queen", false, null)]
        [InlineData("DEV_TEST_1", "Robert", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "Sarah", true, "Control")]
        [InlineData("DEV_TEST_1", "Tierra", false, null)]
        [InlineData("DEV_TEST_1", "Una", true, "Control")]
        [InlineData("DEV_TEST_1", "Varun", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "Will", false, null)]
        [InlineData("DEV_TEST_1", "Xin", true, "Variation-1")]
        [InlineData("DEV_TEST_1", "You", false, null)]
        [InlineData("DEV_TEST_1", "Zeba", true, "Variation-1")]

        [InlineData("DEV_TEST_2", "Ashley", true, "Control")]
        [InlineData("DEV_TEST_2", "Bill", true, "Control")]
        [InlineData("DEV_TEST_2", "Chris", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Dominic", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Emma", true, "Control")]
        [InlineData("DEV_TEST_2", "Faizan", true, "Control")]
        [InlineData("DEV_TEST_2", "Gimmy", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Harry", true, "Control")]
        [InlineData("DEV_TEST_2", "Ian", true, "Control")]
        [InlineData("DEV_TEST_2", "John", true, "Control")]
        [InlineData("DEV_TEST_2", "King", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Lisa", true, "Control")]
        [InlineData("DEV_TEST_2", "Mona", true, "Control")]
        [InlineData("DEV_TEST_2", "Nina", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Olivia", true, "Control")]
        [InlineData("DEV_TEST_2", "Pete", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Queen", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Robert", true, "Control")]
        [InlineData("DEV_TEST_2", "Sarah", true, "Control")]
        [InlineData("DEV_TEST_2", "Tierra", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Una", true, "Control")]
        [InlineData("DEV_TEST_2", "Varun", true, "Control")]
        [InlineData("DEV_TEST_2", "Will", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Xin", true, "Control")]
        [InlineData("DEV_TEST_2", "You", true, "Variation-1")]
        [InlineData("DEV_TEST_2", "Zeba", true, "Control")]

        [InlineData("DEV_TEST_3", "Ashley", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Bill", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Chris", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Dominic", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Emma", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Faizan", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Gimmy", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Harry", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Ian", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "John", true, "Control")]
        [InlineData("DEV_TEST_3", "King", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Lisa", true, "Control")]
        [InlineData("DEV_TEST_3", "Mona", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Nina", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Olivia", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Pete", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Queen", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Robert", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Sarah", true, "Control")]
        [InlineData("DEV_TEST_3", "Tierra", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Una", true, "Control")]
        [InlineData("DEV_TEST_3", "Varun", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Will", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Xin", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "You", true, "Variation-1")]
        [InlineData("DEV_TEST_3", "Zeba", true, "Variation-1")]

        [InlineData("DEV_TEST_4", "Ashley", false, null)]
        [InlineData("DEV_TEST_4", "Bill", false, null)]
        [InlineData("DEV_TEST_4", "Chris", false, null)]
        [InlineData("DEV_TEST_4", "Dominic", false, null)]
        [InlineData("DEV_TEST_4", "Emma", false, null)]
        [InlineData("DEV_TEST_4", "Faizan", false, null)]
        [InlineData("DEV_TEST_4", "Gimmy", false, null)]
        [InlineData("DEV_TEST_4", "Harry", false, null)]
        [InlineData("DEV_TEST_4", "Ian", false, null)]
        [InlineData("DEV_TEST_4", "John", true, "Variation-1")]
        [InlineData("DEV_TEST_4", "King", false, null)]
        [InlineData("DEV_TEST_4", "Lisa", true, "Variation-1")]
        [InlineData("DEV_TEST_4", "Mona", false, null)]
        [InlineData("DEV_TEST_4", "Nina", false, null)]
        [InlineData("DEV_TEST_4", "Olivia", false, null)]
        [InlineData("DEV_TEST_4", "Pete", false, null)]
        [InlineData("DEV_TEST_4", "Queen", false, null)]
        [InlineData("DEV_TEST_4", "Robert", false, null)]
        [InlineData("DEV_TEST_4", "Sarah", true, "Control")]
        [InlineData("DEV_TEST_4", "Tierra", false, null)]
        [InlineData("DEV_TEST_4", "Una", true, "Variation-1")]
        [InlineData("DEV_TEST_4", "Varun", false, null)]
        [InlineData("DEV_TEST_4", "Will", false, null)]
        [InlineData("DEV_TEST_4", "Xin", false, null)]
        [InlineData("DEV_TEST_4", "You", false, null)]
        [InlineData("DEV_TEST_4", "Zeba", false, null)]

        [InlineData("DEV_TEST_5", "Ashley", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Bill", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Chris", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Dominic", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Emma", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Faizan", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Gimmy", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Harry", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Ian", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "John", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "King", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Lisa", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Mona", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Nina", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Olivia", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Pete", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Queen", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Robert", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Sarah", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Tierra", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Una", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Varun", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Will", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Xin", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "You", true, "Variation-1")]
        [InlineData("DEV_TEST_5", "Zeba", true, "Variation-1")]

        [InlineData("DEV_TEST_6", "Ashley", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "Bill", true, "Control")]
        [InlineData("DEV_TEST_6", "Chris", true, "Variation-2")]
        [InlineData("DEV_TEST_6", "Dominic", true, "Variation-2")]
        [InlineData("DEV_TEST_6", "Emma", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "Faizan", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "Gimmy", true, "Variation-2")]
        [InlineData("DEV_TEST_6", "Harry", true, "Control")]
        [InlineData("DEV_TEST_6", "Ian", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "John", true, "Control")]
        [InlineData("DEV_TEST_6", "King", true, "Variation-2")]
        [InlineData("DEV_TEST_6", "Lisa", true, "Control")]
        [InlineData("DEV_TEST_6", "Mona", true, "Control")]
        [InlineData("DEV_TEST_6", "Nina", true, "Variation-2")]
        [InlineData("DEV_TEST_6", "Olivia", true, "Control")]
        [InlineData("DEV_TEST_6", "Pete", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "Queen", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "Robert", true, "Control")]
        [InlineData("DEV_TEST_6", "Sarah", true, "Control")]
        [InlineData("DEV_TEST_6", "Tierra", true, "Variation-2")]
        [InlineData("DEV_TEST_6", "Una", true, "Control")]
        [InlineData("DEV_TEST_6", "Varun", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "Will", true, "Variation-2")]
        [InlineData("DEV_TEST_6", "Xin", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "You", true, "Variation-1")]
        [InlineData("DEV_TEST_6", "Zeba", true, "Variation-1")]


        public void GetVariation_Should_Return_Desired_Output(string campaignKey, string userId, bool expectedPartOfCampaign, string expectedVariationName)
        {
            AppContext.Configure(new FileReaderApiCaller("Campaign50percVariation50-50"));
            VWO.Configure(new Validator());
            var settings = VWO.GetSettingsFile(123456, "sampleSdkKey");
            Assert.NotNull(settings);
            Assert.Equal(123456, settings.AccountId);
            Assert.Equal("sampleSdkKey", settings.SdkKey);

            var vwoClient = VWO.CreateInstance(settings, isDevelopmentMode: true);
            var getVariationResponse = vwoClient.GetVariation(campaignKey, userId);
            if (expectedPartOfCampaign)
            {
                Assert.NotNull(getVariationResponse);
                Assert.NotEmpty(getVariationResponse);
                Assert.Equal(expectedVariationName, getVariationResponse);
            }
            else
            {
                Assert.Null(expectedVariationName);
                Assert.Null(getVariationResponse);
            }

            var activateResponse = vwoClient.Activate(campaignKey, userId);
            if (expectedPartOfCampaign)
            {
                Assert.NotNull(activateResponse);
                Assert.NotEmpty(activateResponse);
                Assert.Equal(expectedVariationName, activateResponse);
            }
            else
            {
                Assert.Null(expectedVariationName);
                Assert.Null(activateResponse);
            }

            var trackResponse = vwoClient.Track(campaignKey, userId, "CUSTOM");
            Assert.Equal(expectedPartOfCampaign, trackResponse);
        }
    }
}
