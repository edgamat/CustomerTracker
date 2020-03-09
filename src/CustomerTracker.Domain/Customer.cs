using System;

namespace CustomerTracker.Domain
{
    public class Customer
    {
        protected Customer()
        {
        }

        public Customer(string name, string emailAddress)
        {
            EditPersonalInfo(name, emailAddress);
            SetStatus(true);
        }

        public Guid Id { get; }

        public string Name { get; private set; }

        public string EmailAddress { get; private set; }

        public bool IsActive { get; private set; }

        public DateTimeOffset? AddedAt { get; private set; }

        public void SetStatus(bool isActive)
        {
            IsActive = isActive;
        }

        public void EditPersonalInfo(string name, string emailAddress)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
        }
    }
}
