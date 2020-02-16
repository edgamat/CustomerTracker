using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CustomerTracker.Persistence
{
    public class CustomerContextFactory : IDesignTimeDbContextFactory<CustomerContext>
    {
        public CustomerContext CreateDbContext(string[] args)
        {
            var runtimePath = GetRuntimePath();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(runtimePath)
                .AddJsonFile("appsettings.json")
                .Build();

            return new CustomerContext(CreateOptions(configuration), EntityTypeConfigurations.All);
        }

        private static string GetRuntimePath()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyFile = executingAssembly.Location;

            return Path.GetDirectoryName(assemblyFile);
        }

        public static DbContextOptions<CustomerContext> CreateOptions(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var contextOptions = new DbContextOptionsBuilder<CustomerContext>();

            contextOptions.UseSqlServer(configuration["Database:ConnectionString"]);

            return contextOptions.Options;
        }
    }
}