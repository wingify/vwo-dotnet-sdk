using Xunit;

namespace VWOSdk.Tests
{
    public class UuidV5HelperTests
    {
        [Theory]
        [InlineData(123456, "Ashley", "4C9BA81BAF53591488EB5FD5E66A98B9")]
        [InlineData(123456, "Bill", "7E74487DBCC758E79CEAC4CD50C0458D")]
        [InlineData(123456, "Emma", "881F627CD7FC5C97B0F3BE9D58A09DA6")]
        [InlineData(123456, "Faizan", "6EA28F5D5CE25F5FA8B808E30039DE09")]
        public void UuidCreator_test(long accountId, string userId, string expected)
        {
            var response = UuidV5Helper.Compute(accountId, userId);
            Assert.Equal(expected, response);
        }
    }
}
