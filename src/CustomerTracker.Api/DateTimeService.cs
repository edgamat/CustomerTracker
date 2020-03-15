using System;
using CustomerTracker.Domain;

namespace CustomerTracker.Api
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;

        public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
    }
}