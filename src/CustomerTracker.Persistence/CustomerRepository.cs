using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomerTracker.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        public CustomerRepository(CustomerContext context)
        {
            _context = context;
        }

        public async Task<Customer> FindByKeyAsync(Guid id)
        {
            return await _context.Set<Customer>().FindAsync(id);
        }

        public async Task<IList<Customer>> FindByAsync(Expression<Func<Customer, bool>> predicate)
        {
            return await _context.Set<Customer>().Where(predicate).ToListAsync();
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

        public Task UpdateAsync(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}