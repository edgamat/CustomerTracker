using CustomerTracker.Domain.SharedKernel;

namespace CustomerTracker.Domain
{
    public class CreateNewCustomerCommand : ICommand
    {
        public CreateNewCustomerCommand(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public string Name { get; }

        public string EmailAddress { get; }
    }
}