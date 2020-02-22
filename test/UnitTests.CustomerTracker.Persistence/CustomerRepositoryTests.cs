using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using CustomerTracker.Persistence;
using CustomerTracker.Persistence.Customers;
using FluentAssertions;
using Xunit;

namespace UnitTests.CustomerTracker.Persistence
{
    public class CustomerRepositoryTests
    {
        [Fact]
        public void FindByKey_Returns_Customer_When_Found()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customer = new Customer { Id = id };
            var customers = new List<Customer> { customer };

            var mockContext = new TestDouble<CustomerTrackerContext>(null);
            mockContext.HasDbSetOf(customers, c => c.Id);

            // Act
            var sut = new CustomerRepository(mockContext.Object);

            var actual = sut.FindByKeyAsync(id);

            // Assert
            actual.Should().NotBeNull();
        }
    }
}
