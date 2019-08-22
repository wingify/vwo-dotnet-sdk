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
