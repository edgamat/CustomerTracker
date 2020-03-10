using System;
using System.Threading.Tasks;
using CustomerTracker.Api.Customers;
using CustomerTracker.Domain;
using CustomerTracker.Domain.SharedKernel;
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
                .ReturnsAsync(new Customer("John Doe", "test@example.com"));

            var sut = new CustomerController(stubLogger.Object, stubRepository.Object);

            var result = await sut.GetById(id);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Return_NotFound_With_Body_When_Customer_Unknown()
        {
            var id = Guid.NewGuid();

            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var stubRepository = new TestDouble<ICustomerRepository>();

            var sut = new CustomerController(stubLogger.Object, stubRepository.Object);

            var result = await sut.GetById(id);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Log_Unknown_Customer_When_Not_Found()
        {
            var id = Guid.NewGuid();

            var mockLogger = new TestDouble<ILogger<CustomerController>>();
            var stubRepository = new TestDouble<ICustomerRepository>();

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
        public void Return_ServerError_When_Creation_Fails()
        {
            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var stubRepository = new TestDouble<ICustomerRepository>();
            stubRepository
                .Setup(x => x.InsertAsync(It.IsAny<Customer>()))
                .ThrowsAsync(new Exception("Something went wrong"));

            var request = new CustomerCreateRequest
            {
                Name = "John Doe",
                EmailAddress = "test@example.com"
            };

            var sut = new CustomerController(stubLogger.Object, stubRepository.Object);

            async Task Act()
            {
                await sut.Insert(request);
            }

            sut.Invoking(x => Act())
                .Should()
                .Throw<Exception>()
                .WithMessage("*Something went wrong*");
        }

        [Fact]
        public async Task Return_CreatedAt_When_Creating_New_Customer()
        {
            var stubLogger = new TestDouble<ILogger<CustomerController>>();
            var mockRepository = new TestDouble<ICustomerRepository>();
            mockRepository
                .Setup(x => x.InsertAsync(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);

            var request = new CustomerCreateRequest
            {
                Name = "John Doe",
                EmailAddress = "test@example.com"
            };

            var sut = new CustomerController(stubLogger.Object, mockRepository.Object);

            await sut.Insert(request);

            mockRepository
                .Verify(x => x.InsertAsync(It.IsAny<Customer>()), Times.Once);
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
                .ReturnsAsync(new Customer("John Doe", "test@example.com"));

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
