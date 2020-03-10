using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using CustomerTracker.Domain.SharedKernel;
using FluentAssertions;
using FluentAssertions.Execution;
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
            var stubDateTimeService = new TestDouble<IDateTimeService>();

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object, stubDateTimeService.Object);

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
            var stubDateTimeService = new TestDouble<IDateTimeService>();

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object, stubDateTimeService.Object);

            var result =  await sut.HandleAsync(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("command is null");
        }

        [Fact]
        public async Task ReturnSuccessWhenRegistered()
        {
            var newId = Guid.NewGuid();
            var command = new CreateNewCustomerCommand("name", "test@example.com");

            var mockRepository = new TestDouble<ICustomerRepository>();
            var mockGateway = new TestDouble<IAccountingGateway>();
            mockGateway
                .Setup(x => x.RegisterCustomerAsync(It.IsAny<RegisterCustomerRequest>()))
                .ReturnsAsync(Result.Ok(newId));

            var stubDateTimeService = new TestDouble<IDateTimeService>();

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object, stubDateTimeService.Object);

            var result = await sut.HandleAsync(command);

            result.IsSuccess.Should().BeTrue();
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

            var stubDateTimeService = new TestDouble<IDateTimeService>();

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object, stubDateTimeService.Object);

            var result = await sut.HandleAsync(command);

            result.IsFailure.Should().BeTrue();
        }
    }
}