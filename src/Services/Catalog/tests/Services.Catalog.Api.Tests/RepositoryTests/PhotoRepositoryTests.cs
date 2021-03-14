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
using Microsoft.Data.Sqlite;

namespace Services.Catalog.Api.UnitTests
{
    public class PhotoRepositoryTests
    {

       

        [Fact]
        public async void AddPhoto_AddsToDatabase()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            connection.CreateFunction("gen_random_uuid", () => Guid.NewGuid());
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;
            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                     Truck truckToAdd = new Truck
                     {
                         TruckId = Guid.NewGuid(),
                         Name = "Truck1",
                         Description = "New description",
                         Price = 10.00M,
                         Year = DateTime.Now.Year,
                      };
                await context.Trucks.AddAsync(truckToAdd);
                Mock<ILogger<PhotoRepository>> moqLogger = new Mock<ILogger<PhotoRepository>>();

                var photoRepository = new PhotoRepository(context, moqLogger.Object);
                await photoRepository.AddPhoto(new Photo
                {
                    PhotoPath = "photo.jpeg",
                    TruckId = truckToAdd.TruckId
                });
                await photoRepository.SaveChanges();
                Assert.Single(context.Photos);
            }
        }

        [Fact]
        public async void AddPhoto_EmptyPhoto_ThrowsNullException()
        {
            var connectionStringBuilder =
            new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;
            using (var context = new CatalogDbContext(options))
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
            var connectionStringBuilder =
            new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;
            using (var context = new CatalogDbContext(options))
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
            var connectionStringBuilder =
            new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;
            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                Mock<ILogger<PhotoRepository>> moqLogger = new Mock<ILogger<PhotoRepository>>();
                var photoRepository = new PhotoRepository(context, moqLogger.Object);
                var photos = await photoRepository.GetPhotosByTruckId(Guid.NewGuid());
                Assert.Empty(photos);
            }
        }

        [Fact]
        public void UpdatePhoto_EmptyPhoto_ThrowsNullException()
        {
            var connectionStringBuilder =
            new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;
            using (var context = new CatalogDbContext(options))
            {
                Mock<ILogger<PhotoRepository>> moqLogger = new Mock<ILogger<PhotoRepository>>();
                var photoRepository = new PhotoRepository(context, moqLogger.Object);
                Assert.Throws<ArgumentNullException>(() => photoRepository.UpdatePhoto(null));
            }
        }
    }
}
