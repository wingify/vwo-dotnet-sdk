using Moq;

namespace VWOSdk.Tests
{
    internal class MockValidator
    {
        internal static Mock<IValidator> Get()
        {
            var mockValidator = new Mock<IValidator>();
            SetupGetSettings(mockValidator, returnValue: true);
            SetupActivate(mockValidator, returnValue: true);
            SetupGetVariation(mockValidator, returnValue: true);
            SetupTrack(mockValidator, returnValue: true);
            SetupSettingsFile(mockValidator, returnValue: true);
            return mockValidator;
        }

        internal static void SetupGetSettings(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.GetSettings(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(returnValue);
        }

        internal static void SetupActivate(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.Activate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(returnValue);
        }

        internal static void SetupGetVariation(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(returnValue);
        }

        internal static void SetupTrack(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(returnValue);
        }

        internal static void SetupSettingsFile(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.SettingsFile(It.IsAny<Settings>()))
                .Returns(returnValue);
        }
    }
}