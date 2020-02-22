using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;

namespace UnitTests.CustomerTracker.Persistence
{
    public static class MoqHelpers
    {
        public static void HasDbSetOf<TContext, TEntity>(this Mock<TContext> mockContext, IList<TEntity> list, params Func<TEntity, IComparable>[] props)
            where TEntity : class
            where TContext : DbContext
        {
            var mockDbSet = CreateDbSet(list).WithFindSupport(props);
            mockContext
                .Setup(x => x.Set<TEntity>())
                .Returns(mockDbSet.Object);
        }

        public static Mock<DbSet<TEntity>> CreateDbSet<TEntity>(IList<TEntity> list) where TEntity : class
        {
            var mockDbSet = list.AsQueryable().BuildMockDbSet();

            // Override AddAsync to add the new object directly to the underlying list.
            mockDbSet
                .Setup(x => x.AddAsync(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()))
                .Returns((TEntity qe, CancellationToken t) =>
                {
                    list.Add(qe);
                    return new ValueTask<EntityEntry<TEntity>>(Task.FromResult<EntityEntry<TEntity>>(null));
                });

            // Override Add to add the new object directly to the underlying list.
            mockDbSet.Setup(o => o.Add(It.IsAny<TEntity>()))
                .Callback((TEntity o) => list.Add(o))
                .Returns((EntityEntry<TEntity>) null);

            return mockDbSet;
        }

        public static Mock<DbSet<TEntity>> WithFindSupport<TEntity>(this Mock<DbSet<TEntity>> dbSet,
            params Func<TEntity, IComparable>[] props) where TEntity : class
        {
            Func<object[], IQueryable<TEntity>> find = objects =>
            {
                IQueryable<TEntity> query = dbSet.Object;

                if (objects.Length != props.Length) throw new Exception("Mismatched number of key fields.");

                for (var i = 0; i < props.Length; i++)
                {
                    var prop = props[i];
                    var id = objects[i];
                    query = query.Where(o => prop(o).Equals(id));
                }

                return query;
            };

            dbSet.Setup(o => o.Find(It.IsAny<object[]>()))
                .Returns((object[] ids) => find(ids).FirstOrDefault());

            dbSet.Setup(o => o.FindAsync(It.IsAny<object[]>()))
                .Returns((object[] ids) => new ValueTask<TEntity>(find(ids).FirstOrDefaultAsync()));

            dbSet.Setup(o => o.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
                .Returns((object[] ids, CancellationToken token) => new ValueTask<TEntity>(find(ids).FirstOrDefaultAsync(token)));

            return dbSet;
        }
    }
}