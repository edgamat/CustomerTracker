using System;

namespace CustomerTracker.Domain
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }

        DateTimeOffset OffsetUtcNow { get; }
    }
}