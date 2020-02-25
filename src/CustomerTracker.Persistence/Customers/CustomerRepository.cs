using System;
using CustomerTracker.Domain;

namespace CustomerTracker.Persistence.Customers
{
    public class CustomerRepository : BaseAsyncRepository<Customer, Guid, CustomerTrackerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerTrackerContext context)
        {
            Context = context;
        }

        protected override CustomerTrackerContext Context { get; }
    }
}