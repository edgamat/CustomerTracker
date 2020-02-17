using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomerTracker.Persistence.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerTrackerContext _context;

        public CustomerRepository(CustomerTrackerContext context)
        {
            _context = context;
        }

        public async Task<Customer> FindByKeyAsync(Guid id)
        {
            return await _context.Set<Customer>().FindAsync(id);
        }

        public async Task<IList<Customer>> FindByAsync(Expression<Func<Customer, bool>> predicate)
        {
            return await _context
                .Set<Customer>()
                .AsNoTracking()
                .Where(predicate).ToListAsync();
        }

        public async Task<IList<Customer>> AllAsync()
        {
            return await _context.Set<Customer>().ToListAsync();
        }

        public async Task InsertAsync(Customer entity)
        {
            _context.Set<Customer>().Add(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer entity)
        {
            if (_context.Set<Customer>().Local.Any(x => x == entity) == false)
                _context.Set<Customer>().Update(entity);

            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}