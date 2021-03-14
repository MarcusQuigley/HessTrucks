using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;

namespace Services.Catalog.Api.Extensions
{
    public static class IHostExtensions
    {
        public static IHost MigrateAndSeedDatabase<T>(this IHost host, int retries = 3) where T : DbContext
        {
            var policy = Policy.Handle<Npgsql.NpgsqlException>().WaitAndRetry(retries, times => TimeSpan.FromMilliseconds(times * 100));

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<T>();
                policy.Execute(() =>
                {
                    context.Database.Migrate();
                    context.Database.EnsureCreated();
                    SeedData.Initialize(scope.ServiceProvider)
                            .Wait();
                });
            }
            return host;
        }
    }
}
