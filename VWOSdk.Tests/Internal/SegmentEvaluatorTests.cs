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

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xunit;

namespace VWOSdk.Tests
{
    public class SegmentEvaluatorTests
    {
        private static readonly Dictionary<string, dynamic> TestExpectations = new FileReaderApiCaller("SegmentExpectations").GetJsonContent<Dictionary<string, dynamic>>();

        [Fact]
        public void Segment_Evaluator_Should_Match_All_Expectations()
        {
            foreach(KeyValuePair<string, dynamic> testCaseGroup in TestExpectations) {
                Dictionary<string, dynamic> testCase = JObject.FromObject(testCaseGroup.Value).ToObject<Dictionary<string, dynamic>>();
                foreach(KeyValuePair<string, dynamic> testCaseContent in testCase) {
                    Dictionary<string, dynamic> segments = JObject.FromObject(testCaseContent.Value["dsl"]).ToObject<Dictionary<string, dynamic>>();
                    Dictionary<string, dynamic> customVariables = testCaseContent.Value.ContainsKey("custom_variables") ? JObject.FromObject(testCaseContent.Value["custom_variables"]).ToObject<Dictionary<string, dynamic>>() : null;
                    Dictionary<string, dynamic> variationTargetingVariables = testCaseContent.Value.ContainsKey("variation_targeting_variables") ? JObject.FromObject(testCaseContent.Value["variation_targeting_variables"]).ToObject<Dictionary<string, dynamic>>() : null;
                    if (variationTargetingVariables != null)
                    {
                        customVariables = variationTargetingVariables;
                    }
                    bool expectation = testCaseContent.Value["expectation"];
                    bool result = new SegmentEvaluator().evaluate("user", "dummyCampaign", Constants.SegmentationType.PRE_SEGMENTATION, segments, customVariables);
                    Assert.Equal(result, expectation);
                }
            }
        }
    }
}
