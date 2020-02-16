using Microsoft.EntityFrameworkCore;

namespace CustomerTracker.Persistence
{
    public class CustomerContext : DbContext
    {
        private readonly IEntityTypeConfiguration[] _configurations;

        public CustomerContext(DbContextOptions options, IEntityTypeConfiguration[] configurations) : base(options)
        {
            _configurations = configurations;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Part of EF Core")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var map in _configurations)
            {
                map.Configure(modelBuilder);
            }
        }
    }
}