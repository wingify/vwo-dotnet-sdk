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
