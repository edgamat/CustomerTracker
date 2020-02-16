using System;

namespace CustomerTracker.Domain
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }
    }
}
