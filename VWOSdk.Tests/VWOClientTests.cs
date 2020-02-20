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
using Moq;
using Xunit;

namespace VWOSdk.Tests
{
    public class VWOClientTests
    {
        private readonly string MockCampaignKey = "MockCampaignKey";
        private readonly string MockUserId = "MockUserId";
        private readonly string MockTagKey = "MockTagKey";
        private readonly string MockTagValue = "MockTagValue";
        private readonly string MockVariableKey = "MockVariableKey";
        private readonly Dictionary<string, dynamic> MockTrackCustomVariables = new Dictionary<string, dynamic>() {
            {"revenue_value", 0.321}
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
                "custom_variables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "012345"}
                    }
            },
            {
                "revenue_value", 10.1
            }
        };
        private readonly Dictionary<string, dynamic> MockOptionsStartWith = new Dictionary<string, dynamic>()
        {
            {
                "custom_variables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "12345"}
                    }
            },
            {
                "revenue_value", 10.1
            }
        };

        private readonly Dictionary<string, dynamic> MockOptionsEquals = new Dictionary<string, dynamic>()
        {
            {
                "custom_variables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "12345"}
                    }
            },
            {
                "revenue_value", 10.1
            }
        };

        private readonly Dictionary<string, dynamic> MockOptionsEndWith = new Dictionary<string, dynamic>()
        {
            {
                "custom_variables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "91123"}
                    }
            },
            {
                "revenue_value", 10.1
            }
        };

        private readonly Dictionary<string, dynamic> MockOptionsLower = new Dictionary<string, dynamic>()
        {
            {
                "custom_variables", new Dictionary<string, dynamic>()
                    {
                        {"hello", "world"},
                        {"a", "91123"}
                    }
            },
            {
                "revenue_value", 10.1
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
            var result = vwoClient.Push(MockTagKey, MockTagValue,  MockUserId);
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
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey , MockUserId);
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
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey, MockVariableKey ,MockUserId);
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
        public void Track_Should_Return_False_When_Requested_Goal_Is_Revenue_Type_And_No_Revenue_Value_Is_Passed()
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
        public void Track_Should_Return_True_When_Requested_Goal_Is_Revenue_Type_And_No_Revenue_Value_Is_Passed_As_Integer()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            Dictionary<string, dynamic> revenueDict = new Dictionary<string, dynamic>(){{"revenue_value", -1}};
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, revenueDict);
            Assert.True(result);

            int revenueValue = revenueDict["revenue_value"];
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
        public void Track_Should_Return_True_When_Requested_Goal_Is_Revenue_Type_And_No_Revenue_Value_Is_Passed_As_Float()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            var mockCampaignResolver = Mock.GetCampaignAllocator();
            var selectedCampaign = GetCampaign();
            Mock.SetupResolve(mockCampaignResolver, selectedCampaign, selectedCampaign);
            var mockVariationResolver = Mock.GetVariationResolver();
            Mock.SetupResolve(mockVariationResolver, GetVariation());

            Dictionary<string, dynamic> revenueDict = new Dictionary<string, dynamic>() {{"revenue_value", -1}};
            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver);
            var result = vwoClient.Track(MockCampaignKey, MockUserId, MockGoalIdentifier, revenueDict);
            Assert.True(result);

            string revenueValue = revenueDict["revenue_value"].ToString();
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
            var result = vwoClient.GetFeatureVariableValue(MockCampaignKey,MockVariableKey, MockUserId);
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

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
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

            var vwoClient = GetVwoClient(mockValidator: mockValidator, mockCampaignResolver: mockCampaignResolver, mockVariationResolver: mockVariationResolver, segmentEvaluator: new SegmentEvaluator());
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
            var result = vwoClient.Push(mockTagKey , MockTagValue,  MockUserId);
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
            var result = vwoClient.Push(mockTagKey, MockTagValue,  MockUserId);
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
            var result = vwoClient.Push(MockTagKey, mockTagValue,  MockUserId);
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
            var result = vwoClient.Push(mockTagKey, MockTagValue,  MockUserId);
            Assert.True(result);

            mockValidator.Verify(mock => mock.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.Push(It.Is<string>(val => mockTagKey.Equals(val)), It.Is<string>(val => MockTagValue.Equals(val)), It.Is<string>(val => MockUserId.Equals(val))), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_Should_Return_False_When_Segments_Are_Passed_But_Custom_Variables_Are_Not_Passed()
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

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
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

        private IVWOClient GetVwoClient(Mock<IValidator> mockValidator = null, Mock<ICampaignAllocator> mockCampaignResolver = null, Mock<IVariationAllocator> mockVariationResolver = null, ISegmentEvaluator segmentEvaluator = null)
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

            if (segmentEvaluator == null) {
                var mockSegmentEvaluator = Mock.GetSegmentEvaluator();
                Mock.SetupResolve(mockSegmentEvaluator, true);
                segmentEvaluator = mockSegmentEvaluator.Object;
            }

            return new VWO(GetSettings(), mockValidator.Object, null, mockCampaignResolver.Object,  segmentEvaluator, mockVariationResolver.Object, true);
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

        private BucketedCampaign GetCampaign(string campaignKey = null, string variationName = null, string status = "RUNNING", string goalIdentifier = null, string campaignType = null, Dictionary<string, dynamic> segments = null, List<Dictionary<string, dynamic>> mockVariables = null)
        {
            campaignKey = campaignKey ?? MockCampaignKey;
            return new BucketedCampaign(-1, 100, campaignKey, status, campaignType != null ? campaignType : Constants.CampaignTypes.VISUAL_AB, segments, mockVariables)
            {
                Variations = GetVariations(variationName, mockVariables),
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
    }
}
