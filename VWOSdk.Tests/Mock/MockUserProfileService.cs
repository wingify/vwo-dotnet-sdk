using System;
using Moq;

namespace VWOSdk.Tests
{
    internal class MockUserProfileService
    {
        internal static Mock<IUserProfileService> Get()
        {
            return new Mock<IUserProfileService>();
        }

        internal static void SetupLookup(Mock<IUserProfileService> mockUserProfileService, UserProfileMap returnValue)
        {
            mockUserProfileService.Setup(mock => mock.Lookup(It.IsAny<string>()))
                .Returns(returnValue);
        }

        internal static void SetupSave(Mock<IUserProfileService> mockUserProfileService, Exception exception)
        {
            mockUserProfileService.Setup(mock => mock.Save(It.IsAny<UserProfileMap>()))
                .Throws(exception);
        }

        internal static void SetupLookup(Mock<IUserProfileService> mockUserProfileService, Exception exception)
        {
            mockUserProfileService.Setup(mock => mock.Lookup(It.IsAny<string>()))
                .Throws(exception);
        }
    }
}