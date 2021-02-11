using Microsoft.EntityFrameworkCore;
using Services.Catalog.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.DbContexts
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            :base(options)
        {

        }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                      .SelectMany(t => t.GetProperties())
                      .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            modelBuilder.Entity<Truck>()
                .Property(t => t.TruckId)
                .HasDefaultValueSql("NewID()");
            modelBuilder.Entity<Truck>()
              .Property(t => t.Name)
              .IsRequired();
            modelBuilder.Entity<Truck>()
              .Property(t => t.Description)
              .IsRequired();
            modelBuilder.Entity<Truck>()
                .Property(t => t.Damaged)
                .HasDefaultValue(false);
            modelBuilder.Entity<Truck>()
               .Property(t => t.Hidden)
               .HasDefaultValue(false);
            modelBuilder.Entity<Truck>()
              .Property(t => t.Quantity)
              .HasDefaultValue(1);


            modelBuilder.Entity<Category>()
              .Property(t => t.IsMiniTruck)
              .HasDefaultValue(false);

            //    .HasMany(t => t.Photos)
            //    .WithOne(prop => prop.Truck)
            //    .IsRequired();

            //modelBuilder.Entity<TruckCategory>()
            //    .HasKey(tc => new { tc.TruckId, tc.CategoryId });
        }
    }
}
