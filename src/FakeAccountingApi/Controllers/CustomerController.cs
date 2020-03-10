using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FakeAccountingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Register([FromBody] CustomerRequest customer)
        {
            _logger.LogInformation("Customer: {name} {emailAddress}", customer?.Name, customer?.EmailAddress);

            if (customer?.Name == "fake user")
            {
                return BadRequest(new { message = "Invalid Customer Details"});
            }

            return Ok(new {id = Guid.NewGuid()});
        }
    }
}