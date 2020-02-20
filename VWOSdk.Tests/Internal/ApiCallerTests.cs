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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace VWOSdk.Tests
{
    public class ApiCallerTests
    {
        private class User
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Avatar { get; set; }
        }

        private class ReqResApiResponse
        {
            [JsonProperty("data")]
            public User User { get; set; }
        }

        [Fact]
        public void Execute_Should_Call_Api_And_Return_Deserialized_Response()
        {
            var apiRequest = new ApiRequest(Method.GET)
            {
                Uri = new Uri("https://reqres.in/api/users/2"),
            };
            IApiCaller apiCaller = GetApiCaller();
            var response = apiCaller.Execute<ReqResApiResponse>(apiRequest);
            Assert.NotNull(response);
            Assert.NotNull(response.User);
            Assert.Equal(2, response.User.Id);
        }

        [Fact]
        public void Execute_Should_Return_Null_For_Null_ApiRequest()
        {
            IApiCaller apiCaller = GetApiCaller();
            var response = apiCaller.Execute<ReqResApiResponse>(null);
            Assert.Null(response);
        }

        [Fact]
        public void ExecuteAsync_Should_Call_Api()
        {
            var apiRequest = new ApiRequest(Method.GET)
            {
                Uri = new Uri("https://reqres.in/api/users/2"),
            };
            IApiCaller apiCaller = GetApiCaller();
            var response = AsyncHelper.RunSync(() => apiCaller.ExecuteAsync(apiRequest));
            Assert.NotNull(response);
        }

        [Fact]
        public void ExecuteAsync_Should_Return_Null_For_Null_ApiRequest()
        {
            IApiCaller apiCaller = GetApiCaller();
            var response = AsyncHelper.RunSync(() => apiCaller.ExecuteAsync(null));
            Assert.Null(response);
        }

        [Fact]
        public void ExecuteAsync_Should_Return_Null_For_Invalid_URI_In_ApiRequest()
        {
            var apiRequest = new ApiRequest(Method.GET)
            {
                Uri = new Uri("http://r.in/api/users/2"),
            };
            IApiCaller apiCaller = GetApiCaller();
            var response = AsyncHelper.RunSync(() => apiCaller.ExecuteAsync(apiRequest));
            Assert.Null(response);
        }

        [Fact]
        public void Execute_Should_Call_Api_And_Return_Null_When_Json_Is_Not_In_Proper_Format()
        {
            var apiRequest = new ApiRequest(Method.GET)
            {
                Uri = new Uri("https://reqres.in/api/users/2"),
            };
            IApiCaller apiCaller = GetApiCaller();
            var response = apiCaller.Execute<List<User>>(apiRequest);
            Assert.Null(response);
        }

        [Fact]
        public void Execute_Should_Call_Api_And_Return_Null_When_Http_Response_Is_Failure()
        {
            var apiRequest = new ApiRequest(Method.GET)
            {
                Uri = new Uri("https://reqres.in/api/unknown/23"),
            };
            IApiCaller apiCaller = GetApiCaller();
            var response = apiCaller.Execute<ReqResApiResponse>(apiRequest);
            Assert.Null(response);
        }

        private static ApiCaller GetApiCaller()
        {
            return new ApiCaller();
        }
    }
}
