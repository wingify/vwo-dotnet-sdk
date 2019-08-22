using Xunit;

namespace VWOSdk.Tests
{
    public class WeightRangeTests
    {
        [Theory]
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(1, 2, 1, 4, false)]
        [InlineData(1, 2, 3, 2, false)]
        [InlineData(1, 2, 1, 2, true)]
        public void Equals_Should_Return_Desired_Results(double xFrom, double xTo, double yFrom, double yTo, bool expectedResult)
        {
            var x = new WeightRange(xFrom, xTo);
            var y = new WeightRange(yFrom, yTo);
            var result = x.Equals(y);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(1, 2, 1, 4, false)]
        [InlineData(1, 2, 3, 2, false)]
        [InlineData(1, 2, 1, 2, true)]
        public void GetHashCode_Should_Return_Desired_Results(double xFrom, double xTo, double yFrom, double yTo, bool expectedResult)
        {
            var x = new WeightRange(xFrom, xTo);
            var y = new WeightRange(yFrom, yTo);
            var xResult = x.GetHashCode();
            var yResult = y.GetHashCode();
            bool actualResult = xResult == yResult;
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
