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
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;

namespace Services.Catalog.Api.UnitTests
{
    public class CategoryRepositoryTests
    {
        //SqliteConnectionStringBuilder connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
        //private DbContextOptions<CatalogDbContext> GetDbContextOptions =>
        //    new DbContextOptionsBuilder<CatalogDbContext>()
        //         .UseSqlite(new SqliteConnection(connectionBuilder.ToString()))
        //         .Options;

        [Fact]
        public async void AddCategory_EmptyCategory_ThrowsNullException()
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
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryRepository.AddCategory(null));
            }
        }

        [Fact]
        public async void AddCategory_AddsToDatabase()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;
            var dbContextGuid = Guid.NewGuid();
            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                await categoryRepository.AddCategory(new Category
                {
                    Name = "Category1",
                    CategoryOrder = 1
                });

                await categoryRepository.SaveChanges();
                Assert.Single(context.Categories);
            }
        }

        [Fact]
        public async void GetCategoriesBySize_ReturnsValidData()
        {

            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContextGuid = Guid.NewGuid();
            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                await context.Categories.AddAsync(new Category { Name = "cat1", IsMiniTruck = false });
                await context.Categories.AddAsync(new Category { Name = "cat2", IsMiniTruck = true });
                await context.Categories.AddAsync(new Category { Name = "cat3", IsMiniTruck = true });
                await context.Categories.AddAsync(new Category { Name = "cat4", IsMiniTruck = true });
                await context.SaveChangesAsync();
            }

            using (var context = new CatalogDbContext(options))
            {
               // await context.Database.OpenConnectionAsync();
                //await context.Database.EnsureCreatedAsync();
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var miniTrucks = await categoryRepository.GetCategoriesBySize(true);
                Assert.Equal(3, miniTrucks.Count());
            }
        }

        [Fact]
        public async void GetCategory_ReturnsNull_WithBadCategoryId()
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
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var category = await categoryRepository.GetCategory(int.MinValue);
                Assert.Null(category);
            }
        }

        [Fact]
        public async void GetCategory_ReturnsCategory_WithValidCategoryId()
        {
            var connectionStringBuilder =
                 new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;

            Category expected = null;
            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                var entity = await context.Categories.AddAsync(
                    new Category { Name = "cat1", IsMiniTruck = false });
                await context.SaveChangesAsync();
                expected = entity.Entity;
            }

            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var actual = await categoryRepository.GetCategory(expected.CategoryId);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async void UpdateCategory_UpdatesCategory_WithValidCategory()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;
            var dbContextGuid = Guid.NewGuid();
            Category categoryToSave = null;
            int categoryId = int.MinValue;
            string updateCategoryName = Guid.NewGuid().ToString();
            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                var entity = await context.Categories.AddAsync(
                    new Category { Name = "cat1", IsMiniTruck = false });
                await context.SaveChangesAsync();
                categoryToSave = entity.Entity;
                categoryId = entity.Entity.CategoryId;
            }

            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                categoryToSave = await categoryRepository.GetCategory(categoryId);

                categoryToSave.Name = updateCategoryName;
                categoryRepository.UpdateCategory(categoryToSave);
                await context.SaveChangesAsync();
            }

            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var returnedCategory = await categoryRepository.GetCategory(categoryId);
                Assert.Equal(updateCategoryName, returnedCategory.Name);
            }
        }

        [Fact]
        public void UpdateCategory_EmptyCategory_ThrowsNullException()
        {
            var connectionStringBuilder =
               new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;
            using (var context = new CatalogDbContext(options))
            {
                  context.Database.OpenConnection();
                  context.Database.EnsureCreated();
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                Assert.Throws<ArgumentNullException>(() => categoryRepository.UpdateCategory(null));
            }
        }
    }
}
