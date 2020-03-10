using System;
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
        private readonly IDateTimeService _dateTimeService;

        public CreateNewCustomerCommandHandler(
            ICustomerRepository repository,
            IAccountingGateway gateway,
            IDateTimeService dateTimeService)
        {
            _repository = repository;
            _gateway = gateway;
            _dateTimeService = dateTimeService;
        }

        public async Task<Result> HandleAsync(CreateNewCustomerCommand command)
        {
            if (command == null)
            {
                return Result.Fail("command is null");
            }

            var request = new RegisterCustomerRequest(command.Name, command.EmailAddress);

            try
            {
                var result = await _gateway.RegisterCustomerAsync(request);
                if (result.IsSuccess)
                {
                    var accountingId = ((Result<Guid>) result).Value;
                    var customer = new Customer(accountingId, command.Name, command.EmailAddress);

                    await _repository.InsertAsync(customer);

                    return Result.Ok(customer.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}