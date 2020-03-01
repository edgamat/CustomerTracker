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
    public class CustomerControllerTests
    {
        [Fact]
        public async Task Return_OK_Response_When_Found()
        {
            var id = Guid.NewGuid();

            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var stubRepository = new TestDouble<ICustomerRepository>();

            stubRepository
                .Setup(x => x.FindByKeyAsync(id))
                .ReturnsAsync(new Customer { Id = id });

            var sut = new CustomerController(stubLogger.Object, stubRepository.Object);

            var result = await sut.GetById(id);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Return_NotFound_With_Body_When_Customer_Unknown()
        {
            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var stubRepository = new TestDouble<ICustomerRepository>();

            var id = Guid.NewGuid();

            var sut = new CustomerController(stubLogger.Object, stubRepository.Object);

            var result = await sut.GetById(id);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Log_Unknown_Customer_When_Not_Found()
        {
            var mockLogger = new TestDouble<ILogger<CustomerController>>();
            var stubRepository = new TestDouble<ICustomerRepository>();

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

        [Fact]
        public async Task New_Customers_Are_Active()
        {
            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var mockRepository = new TestDouble<ICustomerRepository>();

            var request = new CustomerCreateRequest
            {
                Name = "John Doe",
                EmailAddress = "test@example.com"
            };

            var sut = new CustomerController(stubLogger.Object, mockRepository.Object);

            await sut.Insert(request);

            mockRepository
                .Verify(x => x.InsertAsync(It.Is<Customer>(c => c.IsActive)), Times.Once);
        }

        [Fact]
        public async Task Return_CreatedAt_When_Creating_New_Customer()
        {
            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var stubRepository = new TestDouble<ICustomerRepository>();

            var request = new CustomerCreateRequest
            {
                Name = "John Doe",
                EmailAddress = "test@example.com"
            };

            var sut = new CustomerController(stubLogger.Object, stubRepository.Object);

            var result = await sut.Insert(request);

            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task Return_OK_When_Updates_Successful()
        {
            var id = Guid.NewGuid();

            var request = new CustomerUpdateRequest
            {
                Name = "John Doe",
                EmailAddress = "test@example.com",
                IsActive = true
            };

            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var mockRepository = new TestDouble<ICustomerRepository>();
            mockRepository
                .Setup(x => x.FindByKeyAsync(id))
                .ReturnsAsync(new Customer
                {
                    Id = id,
                    Name = "John Doe",
                    EmailAddress = "test@example.com",
                    IsActive = true
                });

            var sut = new CustomerController(stubLogger.Object, mockRepository.Object);

            await sut.Update(id, request);

            mockRepository
                .Verify(x => x.UpdateAsync(It.Is<Customer>(c => c.Name == "John Doe")), Times.Once);
        }

        [Fact]
        public async Task Return_NotFound_When_Customer_To_Update_NotFound()
        {
            var id = Guid.NewGuid();

            var request = new CustomerUpdateRequest
            {
                Name = "John Doe",
                EmailAddress = "test@example.com",
                IsActive = false
            };

            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var stubRepository = new TestDouble<ICustomerRepository>();
            stubRepository
                .Setup(x => x.FindByKeyAsync(id))
                .ReturnsAsync((Customer)null);

            var sut = new CustomerController(stubLogger.Object, stubRepository.Object);

            var result = await sut.Update(id, request);

            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
