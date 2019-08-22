using Moq;

namespace VWOSdk.Tests
{
    internal class MockUserHasher
    {
        public static Mock<IBucketService> Get()
        {
            var mock = new Mock<IBucketService>();
            return mock;
        }

        internal static void SetupCompute(Mock<IBucketService> mockUserHasher, int returnVal)
        {
            mockUserHasher.Setup(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()))
                .Returns(returnVal);
        }

        internal static void SetupComputeBucketValue(Mock<IBucketService> mockUserHasher, int returnVal, double outHashValue)
        {
            SetupCompute(mockUserHasher, returnVal);
            mockUserHasher.Setup(mock => mock.ComputeBucketValue(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), out outHashValue))
                .Returns<string, double, double, double>((userid, maxVal, multiplier, hashvalue) => mockUserHasher.Object.ComputeBucketValue(userid, maxVal, multiplier));
        }
    }
}