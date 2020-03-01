using System;

namespace CustomerTracker.Domain
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }

        DateTimeOffset OffsetUtcNow { get; }
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
    }
}