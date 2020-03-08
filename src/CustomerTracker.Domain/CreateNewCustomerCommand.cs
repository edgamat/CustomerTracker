using System;
using System.Diagnostics.CodeAnalysis;
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

            var customer = new Customer
            {
                Name = command.Name,
                EmailAddress = command.EmailAddress,
                IsActive = true,
                AddedAt = _dateTimeService.OffsetUtcNow
            };

            try
            {
                await _repository.InsertAsync(customer);

                await _gateway.RegisterCustomerAsync(customer);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}