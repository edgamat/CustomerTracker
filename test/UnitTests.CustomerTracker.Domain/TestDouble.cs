using Moq;

namespace UnitTests.CustomerTracker.Domain
{
    public class TestDouble<T> : Mock<T> where T: class
    {
        public TestDouble()
        {
        }

        public TestDouble(params object[] args) : base(args)
        {
        }
    }
}