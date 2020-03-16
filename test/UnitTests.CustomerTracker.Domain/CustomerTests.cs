using System;
using CustomerTracker.Domain;
using FluentAssertions;
using Xunit;

namespace UnitTests.CustomerTracker.Domain
{
    public class CustomerTests
    {
        [Fact]
        public void Customer_Active_By_Default()
        {
            var sut = new Customer("John Doe", "test@example.com");

            Assert.True(sut.IsActive);
        }

        [Fact]
        public void Customer_Determines_When_Added()
        {
            var sut = new Customer("John Doe", "test@example.com");

            Assert.True(sut.AddedAt.HasValue);
        }

        [Fact]
        public void PersonalInfo_Updated_Correctly()
        {
            var sut = new Customer("John Doe", "test@example.com");

            sut.EditPersonalInfo("Jane Doe", "test2@example.com");

            Assert.Equal("Jane Doe", sut.Name);
            Assert.Equal("test2@example.com", sut.EmailAddress);
        }

        [Fact]
        public void Providing_Bad_Name_Throws_Exception()
        {
            var sut = new Customer("John Doe", "test@example.com");

            sut.Invoking(x => x.EditPersonalInfo(null, "test2@example.com"))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage("*name*");
        }

        [Fact]
        public void Providing_Bad_EmailAddress_Throws_Exception()
        {
            var sut = new Customer("John Doe", "test@example.com");

            sut.Invoking(x => x.EditPersonalInfo("John Doe", null))
                .Should()
                .Throw<ArgumentNullException>()
                .WithMessage("*emailAddress*");
        }

        [Fact]
        public void Status_Updated_Correctly()
        {
            var sut = new Customer("John Doe", "test@example.com");

            sut.SetStatus(false);

            Assert.False(sut.IsActive);
        }
    }
}
