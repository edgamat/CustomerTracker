using Microsoft.EntityFrameworkCore;

namespace CustomerTracker.Persistence
{
    public interface IEntityTypeConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}