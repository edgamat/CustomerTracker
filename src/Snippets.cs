
// // When no changes are made then don't bother updating database
// if (customer.Name == model.Name &&
//     customer.EmailAddress == model.EmailAddress &&
//     customer.IsActive == model.IsActive)
// {
//     return Ok();
// }


// var command = new CreateNewCustomerCommand(model.Name, model.EmailAddress);
//
// var result = await _handler.HandleAsync(command);
//
// if (result.IsSuccess)
// {
//     var newId = ((Result<Guid>) result).Value;
//     return CreatedAtAction(nameof(GetById), new { id = newId }, null);
// }
//
// throw new Exception(result.Error);


// private CustomerController CreateSystemUnderTest(
//     TestDouble<ILogger<CustomerController>> logger = null,
//     TestDouble<ICustomerRepository> repository = null,
//     TestDouble<ICommandHandler<CreateNewCustomerCommand>> handler = null)
// {
//     if (logger == null) logger = new TestDouble<ILogger<CustomerController>>();
//     if (repository == null) repository = new TestDouble<ICustomerRepository>();
//     if (handler == null) handler = new TestDouble<ICommandHandler<CreateNewCustomerCommand>>();
//
//     return new CustomerController(logger.Object, repository.Object, handler.Object);
// }
