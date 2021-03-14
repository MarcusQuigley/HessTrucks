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
            modelBuilder.Entity<Truck>()
               .Property(t => t.TruckId)
               .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Truck>()
                .Property(t => t.Name)
                .IsRequired();
            modelBuilder.Entity<Truck>()
                .Property(t => t.Description)
                .IsRequired();
            modelBuilder.Entity<Truck>()
                .Property(t => t.Damaged)
                .HasDefaultValue(false)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Truck>()
                .Property(t => t.Hidden)
                .HasDefaultValue(false)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Truck>()
                .Property(t => t.Quantity)
                .HasDefaultValue(1)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Truck>()
                .Property(t => t.Price)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Truck>()
                .Property(t => t.PreviousPrice)
                .HasPrecision(18, 2);


            modelBuilder.Entity<Category>()
               .Property(t => t.IsMiniTruck)
               .HasDefaultValue(false);
            // .ValueGeneratedOnAdd();

            modelBuilder.Entity<Photo>()
             .Property(t => t.PhotoId)
             .HasDefaultValueSql("gen_random_uuid()");

            base.OnModelCreating(modelBuilder);
        }
    }
}
