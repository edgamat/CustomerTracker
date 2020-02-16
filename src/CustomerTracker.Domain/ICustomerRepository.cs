using System;

namespace CustomerTracker.Domain
{
    public interface ICustomerRepository : IAsyncRepository<Customer, Guid>
    {
    }
}