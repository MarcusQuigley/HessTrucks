using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Entities;
using Services.Catalog.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
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
                Mock<ILogger<PhotoRepository>> moqLogger = new Mock<ILogger<PhotoRepository>>();
                var photoRepository = new PhotoRepository(context, moqLogger.Object);
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
                Mock<ILogger<PhotoRepository>> moqLogger = new Mock<ILogger<PhotoRepository>>();
                var photoRepository = new PhotoRepository(context, moqLogger.Object);
                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await photoRepository.AddPhoto(null));
            }
        }

        [Fact]
        public async void GetPhotosByTruckId_EmptyTruckId_ThrowsNullException()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<PhotoRepository>> moqLogger = new Mock<ILogger<PhotoRepository>>();
                var photoRepository = new PhotoRepository(context, moqLogger.Object);
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await photoRepository.GetPhotosByTruckId(Guid.Empty));
            }
        }

        [Fact]
        public async void GetPhotosByTruckId_ReturnsEmptySequence_WithBadTruckId()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<PhotoRepository>> moqLogger = new Mock<ILogger<PhotoRepository>>();
                var photoRepository = new PhotoRepository(context, moqLogger.Object);
                var photos = await photoRepository.GetPhotosByTruckId(Guid.NewGuid());
                Assert.Empty(photos);
            }
        }

        [Fact]
        public void UpdatePhoto_EmptyPhoto_ThrowsNullException()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<PhotoRepository>> moqLogger = new Mock<ILogger<PhotoRepository>>();
                var photoRepository = new PhotoRepository(context, moqLogger.Object);
                Assert.Throws<ArgumentNullException>(() => photoRepository.UpdatePhoto(null));
            }
        }
    }
}
