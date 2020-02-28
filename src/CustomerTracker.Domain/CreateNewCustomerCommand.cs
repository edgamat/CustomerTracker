using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

    public class CreateNewCustomerCommandHandler : ICommandHandler<CreateNewCustomerCommand>
    {
        private readonly ICustomerRepository _repository;
        private readonly IAccountingGateway _gateway;

        public CreateNewCustomerCommandHandler(ICustomerRepository repository, IAccountingGateway gateway)
        {
            _repository = repository;
            _gateway = gateway;
        }

        public async Task<Result> HandleAsync(CreateNewCustomerCommand command)
        {
            var customer = new Customer
            {
                Name = command.Name,
                EmailAddress = command.EmailAddress,
                IsActive = true
            };

            await _repository.InsertAsync(customer);

            await _gateway.RegisterCustomerAsync(customer);

            return Result.Ok();
        }
    }
}