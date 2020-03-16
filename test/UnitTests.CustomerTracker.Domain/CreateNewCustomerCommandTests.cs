using System;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using CustomerTracker.Domain.SharedKernel;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.CustomerTracker.Domain
{
    public class CreateNewCustomerCommandTests
    {
        [Fact]
        public void BadCommandShouldThrowException()
        {
            var command = new CreateNewCustomerCommand(null, null);

            var mockRepository = new TestDouble<ICustomerRepository>();
            var mockGateway = new TestDouble<IAccountingGateway>();

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object);

            async Task Act()
            {
                await sut.HandleAsync(command);
            }

            sut.Invoking(x => Act())
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage("*name*");
        }

        [Fact]
        public async Task NullCommandFails()
        {
            var mockRepository = new TestDouble<ICustomerRepository>();
            var mockGateway = new TestDouble<IAccountingGateway>();

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object);

            var result =  await sut.HandleAsync(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("command is null");
        }

        [Fact]
        public async Task ReturnSuccessWhenRegistered()
        {
            // Arrange
            var newId = Guid.NewGuid();
            var newAccountingId = Guid.NewGuid();
            var command = new CreateNewCustomerCommand("name", "test@example.com");

            var mockRepository = new TestDouble<ICustomerRepository>();
            mockRepository
                .Setup(x => x.InsertAsync(It.IsAny<Customer>()))
                .Callback((Customer c) =>
                {
                    c.Id = newId;
                });

            var mockGateway = new TestDouble<IAccountingGateway>();
            mockGateway
                .Setup(x => x.RegisterCustomerAsync(It.IsAny<RegisterCustomerRequest>()))
                .ReturnsAsync(Result.Ok(newAccountingId));

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object);

            // Act
            var result = await sut.HandleAsync(command);

            // Assert
            result.IsSuccess.Should().BeTrue();

            mockRepository
                .Verify(x => x.InsertAsync(It.Is<Customer>(c => c.AccountingId == newAccountingId)), Times.Once);
        }

        [Fact]
        public async Task ReturnFailedWhenCannotInsertCustomer()
        {
            var newId = Guid.NewGuid();
            var command = new CreateNewCustomerCommand("name", "test@example.com");

            var mockRepository = new TestDouble<ICustomerRepository>();
            mockRepository
                .Setup(x => x.InsertAsync(It.IsAny<Customer>()))
                .ThrowsAsync(new Exception("Something went wrong"));

            var mockGateway = new TestDouble<IAccountingGateway>();
            mockGateway
                .Setup(x => x.RegisterCustomerAsync(It.IsAny<RegisterCustomerRequest>()))
                .ReturnsAsync(Result.Ok(newId));

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object);

            var result = await sut.HandleAsync(command);

            result.IsFailure.Should().BeTrue();
        }
    }
}