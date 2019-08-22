using Moq;

namespace VWOSdk.Tests
{
    internal class MockApiCaller
    {
        internal static Mock<IApiCaller> Get<T>(IApiCaller innerApiCaller = null)
        {
            innerApiCaller = innerApiCaller ?? new FileReaderApiCaller();
            var mockApiCaller = new Mock<IApiCaller>();
            SetupExecute<T>(mockApiCaller, innerApiCaller);
            SetupExecuteAsync(mockApiCaller, innerApiCaller);
            return mockApiCaller;
        }

        internal static void SetupExecute<T>(Mock<IApiCaller> mockApiCaller, T returnValue = default(T))
        {
            mockApiCaller.Setup(mock => mock.Execute<T>(It.IsAny<ApiRequest>()))
                .Returns(returnValue);
        }

        internal static void SetupExecute<T>(Mock<IApiCaller> mockApiCaller, IApiCaller innerApiCaller = null)
        {
            innerApiCaller = innerApiCaller ?? new FileReaderApiCaller();
            mockApiCaller.Setup(mock => mock.Execute<T>(It.IsAny<ApiRequest>()))
                .Returns<ApiRequest>((rq) => innerApiCaller.Execute<T>(rq));
        }

        internal static void SetupExecuteAsync(Mock<IApiCaller> mockApiCaller, IApiCaller innerApiCaller = null)
        {
            innerApiCaller = innerApiCaller ?? new FileReaderApiCaller();
            mockApiCaller.Setup(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()))
                .Returns<ApiRequest>((rq) => innerApiCaller.ExecuteAsync(rq));
        }
    }
}