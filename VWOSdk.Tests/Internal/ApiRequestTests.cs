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
