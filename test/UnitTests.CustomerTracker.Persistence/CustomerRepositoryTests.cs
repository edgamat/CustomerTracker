using System;
using System.Collections.Generic;
using CustomerTracker.Domain;
using CustomerTracker.Persistence;
using CustomerTracker.Persistence.Customers;
using FluentAssertions;
using Xunit;

namespace UnitTests.CustomerTracker.Persistence
{
    public class CustomerRepositoryTests
    {
        [Fact(DisplayName = "FindByKey Returns Customer When Found")]
        public void FindByKey_Returns_Customer_When_Found()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customer = new Customer("John Doe", "test@example.com");
            var customers = new List<Customer> { customer };

            var stubContext = new TestDouble<CustomerTrackerContext>(null);
            stubContext.HasDbSetOf(customers, c => c.Id);

            var sut = new CustomerRepository(stubContext.Object);

            // Act
            var actual = sut.FindByKeyAsync(id);

            // Assert
            actual.Should().NotBeNull();
        }
    }
}
