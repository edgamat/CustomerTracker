using System;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using FluentAssertions;
using Moq;
using Xunit;

namespace UnitTests.CustomerTracker.Domain
{
    public class CreateNewCustomerCommandTests
    {
        [Fact]
        public async Task BasicUsage()
        {
            var command = new CreateNewCustomerCommand("name", "test@example.com");

            var mockRepository = new Mock<ICustomerRepository>();
            var mockGateway = new Mock<IAccountingGateway>();
            var stubDateTimeService = new Mock<IDateTimeService>();

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object, stubDateTimeService.Object);

            Func<Task> act = async () => { await sut.HandleAsync(command); };

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task NullCommandFails()
        {
            var mockRepository = new Mock<ICustomerRepository>();
            var mockGateway = new Mock<IAccountingGateway>();
            var stubDateTimeService = new Mock<IDateTimeService>();

            var sut = new CreateNewCustomerCommandHandler(mockRepository.Object, mockGateway.Object, stubDateTimeService.Object);

            var result =  await sut.HandleAsync(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("command is null");
        }
    }
}