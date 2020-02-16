using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerTracker.Domain;
using CustomerTracker.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomerTracker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var options = CustomerContextFactory.CreateOptions(Configuration);
            var configurations = EntityTypeConfigurations.All;

            using (var context = new CustomerContext(options, configurations))
                context.Database.EnsureCreated();

            services.AddScoped(p => new CustomerContext(options, configurations));
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddControllers(o => o.SuppressAsyncSuffixInActionNames = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
