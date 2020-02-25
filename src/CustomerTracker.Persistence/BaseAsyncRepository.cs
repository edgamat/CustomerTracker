using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using Microsoft.EntityFrameworkCore;

namespace CustomerTracker.Persistence
{
    public abstract class BaseAsyncRepository<TEntity, TKey, TContext> : IAsyncRepository<TEntity, TKey>
        where TContext : DbContext
        where TEntity : class
    {
        protected abstract TContext Context { get; }

        public async Task<TEntity> FindByKeyAsync(TKey id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IList<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context
                .Set<TEntity>()
                .AsNoTracking()
                .Where(predicate).ToListAsync();
        }

        public async Task<IList<TEntity>> AllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task InsertAsync(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);

            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (Context.Set<TEntity>().Local.Any(x => x == entity) == false)
                Context.Set<TEntity>().Update(entity);

            await Context.SaveChangesAsync();
        }

        public Task DeleteAsync(TKey id)
        {
            throw new NotImplementedException();
        }
    }
}