﻿#pragma warning disable 1587
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

using Moq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using Xunit;
using Newtonsoft.Json;
namespace VWOSdk.Tests
{
    public class VWOClientTests
    {
        private readonly string MockCampaignKey = "MockCampaignKey";
        private readonly string MockCampaignKey1 = "MockCampaignKey1";
        private readonly string MockUserId = "MockUserId";
        private readonly string MockTagKey = "MockTagKey";
        private readonly string MockTagValue = "MockTagValue";
        private readonly string MockVariableKey = "MockVariableKey";


        private readonly Dictionary<string, dynamic> MockTrackCustomVariables = new Dictionary<string, dynamic>() {
            {"revenueValue", 0.321}
        };
        private readonly Dictionary<string, dynamic> MockTrackOptionsCustomGoal = new Dictionary<string, dynamic>() {
            {"revenueValue", 1},
            {"goalTypeToTrack", Constants.GoalTypes.CUSTOM}
        };
        private readonly Dictionary<string, dynamic> MockTrackOptionsRevenueGoal = new Dictionary<string, dynamic>() {
            {"revenueValue", 1},
            {"goalTypeToTrack", Constants.GoalTypes.REVENUE}
        };
        private readonly string MockGoalIdentifier = "MockGoalIdentifier";
        private readonly string MockVariationName = "VariationName";
        private readonly string MockSdkKey = "MockSdkKey";
        private readonly string MockLongString = "eafhsjfdrhfkvgfxkgbxfbkrekmkfdgdlkgvekrdkdfveafhsjfdrhfkvgfxkgbxfbkrekmkfdgdlkgvekrdkdfveafhsjfdrhfkvgfxkgbxfbkrekmkfdgdlkgvekrdkdfveafhsjfdrhfkvgfxkgbxfbkrekmkfdgdlkgvekrdkdfveafhsjfdrhfkvgfxkgbxfbkrekmkfdgdlkgvekrdkdfveafhsjfdrhfkvgfxkgbxfbkrekmkfdgdlkgvekrdkdfveafhsjfdrhfkvgfxkgbxfbkrekmkfdgdlkgvekrdkdfv";

        private readonly List<Dictionary<string, dynamic>> MockVariables = new List<Dictionary<string, dynamic>>() {
            new Dictionary<string, dynamic>()
            {
                {"value", "test"},
                {"type", "string"},
                {"key", "MockVariableKey"}
            }
        };

        private readonly List<Dictionary<string, dynamic>> MockVariablesInt = new List<Dictionary<string, dynamic>>() {
            new Dictionary<string, dynamic>()
            {
                {"value", "1"},
                {"type", "integer"},
                {"key", "MockVariableKey"}
            }
        };

        private readonly List<Dictionary<string, dynamic>> MockVariablesBool = new List<Dictionary<string, dynamic>>() {
            new Dictionary<string, dynamic>()
            {
                {"value", "true"},
                {"type", "boolean"},
                {"key", "MockVariableKey"}
            }
        };

