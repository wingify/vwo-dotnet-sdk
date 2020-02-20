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

using System.Net.Http;
using Xunit;

namespace VWOSdk.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        public void Extension_GetHttpMethod_Should_Return_Desired_HttpMethod_For_Given_Method()
        {
            Assert.Equal(Method.GET.GetHttpMethod(), HttpMethod.Get);
        }

        [Theory]
        [InlineData(LogLevel.ERROR, LogLevel.ERROR, true)]
        [InlineData(LogLevel.ERROR, LogLevel.WARNING, false)]
        [InlineData(LogLevel.ERROR, LogLevel.INFO, false)]
        [InlineData(LogLevel.ERROR, LogLevel.DEBUG, false)]

        [InlineData(LogLevel.WARNING, LogLevel.ERROR, true)]
        [InlineData(LogLevel.WARNING, LogLevel.WARNING, true)]
        [InlineData(LogLevel.WARNING, LogLevel.INFO, false)]
        [InlineData(LogLevel.WARNING, LogLevel.DEBUG, false)]

        [InlineData(LogLevel.INFO, LogLevel.ERROR, true)]
        [InlineData(LogLevel.INFO, LogLevel.WARNING, true)]
        [InlineData(LogLevel.INFO, LogLevel.INFO, true)]
        [InlineData(LogLevel.INFO, LogLevel.DEBUG, false)]

        [InlineData(LogLevel.DEBUG, LogLevel.ERROR, true)]
        [InlineData(LogLevel.DEBUG, LogLevel.WARNING, true)]
        [InlineData(LogLevel.DEBUG, LogLevel.INFO, true)]
        [InlineData(LogLevel.DEBUG, LogLevel.DEBUG, true)]
        public void Extension_IsLogTypeEnabled_Should_Return_Desired_Value(LogLevel userSpecifiedLevel, LogLevel logLevel, bool expectedValue)
        {
            Assert.Equal(expectedValue, userSpecifiedLevel.IsLogTypeEnabled(logLevel));
        }
    }
}
