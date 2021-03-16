using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Services;
using AutoMapper;
using System.Reflection;

namespace Services.Catalog.Api
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
            services.AddControllers();

            //docker run -e POSTGRES_DB=hess_catalog_db -e POSTGRES_USER=marcus -e POSTGRES_PASSWORD=password -p 5432:5432 --name postgres_catalog -d postgres
            //docker exec -it 480c32e2bb53 "bash" //where 480c3.. is the container id
            //psql -h localhost -p 5432 -U postgres -d hess_catalog_db_thursday -W

            var server = Configuration["POSTGRES_SERVER"] ?? "localhost";
            var port = Configuration["POSTGRES_PORT"] ?? "5432";
            var database = Configuration["POSTGRES_DB"] ?? "hess_catalog_db_thursday";
            var user = Configuration["POSTGRES_USER"] ?? "postgres";
            var password = Configuration["POSTGRES_PASSWORD"] ?? "password";

            var connectionString = $"Host={server}; Port={port}; Database={database}; Username={user}; Password={password};";

            //"User ID =postgres;Password=password;Server=localhost;Port=5432;Database=testDb;Integrated Security=true;Pooling=true;" //alternative
            services.AddDbContext<CatalogDbContext>(options =>
                options.UseNpgsql(connectionString)
                       .UseSnakeCaseNamingConvention()
                    );

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITruckRepository, TruckRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            //services.AddScoped<>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Services.Catalog.Api", Version = "v1" });
            });
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Services.Catalog.Api v1"));
            } 
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("Open");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("health/live");
                endpoints.MapControllers();
            });
            logger.LogInformation("Pipeline setup at {0}", DateTime.Now);
        }
    }
}
