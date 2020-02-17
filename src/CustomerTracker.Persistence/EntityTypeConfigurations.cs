using CustomerTracker.Persistence.Customers;

namespace CustomerTracker.Persistence
{
    public class EntityTypeConfigurations
    {
        public static readonly IEntityTypeConfiguration[] All =
        {
            new CustomerConfiguration()
        };
    }
}