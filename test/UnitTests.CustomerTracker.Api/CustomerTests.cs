using System;
using System.Threading.Tasks;
using CustomerTracker.Api.Customers;
using CustomerTracker.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.CustomerTracker.Api
{
    public class CustomerTests
    {
        [Fact]
        public async Task Return_OK_Response_When_Found()
        {
            var mockLogger = new Mock<ILogger<CustomerController>>();
            var stubRepository = new Mock<ICustomerRepository>();

            var id = Guid.NewGuid();

            stubRepository
                .Setup(x => x.FindByKeyAsync(id))
                .ReturnsAsync(new Customer { Id = id });

            var sut = new CustomerController(mockLogger.Object, stubRepository.Object);

            var result = await sut.GetById(id);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Return_404_When_Not_Found()
        {
            var mockLogger = new Mock<ILogger<CustomerController>>();
            var stubRepository = new Mock<ICustomerRepository>();

            var id = Guid.NewGuid();

            var sut = new CustomerController(mockLogger.Object, stubRepository.Object);

            var result = await sut.GetById(id);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Log_Unknown_Customer_When_Not_Found()
        {
            var mockLogger = new Mock<ILogger<CustomerController>>();
            var stubRepository = new Mock<ICustomerRepository>();

            var id = Guid.NewGuid();

            var sut = new CustomerController(mockLogger.Object, stubRepository.Object);

            var _ = await sut.GetById(id);

            mockLogger.Verify(x =>
                x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Customer Not Found: {id}"),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ), Times.Once);
        }
    }
}
