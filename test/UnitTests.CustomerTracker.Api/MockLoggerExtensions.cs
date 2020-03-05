using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.CustomerTracker.Api
{
    public static class MockLoggerExtensions
    {
        public static void VerifyLogEquals<T>(this Mock<ILogger<T>> mockLogger, LogLevel level, string message)
        {
            mockLogger.Verify(x =>
                x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString() == message),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ), Times.Once);
        }

        public static void VerifyLogContains<T>(this Mock<ILogger<T>> mockLogger, LogLevel level, string message)
        {
            mockLogger.Verify(x =>
                x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(message)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ), Times.Once);
        }
    }
}