        private readonly List<Dictionary<string, dynamic>> MockVariablesDouble = new List<Dictionary<string, dynamic>>() {
            new Dictionary<string, dynamic>()
            {
                {"value", "1.1"},
                {"type", "double"},
                {"key", "MockVariableKey"}
            }
        };
        private static dynamic jsonVariable = new Dictionary<string, dynamic>()
                                        {
                                            {"type", "simple"},
                                            {"name", "algorithm-testing"},
                                            {"modles", new List<string>(){"linear","sequential"}}
                                        };
        private readonly List<Dictionary<string, dynamic>> MockVariablesJson = new List<Dictionary<string, dynamic>>() {
            new Dictionary<string, dynamic>()
            {
                {"value", jsonVariable},
                {"type", "json"},
                {"key", "MockVariableKey"}
            }
        };
        private readonly List<Dictionary<string, dynamic>> MockVariablesWrongJson = new List<Dictionary<string, dynamic>>() {
            new Dictionary<string, dynamic>()
            {
                {"value", "Wrong Json"},
                {"type", "json"},
                {"key", "MockVariableKey"}
            }
        };
        private readonly Dictionary<string, dynamic> MockSegment = new Dictionary<string, dynamic>()
        {
            {
                "and", new List<Dictionary<string, dynamic>>()
                {
                    new Dictionary<string, dynamic>()
                    {
                        {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"a", "wildcard(*123*)"}
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new Dictionary<string, dynamic>()
                    {
                        {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"hello", "regex(world)"}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private readonly Dictionary<string, dynamic> MockSegmentStartsWith = new Dictionary<string, dynamic>()
        {
            {
                "and", new List<Dictionary<string, dynamic>>()
                {
                    new Dictionary<string, dynamic>()
                    {
                        {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"a", "wildcard(1234*)"}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private readonly Dictionary<string, dynamic> MockSegmentEndsWith = new Dictionary<string, dynamic>()
        {
            {
                "and", new List<Dictionary<string, dynamic>>()
                {
                    new Dictionary<string, dynamic>()
                    {
                        {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"a", "wildcard(*123)"}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private readonly Dictionary<string, dynamic> MockSegmentLower = new Dictionary<string, dynamic>()
        {
            {
                "and", new List<Dictionary<string, dynamic>>()
                {
                    new Dictionary<string, dynamic>()
                    {
                        {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"hello", "lower(WORLD)"}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private readonly Dictionary<string, dynamic> MockSegmentEquals = new Dictionary<string, dynamic>()
        {
            {
                "and", new List<Dictionary<string, dynamic>>()
                {
                    new Dictionary<string, dynamic>()
                    {
                        {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"hello", "equals(world)"}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private readonly Dictionary<string, dynamic> MockOptions = new Dictionary<string, dynamic>()
        {
            {
                "customVariables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "012345"}
                    }
            },
            {
                "revenueValue", 10.1
            }
        };
        private readonly Dictionary<string, dynamic> MockOptionsStartWith = new Dictionary<string, dynamic>()
        {
            {
                "customVariables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "12345"}
                    }
            },
            {
                "revenueValue", 10.1
            }
        };

        private readonly Dictionary<string, dynamic> MockOptionsEquals = new Dictionary<string, dynamic>()
        {
            {
                "customVariables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "12345"}
                    }
            },
            {
                "revenueValue", 10.1
            }
        };

        private readonly Dictionary<string, dynamic> MockOptionsEndWith = new Dictionary<string, dynamic>()
        {
            {
                "customVariables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "91123"}
                    }
            },
            {
                "revenueValue", 10.1
            }
        };

        private readonly Dictionary<string, dynamic> MockOptionsLower = new Dictionary<string, dynamic>()
        {
            {
                "customVariables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "91123"}
                    }
            },
            {
                "revenueValue", 10.1
            }
        };

        [Fact]
        public void Activate_Should_Return_Null_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupActivate(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.Null(result);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupGetVariation(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.GetVariation(It.Is<string>(val => MockCampaignKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupTrack(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockTrackCustomVariables);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.IsAny<string>(),
                It.Is<Dictionary<string, dynamic>>(val => MockTrackCustomVariables.Equals(val))
            ), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupIsFeatureEnabled(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockValidator.Verify(mock => mock.IsFeatureEnabled(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.IsFeatureEnabled(It.Is<string>(val => MockCampaignKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_False_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupGetFeatureVariableValue(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetFeatureVariableValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.GetFeatureVariableValue(It.Is<string>(val => MockCampaignKey.Equals(val)), It.Is<string>(val => MockVariableKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }

        [Fact]
        public void Push_Should_Return_False_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupPush(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Push(MockTagKey, MockTagValue, MockUserId);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Push(It.Is<string>(val => MockTagKey.Equals(val)), It.Is<string>(val => MockTagValue.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_CampaignResolver_Returns_Null()
        {
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            Mock.SetupResolve(mockCampaignResolver, null);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver);
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
            mockValidator.Verify(mock => mock.GetVariation(It.Is<string>(val => MockCampaignKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<Dictionary<string, object>>()), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
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
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

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
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockTrackCustomVariables);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.IsAny<string>(),
                It.Is<Dictionary<string, dynamic>>(val => MockTrackCustomVariables.Equals(val))
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_CampaignResolver_Returns_Null()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            Mock.SetupResolve(mockCampaignResolver, null);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }


        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_CampaignResolver_Returns_Null()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            Mock.SetupResolve(mockCampaignResolver, null);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

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
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.GetVariation(It.Is<string>(val => MockCampaignKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
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
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

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
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockTrackCustomVariables);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.IsAny<string>(),
                It.Is<Dictionary<string, dynamic>>(val => MockTrackCustomVariables.Equals(val))
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_VariationResolver_Returns_Null()
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
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_VariationResolver_Returns_Null()
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
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void GetVariation_Should_Return_Variation_Name_When_VariationResolver_Returns_Eligible_Variation()
        {
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockValidator.Verify(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.GetVariation(It.Is<string>(val => MockCampaignKey.Equals(val)), It.Is<string>(val => MockUserId.Equals(val)), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_VariationName_When_VariationResolver_Returns_Eligible_Variation()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_True_When_VariationResolver_Returns_Valid_Variation()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockTrackCustomVariables);
            Assert.True(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.IsAny<string>(),
                It.Is<Dictionary<string, dynamic>>(val => MockTrackCustomVariables.Equals(val))
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_True_When_VariationResolver_Returns_Eligible_Variation()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_Variable_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_Requested_Goal_Is_Revenue_Type_And_No_RevenueValue_Is_Passed()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => val == null),
                It.IsAny<Dictionary<string, dynamic>>()
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_True_When_Requested_Goal_Is_Revenue_Type_And_No_RevenueValue_Is_Passed_As_Integer()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            Dictionary<string, dynamic> revenueDict = new Dictionary<string, dynamic>() { { "revenueValue", -1 } };
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, revenueDict);
            Assert.True(result);

            int revenueValue = revenueDict["revenueValue"];
            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => val == revenueValue.ToString()),
                It.IsAny<Dictionary<string, dynamic>>()
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_True_When_Requested_Goal_Is_Revenue_Type_And_No_RevenueValue_Is_Passed_As_Float()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            Dictionary<string, dynamic> revenueDict = new Dictionary<string, dynamic>() { { "revenueValue", -1 } };
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, revenueDict);
            Assert.True(result);

            string revenueValue = revenueDict["revenueValue"].ToString();
            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.Is<string>(val => val == revenueValue),
                It.IsAny<Dictionary<string, dynamic>>()
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
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
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockTrackCustomVariables);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
            mockValidator.Verify(mock => mock.Track
            (
                It.Is<string>(val => MockCampaignKey.Equals(val)),
                It.Is<string>(val => MockUserId.Equals(val)),
                It.Is<string>(val => MockGoalIdentifier.Equals(val)),
                It.IsAny<string>(),
                It.Is<Dictionary<string, dynamic>>(val => MockTrackCustomVariables.Equals(val))
            ), Times.Once);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Null_When_Campaign_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            BucketedCampaign mockGetCampaign = null;
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, mockGetCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_Campaign_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            BucketedCampaign mockGetCampaign = null;
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, mockGetCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_Campaign_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            BucketedCampaign mockGetCampaign = null;
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, mockGetCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_Campaign_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            BucketedCampaign mockGetCampaign = null;
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, mockGetCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_Campaign_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            BucketedCampaign mockGetCampaign = null;
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, mockGetCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Null_When_Campaign_Is_Not_Running()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(null, null, "PAUSED", null);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_Campaign_Is_Not_Running()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(null, null, "PAUSED", null);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_Campaign_Is_Not_Running()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(null, null, "PAUSED", null);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_Campaign_Is_Not_Running()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(null, null, "PAUSED", null);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_Campaign_Is_Not_Running()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(null, null, "PAUSED", null);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Null_When_Campaign_Is_Not_Visual_AB()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_Campaign_Is_Feature_Rollout()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_Campaign_Is_Feature_Rollout()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_Campaign_Is_Visual_AB()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_Campaign_Is_Visual_AB()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Null_When_Segment_Evaluator_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
            Mock.SetupResolve(mockSegmentEvaluator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: mockSegmentEvaluator.Object);
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_Segment_Evaluator_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
            Mock.SetupResolve(mockSegmentEvaluator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: mockSegmentEvaluator.Object);
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_Segment_Evaluator_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
            Mock.SetupResolve(mockSegmentEvaluator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: mockSegmentEvaluator.Object);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_Segment_Evaluator_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
            Mock.SetupResolve(mockSegmentEvaluator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: mockSegmentEvaluator.Object);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_Segment_Evaluator_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
            Mock.SetupResolve(mockSegmentEvaluator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: mockSegmentEvaluator.Object);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_Campaign_Is_Feature_Test_But_Variable_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment, campaignType: Constants.CampaignTypes.FEATURE_TEST);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
            Mock.SetupResolve(mockSegmentEvaluator, true);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: mockSegmentEvaluator.Object);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variable_When_Campaign_Is_Feature_Test_And_Variable_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_TEST, mockVariables: MockVariables);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver,
                segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Equal(result, "test");

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variable_When_Campaign_Is_Feature_Test_And_Variable_Found_Int()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_TEST, mockVariables: MockVariablesInt);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation(variables: MockVariablesInt);
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Equal(result, 1);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variable_When_Campaign_Is_Feature_Test_And_Variable_Found_Bool()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_TEST, mockVariables: MockVariablesBool);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation(variables: MockVariablesBool);
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variable_When_Campaign_Is_Feature_Test_And_Variable_Found_Double()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_TEST, mockVariables: MockVariablesDouble);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation(variables: MockVariablesDouble);
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Equal(result, 1.1);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }
        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variable_When_Campaign_Is_Feature_Test_And_Variable_Found_Json()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_TEST, mockVariables: MockVariablesJson);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation(variables: MockVariablesJson);
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);          
            Assert.Equal(result, jsonVariable);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }
        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variable_When_Campaign_Is_Feature_Test_And_Variable_NotFound_Json()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment, campaignType: Constants.CampaignTypes.FEATURE_TEST, mockVariables: MockVariablesWrongJson);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation(variables: MockVariablesWrongJson);
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
            Mock.SetupResolve(mockSegmentEvaluator, true);
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: mockSegmentEvaluator.Object);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }
        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_Campaign_Is_Feature_Rollout_But_Variable_Not_Found()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            AppContext.Configure(new FileReaderApiCaller("ABCampaignWithSegment50percVariation50-50"));
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment, campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
            Mock.SetupResolve(mockSegmentEvaluator, true);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: mockSegmentEvaluator.Object);
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Null_When_PreSegmentation_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Variation_When_PreSegmentation_Passes_With_Contains()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Activate(MockCampaignKey, MockUserId, MockOptions);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Variation_When_PreSegmentation_Passes_With_Contains()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId, MockOptions);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_True_When_PreSegmentation_Passes_With_Contains()
        {

            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockOptions);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_True_When_PreSegmentation_Passes_With_Contains()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment, campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId, MockOptions);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_PreSegmentation_Passes_With_Contains()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId, MockOptions);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Variation_When_PreSegmentation_Passes_With_StartsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentStartsWith);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Activate(MockCampaignKey, MockUserId, MockOptionsStartWith);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Variation_When_PreSegmentation_Passes_With_StartsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentStartsWith);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId, MockOptionsStartWith);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_Variation_When_PreSegmentation_Passes_With_StartsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentStartsWith);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockOptionsStartWith);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_Variation_When_PreSegmentation_Passes_With_StartsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentStartsWith, campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver,
                mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId, MockOptionsStartWith);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_PreSegmentation_Passes_With_StartsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentStartsWith);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId, MockOptionsStartWith);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Variation_When_PreSegmentation_Passes_With_EndsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEndsWith);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Activate(MockCampaignKey, MockUserId, MockOptionsEndWith);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Variation_When_PreSegmentation_Passes_With_EndsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEndsWith);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId, MockOptionsEndWith);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_Variation_When_PreSegmentation_Passes_With_EndsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEndsWith);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockOptionsEndWith);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_Variation_When_PreSegmentation_Passes_With_EndsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEndsWith, campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId, MockOptionsEndWith);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variation_When_PreSegmentation_Passes_With_EndsWith()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEndsWith);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId, MockOptionsEndWith);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }


        [Fact]
        public void Activate_Should_Return_Variation_When_PreSegmentation_Passes_With_Lower()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentLower);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Activate(MockCampaignKey, MockUserId, MockOptionsLower);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Variation_When_PreSegmentation_Passes_With_Lower()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentLower);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId, MockOptionsLower);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_Variation_When_PreSegmentation_Passes_With_Lower()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentLower);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockOptionsLower);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_Variation_When_PreSegmentation_Passes_With_Lower()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentLower, campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId, MockOptionsLower);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variation_When_PreSegmentation_Passes_With_Lower()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentLower);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId, MockOptionsLower);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Activate_Should_Return_Variation_When_PreSegmentation_Passes_With_Equals()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEquals);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Activate(MockCampaignKey, MockUserId, MockOptionsEquals);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Variation_When_PreSegmentation_Passes_With_Equals()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEquals);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId, MockOptionsEquals);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_Variation_When_PreSegmentation_Passes_With_Equals()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEquals);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockOptionsEquals);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_Variation_When_PreSegmentation_Passes_With_Equals()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEquals, campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId, MockOptionsEquals);
            Assert.True(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Variation_When_PreSegmentation_Passes_With_Equals()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegmentEquals);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId, MockOptionsEquals);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetVariation_Should_Return_Null_When_PreSegmentation_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetVariation(MockCampaignKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Track_Should_Return_False_When_PreSegmentation_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_PreSegmentation_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void GetFeatureVariableValue_Should_Return_Null_When_PreSegmentation_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey, MockUserId);
            Assert.Null(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);
        }

        [Fact]
        public void Push_Should_Return_Null_When_Validation_Fails()
        {
            var mockValidator = Mock.GetValidator();
            Mock.SetupPush(mockValidator, false);

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Push(MockTagKey, MockTagValue, MockUserId);
            Assert.False(result);
        }

        [Fact]
        public void Push_Should_Return_False_When_Tag_Key_Length_Exceeds()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockTagKey = MockLongString;

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Push(mockTagKey, MockTagValue, MockUserId);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Push(It.Is<string>(val => mockTagKey.Equals(val)), It.Is<string>(val => MockTagValue.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);

        }

        [Fact]
        public void Push_Should_Return_True_When_Tag_Key_Length_Does_Not_Exceeds()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockTagKey = "test";

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Push(mockTagKey, MockTagValue, MockUserId);
            Assert.True(result);

            mockValidator.Verify(mock => mock.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Push(It.Is<string>(val => mockTagKey.Equals(val)), It.Is<string>(val => MockTagValue.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void Push_Should_Return_False_When_Tag_Value_Length_Exceeds()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockTagValue = MockLongString;

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Push(MockTagKey, mockTagValue, MockUserId);
            Assert.False(result);

            mockValidator.Verify(mock => mock.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Push(It.Is<string>(val => MockTagKey.Equals(val)), It.Is<string>(val => mockTagValue.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }


        [Fact]
        public void Push_Should_Return_True_When_Tag_Value_Length_Does_Not_Exceeds()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockTagKey = "test";

            var vwoClient = GetVwoClient(mockValidator: mockValidator);
            var result = vwoClient.Push(mockTagKey, MockTagValue, MockUserId);
            Assert.True(result);

            mockValidator.Verify(mock => mock.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Push(It.Is<string>(val => mockTagKey.Equals(val)), It.Is<string>(val => MockTagValue.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_Segments_Are_Passed_But_customVariables_Are_Not_Passed()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(segments: MockSegment, campaignType: Constants.CampaignTypes.FEATURE_ROLLOUT);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_Assigned_Variation_Has_FeatureEnabled_False()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_TEST);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.IsFeatureEnabled(MockCampaignKey, MockUserId);
            Assert.False(result);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_True_When_Assigned_Variation_Has_FeatureEnabled_True()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(campaignType: Constants.CampaignTypes.FEATURE_TEST, status: Constants.CampaignStatus.RUNNING);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation(IsFeatureEnabled: true);
            Mock.SetupResolve(mockVariationResolver, selectedVariation);

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.IsFeatureEnabled("x", MockUserId);
            Assert.True(result);
            mockValidator.Verify(mock => mock.IsFeatureEnabled(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);          
        }

        // Unique Goal Conversion + Multiple Campaign Key Test cases

        [Fact]
        public void Track_Should_Fire_For_Revenue_Goals_Only_When_Revenue_Passed()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            var otherCampaign = GetCampaign(campaignKey: MockCampaignKey1, goalType: Constants.GoalTypes.CUSTOM);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign, otherCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(settingType: "MultipleCampaignForTrack", mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockUserId, MockGoalIdentifier);
            Assert.False(result[MockCampaignKey]);
            Assert.True(result[MockCampaignKey1]);

            var resultRevenue = vwoClient.Track(MockUserId, MockGoalIdentifier, MockTrackCustomVariables);
            Assert.True(resultRevenue[MockCampaignKey]);
            Assert.True(resultRevenue[MockCampaignKey1]);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.AtLeastOnce);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.AtLeastOnce);
        }

        [Fact]
        public void Track_Should_Fire_Only_When_GoalType_Is_Custom()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(goalType: Constants.GoalTypes.CUSTOM);
            var otherCampaign = GetCampaign(campaignKey: MockCampaignKey1, goalType: Constants.GoalTypes.CUSTOM);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign, otherCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(settingType: "GoalTypeCustom", mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockUserId, MockGoalIdentifier, MockTrackOptionsCustomGoal);
            Assert.True(result[MockCampaignKey]);
            Assert.True(result[MockCampaignKey1]);

            var resultRevenue = vwoClient.Track(MockUserId, MockGoalIdentifier, MockTrackOptionsRevenueGoal);
            Assert.Null(resultRevenue);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.AtLeastOnce);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.AtLeastOnce);
        }

        [Fact]
        public void Track_Should_Fire_Only_When_GoalType_Is_Revenue()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(goalType: Constants.GoalTypes.REVENUE);
            var otherCampaign = GetCampaign(campaignKey: MockCampaignKey1, goalType: Constants.GoalTypes.REVENUE);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign, otherCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(settingType: "GoalTypeRevenue", mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockUserId, MockGoalIdentifier, MockTrackOptionsCustomGoal);
            Assert.Null(result);

            var resultRevenue = vwoClient.Track(MockUserId, MockGoalIdentifier, MockTrackOptionsRevenueGoal);
            Assert.True(resultRevenue[MockCampaignKey]);
            Assert.True(resultRevenue[MockCampaignKey1]);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.AtLeastOnce);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.AtLeastOnce);
        }

        [Fact]
        public void Track_Should_Fire_For_Selected_Campaigns()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            var otherCampaign = GetCampaign(campaignKey: MockCampaignKey1, goalType: Constants.GoalTypes.CUSTOM);
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign, otherCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(settingType: "MultipleCampaignForTrack", mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(new List<string>() { MockCampaignKey, MockCampaignKey1 }, MockUserId, MockGoalIdentifier);
            Assert.False(result[MockCampaignKey]);
            Assert.True(result[MockCampaignKey1]);

            var resultRevenue = vwoClient.Track(new List<string>() { MockCampaignKey, MockCampaignKey1 }, MockUserId, MockGoalIdentifier, MockTrackCustomVariables);
            Assert.True(resultRevenue[MockCampaignKey]);
            Assert.True(resultRevenue[MockCampaignKey1]);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.AtLeastOnce);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.AtLeastOnce);
        }
        [Fact]
        public void Track_Should_Not_Fire_For_Disallowed_Goal_Type()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign(goalType: "NON_EXISTENT_GOAL_TYPE");
            var otherCampaign = GetCampaign(campaignKey: MockCampaignKey1, goalType: "NON_EXISTENT_GOAL_TYPE");
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign, otherCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            var vwoClient = GetVwoClient(settingType: "NonExistentGoalType", mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(new List<string>() { MockCampaignKey, MockCampaignKey1 }, MockUserId, MockGoalIdentifier,
                MockTrackOptionsCustomGoal);
            Assert.False(result[MockCampaignKey]);
            Assert.False(result[MockCampaignKey1]);

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.AtLeastOnce);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.AtLeastOnce);
        }


        #region Event Batching
        internal class FlushCallback : IFlushInterface
        {
            public void onFlush(string error, object events)
            {
                var jsonBytes = events.ToString();
                var jsonDoc = JsonDocument.Parse(jsonBytes);
                var root = jsonDoc.RootElement;
                var myEventList = root.GetProperty("ev");
                var count = myEventList.GetArrayLength();
                Assert.Equal(1, count);
            }
        }

        [Fact]
        public void Activate_EventBatching_Queue_Size_Tests()
        {
            // "Event Batching: enqueue should queue an event, flushEvents should flush queue"
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            BatchEventData batchData = new BatchEventData();
            batchData.EventsPerRequest = 4;
            batchData.RequestTimeInterval = 20;
            batchData.FlushCallback = new FlushCallback(); //Callback
            var vwoClient = GetVwoClient(null, mockValidator: mockValidator,
                mockCampaignResolver: mockCampaignResolver,
                mockVariationResolver: mockVariationResolver, null, null, batchData);
            var result = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.NotNull(result);
            Assert.Equal(MockVariationName, result);
            Assert.NotNull(result);
            Assert.Equal(1, vwoClient.getBatchEventQueue().BatchQueueCount());
            vwoClient.getBatchEventQueue().flush(true);
            Assert.Equal(0, vwoClient.getBatchEventQueue().BatchQueueCount());

            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.IsAny<string>()), Times.Once);
            mockCampaignResolver.Verify(mock => mock.GetCampaign(It.IsAny<AccountSettings>(), It.Is<string>(val => MockCampaignKey.Equals(val))), Times.Once);

        }

        [Fact]
        public void FlushQueueOnMaxEventsTest()
        {
            //"Event Batching: queue should be flushed if eventsPerRequest is reached"
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            BatchEventData batchData = new BatchEventData();
            batchData.EventsPerRequest = 2;
            batchData.RequestTimeInterval = 20;
            batchData.FlushCallback = new FlushCallback(); //Callback

            var vwoClient = GetVwoClient(null, mockValidator: mockValidator,
                mockCampaignResolver: mockCampaignResolver,
                mockVariationResolver: mockVariationResolver, null, null, batchData);

            var result1 = vwoClient.Activate(MockCampaignKey, MockUserId);

            Assert.NotNull(result1);


            Assert.Equal(1, vwoClient.getBatchEventQueue().BatchQueueCount());

            var result2 = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockOptionsLower);
            Assert.True(result2);
            Thread.Sleep(2000);
            Assert.Equal(0, vwoClient.getBatchEventQueue().BatchQueueCount());


        }

        [Fact]
        public void FlushQueueOnTimerExpiredTest()
        {

            //"Event Batching: queue should be flushed if requestTimeInterval is reached"
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            BatchEventData batchData = new BatchEventData();
            batchData.EventsPerRequest = 5;
            batchData.RequestTimeInterval = 10;
            batchData.FlushCallback = new FlushCallback(); //Callback

            var vwoClient = GetVwoClient(null, mockValidator: mockValidator,
                mockCampaignResolver: mockCampaignResolver,
                mockVariationResolver: mockVariationResolver, null, null, batchData);
            var result1 = vwoClient.Activate(MockCampaignKey, MockUserId);

            Assert.NotNull(result1);
            Assert.Equal(1, vwoClient.getBatchEventQueue().BatchQueueCount());

            var result2 = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, MockOptionsLower);
            Assert.True(result2);
            Assert.Equal(2, vwoClient.getBatchEventQueue().BatchQueueCount());
            Thread.Sleep(12000);
            Assert.Equal(0, vwoClient.getBatchEventQueue().BatchQueueCount());

        }

        [Fact]
        public void FlushEventsAPITest()
        {

            //"Event Batching: enqueue should queue an event, flushEvents should flush queue"
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            var selectedVariation = GetVariation();
            Mock.SetupResolve(mockVariationResolver, selectedVariation);
            BatchEventData batchData = new BatchEventData();
            batchData.EventsPerRequest = 5;
            batchData.RequestTimeInterval = 10;
            batchData.FlushCallback = new FlushCallback(); //Callback

            var vwoClient = GetVwoClient(null, mockValidator: mockValidator,
                mockCampaignResolver: mockCampaignResolver,
                mockVariationResolver: mockVariationResolver, null, null, batchData);
            var result1 = vwoClient.Activate(MockCampaignKey, MockUserId);
            Assert.NotNull(result1);
            Assert.Equal(1, vwoClient.getBatchEventQueue().BatchQueueCount());

            bool isQueueFlushed = vwoClient.FlushEvents();
            Assert.True(isQueueFlushed);

            Assert.Equal(0, vwoClient.getBatchEventQueue().BatchQueueCount());
        }



        #endregion



        private bool VerifyTrackUserVerb(ApiRequest apiRequest)
        {
            if (apiRequest != null)
            {
                var url = apiRequest.Uri.ToString().ToLower();
                if (url.Contains("track-user") && url.Contains("experiment_id =-1") && url.Contains("combination=-2"))
                {
                    return true;
                }
            }
            return false;
        }

        private IVWOClient GetVwoClient(string settingType = null, Mock<IValidator> mockValidator = null,
            Mock<ICampaignAllocator> mockCampaignResolver = null, Mock<IVariationAllocator> mockVariationResolver = null,
            ISegmentEvaluator segmentEvaluator = null, Mock<IUserStorageService> mockUserStorageService = null,
            BatchEventData EventBatching = null)
        {
            mockValidator = mockValidator ?? Mock.GetValidator();
            if (mockCampaignResolver == null)
            {
                mockCampaignResolver = Mock.GetCampaignAllocator();
                Mock.SetupResolve(mockCampaignResolver, GetCampaign());
            }

            if (mockVariationResolver == null)
            {
                mockVariationResolver = Mock.GetVariationResolver();
                Mock.SetupResolve(mockVariationResolver, GetVariation());
            }

            if (segmentEvaluator == null)
            {
                var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
                Mock.SetupResolve(mockSegmentEvaluator, true);
                segmentEvaluator = mockSegmentEvaluator.Object;
            }

            var mockUserStorageServiceObject = mockUserStorageService == null ? null : mockUserStorageService.Object;

            var mockEventBatching = EventBatching == null ? null : EventBatching;
            //changed
            //  BatchEventData batchData = VWOCore.Controllers.EventBatchDataProvider.BatchEventData();
            return new VWO(GetSettings(settingType), mockValidator.Object, mockUserStorageServiceObject, mockCampaignResolver.Object,
                segmentEvaluator, mockVariationResolver.Object, false, mockEventBatching);
        }

        private AccountSettings GetSettings(string settingType = null)
        {
            return new AccountSettings(MockSdkKey, GetCampaigns(settingType), 123456, 1,null,null);
        }

        private List<BucketedCampaign> GetCampaigns(string settingType = null, string status = "running")
        {
            var result = new List<BucketedCampaign>();
            if (settingType == "NonExistentGoalType")
            {
                result.Add(GetCampaign(campaignKey: MockCampaignKey, status: status, goalType: "NON_EXISTENT_GOAL_TYPE"));
                result.Add(GetCampaign(campaignKey: MockCampaignKey1, status: status, goalType: "NON_EXISTENT_GOAL_TYPE"));
                return result;
            }
            if (settingType == "GoalTypeCustom")
            {
                result.Add(GetCampaign(campaignKey: MockCampaignKey, status: status, goalType: Constants.GoalTypes.CUSTOM));
                result.Add(GetCampaign(campaignKey: MockCampaignKey1, status: status, goalType: Constants.GoalTypes.CUSTOM));
                return result;
            }
            if (settingType == "GoalTypeRevenue")
            {
                result.Add(GetCampaign(campaignKey: MockCampaignKey, status: status, goalType: Constants.GoalTypes.REVENUE));
                result.Add(GetCampaign(campaignKey: MockCampaignKey1, status: status, goalType: Constants.GoalTypes.REVENUE));
                return result;
            }
            result.Add(GetCampaign(status: status));
            if (settingType == "MultipleCampaignForTrack")
            {
                result.Add(GetCampaign(campaignKey: MockCampaignKey1, status: status, goalType: Constants.GoalTypes.CUSTOM));
            }
            return result;
        }

        private BucketedCampaign GetCampaign(string campaignKey = null, string variationName = null, string status = "RUNNING", string goalIdentifier = null, string campaignType = null, Dictionary<string, dynamic> segments = null, List<Dictionary<string, dynamic>> mockVariables = null, string goalType = null,bool isBucketingSeed=false)
        {
            campaignKey = campaignKey ?? MockCampaignKey;
            return new BucketedCampaign(-1, "test", 100, campaignKey, status, campaignType != null ? campaignType : Constants.CampaignTypes.VISUAL_AB, false, isBucketingSeed, segments, mockVariables)
            {
                Variations = GetVariations(variationName, mockVariables),
                Goals = GetGoals(goalIdentifier, goalType)
            };
        }
        //private BucketedCampaign GetBucketingSeedCampaign(string campaignKey = null, string variationName = null, string status = "RUNNING", string goalIdentifier = null, string campaignType = null, Dictionary<string, dynamic> segments = null, List<Dictionary<string, dynamic>> mockVariables = null, string goalType = null)
        //{
        //    campaignKey = campaignKey ?? MockCampaignKey;
        //    return new BucketedCampaign(-1, "test", 100, campaignKey, status, campaignType != null ? campaignType : Constants.CampaignTypes.VISUAL_AB, false, false, segments, mockVariables)
        //    {
        //        Variations = GetVariations(variationName, mockVariables),
        //        Goals = GetGoals(goalIdentifier, goalType)
        //    };
        //}
        private Dictionary<string, Goal> GetGoals(string goalIdentifier = null, string goalType = null)
        {
            goalIdentifier = goalIdentifier ?? MockGoalIdentifier;
            return new Dictionary<string, Goal>() { { goalIdentifier, GetGoal(goalType) } };
        }

        private Goal GetGoal(string type = null)
        {
            type = type ?? Constants.GoalTypes.REVENUE;
            return new Goal(-3, MockGoalIdentifier, type);
        }

        private RangeBucket<Variation> GetVariations(string variationName = null, List<Dictionary<string, dynamic>> variables = null)
        {
            var result = new RangeBucket<Variation>(10000);
            variables = variables == null ? MockVariables : variables;
            result.Add(1, new Variation(1, "Control", null, 100, false, variables));
            result.Add(100, GetVariation(variationName));
            return result;
        }

        private Variation GetVariation(string variationName = null, List<Dictionary<string, dynamic>> variables = null, bool IsFeatureEnabled = false)
        {
            variationName = variationName ?? MockVariationName;
            variables = variables == null ? MockVariables : variables;
            return new Variation(-2, variationName, null, 100, IsFeatureEnabled, variables);
        }

        private UserStorageMap GetUserStorageMap(string goalIdentifier = null, string campaignKey = null)
        {
            return new UserStorageMap()
            {
                CampaignKey = campaignKey ?? MockCampaignKey,
                UserId = MockUserId,
                VariationName = MockVariationName,
                GoalIdentifier = goalIdentifier
            };
        }
    }
}
