using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerTracker.Api.Customers
{
    [Route("customer")]
    [ApiController]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _repository;

        public CustomerController(ILogger<CustomerController> logger, ICustomerRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> AllAsync()
        {
            return await _repository.AllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> FindByKeyAsync(Guid id)
        {
            var customer = await _repository.FindByKeyAsync(id);
            if (customer == null)
            {
                _logger.LogInformation("Customer Not Found: {id}", id);
                return NotFound("unknown_customer");
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerCreateRequest model)
        {
            var customer = new Customer
            {
                Name = model.Name,
                EmailAddress = model.EmailAddress,
                IsActive = true
            };

            await _repository.InsertAsync(customer);

            return CreatedAtAction(nameof(FindByKeyAsync), new { customer.Id }, customer);
        }
    }
}