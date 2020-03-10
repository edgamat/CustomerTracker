using System;
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

        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
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
        public async Task<IActionResult> Insert([FromBody] CustomerCreateRequest model)
        {
            var customer = new Customer(model.Name, model.EmailAddress);

            await _repository.InsertAsync(customer);

            return CreatedAtAction(nameof(GetById), new { customer.Id }, null);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerUpdateRequest model)
        {
            var customer = await _repository.FindByKeyAsync(id);
            if (customer == null)
            {
                _logger.LogInformation("Customer Not Found: {id}", id);
                return NotFound("unknown_customer");
            }

            customer.EditPersonalInfo(model.Name, model.EmailAddress);
            customer.SetStatus(model.IsActive.GetValueOrDefault());

            await _repository.UpdateAsync(customer);

            return Ok();
        }
    }
}