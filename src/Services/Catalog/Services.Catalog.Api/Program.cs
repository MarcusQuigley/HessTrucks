using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Extensions;

namespace Services.Catalog.Api
{
    public class Program
    {
        public   static void Main(string[] args)
        {
            CreateHostBuilder(args)
                 .Build()
                 .MigrateAndSeedDatabase<CatalogDbContext>(retries: 3)
                 //.SeedDatabase<CatalogDbContext>()
                 .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration)
                                                       .WriteTo.Console())
                ;
    }
}
