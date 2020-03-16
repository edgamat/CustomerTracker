using System;
using System.Threading.Tasks;
using CustomerTracker.Domain.SharedKernel;

namespace CustomerTracker.Domain
{
    public class CreateNewCustomerCommandHandler : ICommandHandler<CreateNewCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _repository;
        private readonly IAccountingGateway _gateway;

        public CreateNewCustomerCommandHandler(
            ICustomerRepository repository,
            IAccountingGateway gateway)
        {
            _repository = repository;
            _gateway = gateway;
        }

        public async Task<Result<Guid>> HandleAsync(CreateNewCustomerCommand command)
        {
            if (command == null)
            {
                return Result.Fail<Guid>("command is null");
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

                return Result.Fail<Guid>(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Fail<Guid>(ex.Message);
            }
        }
    }
}