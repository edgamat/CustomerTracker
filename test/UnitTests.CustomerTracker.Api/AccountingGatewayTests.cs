using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CustomerTracker.Api.Accounting;
using CustomerTracker.Domain;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using UnitTests.CustomerTracker.Api.Http;
using Xunit;

namespace UnitTests.CustomerTracker.Api
{
    public class AccountingGatewayTests
    {
        [Fact]
        public async Task Successful_Result_When_Customer_Data_Returned()
        {
            var configuration = new AccountingConfiguration{ BaseUri = "http://test.local" };
            var handler = new Mock<IMockHttpMessageHandler>();
            handler.AddJsonResponse("http://test.local/customer", HttpMethod.Post, new RegisteredCustomer());

            var stubFactory = handler.GetMockHttpClientFactory("accounting");

            var request = new RegisterCustomerRequest("John", "test@example.com");

            var sut = new AccountingGateway(stubFactory.Object, configuration);

            var result = await sut.RegisterCustomerAsync(request);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Failure_Result_When_BadData_Sent()
        {
            var request = new RegisterCustomerRequest("John", "test@example.com");

            var configuration = new AccountingConfiguration{ BaseUri = "http://test.local" };
            var handler = new Mock<IMockHttpMessageHandler>();
            handler.AddJsonResponse(
                "http://test.local/customer",
                HttpMethod.Post,
                request,
                new RegistrationErrors {Message = "Test Error"},
                HttpStatusCode.BadRequest);

            var stubFactory = handler.GetMockHttpClientFactory("accounting");

            var sut = new AccountingGateway(stubFactory.Object, configuration);

            var result = await sut.RegisterCustomerAsync(request);

            using (new AssertionScope())
            {
                result.IsFailure.Should().BeTrue();

                result.Error.Should().Be("Test Error");

                handler.Verify(
                    x => x.SendAsync(
                        It.Is<HttpRequestMessage>(y => y.RequestUri == new Uri("http://test.local/customer") && y.Method == HttpMethod.Post),
                        It.IsAny<CancellationToken>())
                    , Times.Once);
            }
        }
    }
}