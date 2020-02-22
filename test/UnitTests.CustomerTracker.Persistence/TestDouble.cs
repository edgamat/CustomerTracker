using Moq;

namespace UnitTests.CustomerTracker.Persistence
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