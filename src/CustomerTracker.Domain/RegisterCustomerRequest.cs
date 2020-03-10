using System;

namespace CustomerTracker.Domain
{
    public class RegisterCustomerRequest
    {
        public RegisterCustomerRequest(string name, string emailAddress)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
        }

        public string Name { get; }

        public string EmailAddress { get; }
    }
}