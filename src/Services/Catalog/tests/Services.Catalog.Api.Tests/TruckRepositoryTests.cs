﻿using Microsoft.EntityFrameworkCore;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Entities;
using Services.Catalog.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Diagnostics;
using Moq;
using Microsoft.Extensions.Logging;

namespace Services.Catalog.Api.UnitTests
{
   public class TruckRepositoryTests
    {
        private DbContextOptions<CatalogDbContext> GetDbContextOptions(Guid dbGuid) =>
             new DbContextOptionsBuilder<CatalogDbContext>()
             .UseInMemoryDatabase(dbGuid.ToString())
             .Options;

        [Fact]
        public async void AddTruck_EmptyTruck_ThrowsException()
        {
             using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<TruckRepository>> moqLogger = new Mock<ILogger<TruckRepository>>();
                var truckRepository = new TruckRepository(context, moqLogger.Object);
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await truckRepository.AddTruck(null));
            }
        }

        [Fact]
        public async void GetTruckById_EmptyTruckId_ThrowsException()
        {
             using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<TruckRepository>> moqLogger = new Mock<ILogger<TruckRepository>>();
                var truckRepository = new TruckRepository(context, moqLogger.Object);
                await Assert.ThrowsAsync<ArgumentException>(async () => await truckRepository.GetTruckById(Guid.Empty));
            }
        }

        [Fact]
        public   void UpdateTruck_EmptyTruck_ThrowsException()
        {
             using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<TruckRepository>> moqLogger = new Mock<ILogger<TruckRepository>>();
                var truckRepository = new TruckRepository(context, moqLogger.Object);
                Assert.Throws<ArgumentNullException>(  () => truckRepository.UpdateTruck(null));
            }
        }

//        [Fact]
        //public async void GetTruckById_WithGoodData_ReturnsProperData()
        //{
        //    var dbContextGuid = Guid.NewGuid();
        //    var truckId = Guid.NewGuid();
        //    Category categoryToAdd = new Category
        //    {
        //        Name = "cat1",
        //        IsMiniTruck = false
        //    };

        //    Truck truckToAdd = new Truck
        //    {
        //        TruckId = truckId,
        //        Name = "Truck1",
        //        Description = "New description",
        //        Price = 10.00M,
        //        Year = DateTime.Now.Year,

        //        //IsMiniTruck = false }); ; ;
        //    };

        //    using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
        //    {
        //        Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
        //        var categoryRepository = new CategoryRepository(context, moqLogger.Object);
        //        var categoryId= await categoryRepository.AddCategory(categoryToAdd);
        //        await context.SaveChangesAsync();
        //        categoryToAdd.CategoryId = categoryId;
        //    }

        //    using (var context = new CatalogDbContext(GetDbContextOptions(  dbContextGuid)))
        //    {
        //        Mock<ILogger<TruckRepository>> moqLogger = new Mock<ILogger<TruckRepository>>();
        //        var truckRepository = new TruckRepository(context, moqLogger.Object);
        //        truckToAdd.Categories.Add(categoryToAdd);// = new List<Category> { categoryToAdd };
        //          await truckRepository.AddTruck(truckToAdd);

        //        try
        //        {
        //            await context.SaveChangesAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine(ex.Message);
        //        }
        //    }

        //    using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
        //    {
        //        Photo photo = new Photo { 
        //        PhotoId = Guid.NewGuid(),
        //            PhotoPath = "photo",
        //            TruckId = truckId
        //        };
        //        context.Photos.Add(photo);
        //        truckToAdd.Photos.Add(photo);
        //        Mock<ILogger<TruckRepository>> moqLogger = new Mock<ILogger<TruckRepository>>();
        //        var truckRepository = new TruckRepository(context, moqLogger.Object);

        //        //  truckToAdd.Categories = new List<Category> { categoryToAdd };
        //        truckRepository.UpdateTruck(truckToAdd);

        //        await context.SaveChangesAsync();
        //    }

        //    using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
        //    {
        //        Mock<ILogger<TruckRepository>> moqLogger = new Mock<ILogger<TruckRepository>>();
        //        var truckRepository = new TruckRepository(context, moqLogger.Object);
        //        var truck = await truckRepository.GetTruckById(truckId);
        //        truck.Should().BeEquivalentTo(truckToAdd);
        //    }
        //}
    }
}
