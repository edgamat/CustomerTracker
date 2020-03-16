using System;

namespace CustomerTracker.Domain
{
    public class Customer
    {
        // ReSharper disable once UnusedMember.Global
        protected Customer()
        {
        }

        public Customer(string name, string emailAddress)
        {
            EditPersonalInfo(name, emailAddress);
            SetStatus(true);
            AddedAt = DateTimeOffset.UtcNow;
        }

        public Customer(Guid accountingId, string name, string emailAddress)
            : this(name, emailAddress)
        {
            AccountingId = accountingId;
        }

        public Guid Id { get; set; }

        public string Name { get; private set; }

        public string EmailAddress { get; private set; }

        public bool IsActive { get; private set; }

        public Guid? AccountingId { get; }

        public DateTimeOffset? AddedAt { get; }

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
