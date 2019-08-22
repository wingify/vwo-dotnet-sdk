using Moq;

namespace VWOSdk.Tests
{
    internal class MockSettingsProcessor
    {
        internal static Mock<ISettingsProcessor> Get()
        {
            var mockSettingsProcessor = new Mock<ISettingsProcessor>();
            SetupProcessAndBucket(mockSettingsProcessor);
            return mockSettingsProcessor;
        }

        internal static void SetupProcessAndBucket(Mock<ISettingsProcessor> mockSettingsProcessor, ISettingsProcessor innerSettingsProcessor = null)
        {
            innerSettingsProcessor = innerSettingsProcessor ?? new SettingsProcessor();
            mockSettingsProcessor.Setup(mock => mock.ProcessAndBucket(It.IsAny<Settings>()))
                .Returns<Settings>(settings => innerSettingsProcessor.ProcessAndBucket(settings));
        }

        internal static void SetupProcessAndBucket(Mock<ISettingsProcessor> mockSettingsProcessor, AccountSettings returnValue)
        {
            mockSettingsProcessor.Setup(mock => mock.ProcessAndBucket(It.IsAny<Settings>()))
                .Returns(returnValue);
        }
    }
}