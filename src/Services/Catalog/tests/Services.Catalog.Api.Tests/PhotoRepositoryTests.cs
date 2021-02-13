using Microsoft.EntityFrameworkCore;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Entities;
using Services.Catalog.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
//using FluentAssertions;
namespace Services.Catalog.Api.UnitTests
{
   public class PhotoRepositoryTests
    {

        private DbContextOptions<CatalogDbContext> GetDbContextOptions(Guid dbGuid) =>
             new DbContextOptionsBuilder<CatalogDbContext>()
             .UseInMemoryDatabase(dbGuid.ToString())
             .Options;

        [Fact]
        public async void AddPhoto_AddsToDatabase()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                var photoRepository = new PhotoRepository(context, null);
               await photoRepository.AddPhoto(new Photo
                {
                    PhotoPath = "",
                   TruckId = Guid.NewGuid()
                });
                await photoRepository.SaveChanges();
                Assert.Single(context.Photos);
            }
        }

        [Fact]
        public async void AddPhoto_EmptyPhoto_ThrowsNullException()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                var photoRepository = new PhotoRepository(context, null);
              await Assert.ThrowsAsync<ArgumentNullException>(
                  async () =>await photoRepository.AddPhoto(null));
            }
        }

        [Fact]
        public async void GetPhoto_EmptyTruckId_ThrowsNullException()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                var photoRepository = new PhotoRepository(context, null);
                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await photoRepository.GetPhotosByTruckId(Guid.Empty));
            }
        }

        [Fact]
        public async void GetPhoto_ReturnsEmptySequence_WithBadTruckId()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                var photoRepository = new PhotoRepository(context, null);
                var photos = await photoRepository.GetPhotosByTruckId(Guid.NewGuid());
                Assert.Empty(photos);
            }
        }

        [Fact]
        public void UpdatePhoto_EmptyPhoto_ThrowsNullException()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                var photoRepository = new PhotoRepository(context, null);
                Assert.Throws<ArgumentNullException>(() => photoRepository.UpdatePhoto(null));
            }
        }
    }
}
