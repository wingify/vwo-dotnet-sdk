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
using System.Threading;
using Xunit;

namespace VWOSdk.Tests
{
    public class ApiRequestTests
    {
        [Fact]
        public void Execute_Should_Not_Call_ApiCaller_For_Development_Mode_And_Return_Default_Value()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            var apiRequest = new ApiRequest(Method.GET, true).WithCaller(mockApiCaller.Object);
            var response = apiRequest.Execute<Settings>();
            Assert.Null(response);

            mockApiCaller.Verify(mock => mock.Execute<Settings>(It.IsAny<ApiRequest>()), Times.Never);
            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void ExecuteAsync_Should_Not_Call_ApiCaller_For_Development_Mode()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            var apiRequest = new ApiRequest(Method.GET, true).WithCaller(mockApiCaller.Object);
            apiRequest.ExecuteAsync();

            mockApiCaller.Verify(mock => mock.Execute<Settings>(It.IsAny<ApiRequest>()), Times.Never);
            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void Execute_Should_Call_ApiCaller_For_Production_Mode_And_Return_Returned_Value()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            var apiRequest = new ApiRequest(Method.GET).WithCaller(mockApiCaller.Object);
            var response = apiRequest.Execute<Settings>();
            Assert.NotNull(response);

            mockApiCaller.Verify(mock => mock.Execute<Settings>(It.IsAny<ApiRequest>()), Times.Once);
            mockApiCaller.Verify(mock => mock.Execute<Settings>(It.Is<ApiRequest>(val => ReferenceEquals(apiRequest, val))), Times.Once);
            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void ExecuteAsync_Should_Call_ApiCaller_For_Production_Mode()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            var apiRequest = new ApiRequest(Method.GET).WithCaller(mockApiCaller.Object);
            apiRequest.ExecuteAsync();

            Thread.Sleep(10);

            mockApiCaller.Verify(mock => mock.Execute<Settings>(It.IsAny<ApiRequest>()), Times.Never);
            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.Is<ApiRequest>(val => ReferenceEquals(apiRequest, val))), Times.Once);
        }
    }
}
