using CustomerTracker.Domain;
using Xunit;

namespace UnitTests.CustomerTracker.Domain
{
    public class CustomerTests
    {
        [Fact]
        public void Customer_Inactive_By_Default()
        {
            var sut = new Customer();

            Assert.False(sut.IsActive);
        }
    }
}
