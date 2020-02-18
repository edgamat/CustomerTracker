using Moq;

namespace UnitTests.CustomerTracker.Api
{
    public class TestDouble<T> : Mock<T> where T: class
    {
        public TestDouble()
        {
        }

        public TestDouble(MockBehavior behavior) : base(behavior)
        {
        }
    }
}