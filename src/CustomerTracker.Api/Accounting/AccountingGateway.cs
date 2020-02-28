using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using CustomerTracker.Domain.SharedKernel;
using Newtonsoft.Json.Schema;

namespace CustomerTracker.Api.Accounting
{
    public class AccountingGateway : IAccountingGateway
    {
        private readonly HttpClient _client;
        private readonly AccountingConfiguration _configuration;

        public AccountingGateway(IHttpClientFactory factory, AccountingConfiguration configuration)
        {
            _client = factory.CreateClient("accounting");
            _configuration = configuration;
        }

        public async Task<Result> RegisterCustomerAsync(Customer customer)
        {
            var uri = $"{_configuration.BaseUri}/customer";

            var response = await _client.PostAsJsonAsync(uri, customer);
            if (response.IsSuccessStatusCode)
            {
                var customerData = await response.Content.ReadAsAsync<RegisteredCustomer>();
                return Result.Ok(customerData);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorData = await response.Content.ReadAsAsync<RegistrationErrors>();
                return Result.Fail(errorData.Message);
            }

            return Result.Fail($"{response.StatusCode} {response.ReasonPhrase}");
        }
    }
}