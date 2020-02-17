using System;
using CustomerTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace CustomerTracker.Persistence.Customers
{
    public class CustomerConfiguration : IEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<Customer>(builder =>
            {
                builder.Property(e => e.Id).HasValueGenerator<SequentialGuidValueGenerator>();
                builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
                builder.Property(e => e.EmailAddress).HasMaxLength(255);
                builder.Property(e => e.IsActive).HasDefaultValue(true);
            });
        }
    }
}
