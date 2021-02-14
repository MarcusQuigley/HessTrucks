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

namespace Services.Catalog.Api.UnitTests
{
    public class CategoryRepositoryTests
    {
        private DbContextOptions<CatalogDbContext> GetDbContextOptions(Guid dbGuid) =>
            new DbContextOptionsBuilder<CatalogDbContext>()
                       .UseInMemoryDatabase(dbGuid.ToString())
                       .Options;

        [Fact]
        public async void AddCategory_EmptyCategory_ThrowsNullException()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryRepository.AddCategory(null));
            }
        }

        [Fact]
        public async void AddCategory_AddsToDatabase()
        {
            var dbContextGuid = Guid.NewGuid();
            using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                await categoryRepository.AddCategory(new Category
                {
                    Name = "Category1",
                    Order = 1
                });

                await categoryRepository.SaveChanges();
                Assert.Single(context.Categories);
            }
        }

        [Fact]
        public async void AddCategory_ReturnsValidCategoryId()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var categoryId = await categoryRepository.AddCategory(new Category
                {
                    Name = "Category1",
                    Order = 1
                });

                await categoryRepository.SaveChanges();
                Assert.Collection<Category>(context.Categories, c => Assert.Equal(c.CategoryId, categoryId));
            }
        }

        [Fact]
        public async void GetCategoriesBySize_ReturnsValidData()
        {
            var dbContextGuid = Guid.NewGuid();
            using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
            {
                await context.Categories.AddAsync(new Category { Name = "cat1", IsMiniTruck = false });
                await context.Categories.AddAsync(new Category { Name = "cat2", IsMiniTruck = true });
                await context.Categories.AddAsync(new Category { Name = "cat3", IsMiniTruck = true });
                await context.Categories.AddAsync(new Category { Name = "cat4", IsMiniTruck = true });
                await context.SaveChangesAsync();
            }

            using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var miniTrucks = await categoryRepository.GetCategoriesBySize(true);
                Assert.Equal(3, miniTrucks.Count());
            }
        }

        [Fact]
        public async void GetCategory_ReturnsNull_WithBadCategoryId()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var category = await categoryRepository.GetCategory(int.MinValue);
                Assert.Null(category);
            }
        }

        [Fact]
        public async void GetCategory_ReturnsCategory_WithValidCategoryId()
        {
            var dbContextGuid = Guid.NewGuid();
            Category expected = null;
            using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
            {
                var entity = await context.Categories.AddAsync(
                    new Category { Name = "cat1", IsMiniTruck = false });
                await context.SaveChangesAsync();
                expected = entity.Entity;
            }

            using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var actual = await categoryRepository.GetCategory(expected.CategoryId);
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async void UpdateCategory_UpdatesCategory_WithValidCategory()
        {
            var dbContextGuid = Guid.NewGuid();
            Category categoryToSave = null;
            int categoryId = int.MinValue;
            string updateCategoryName = Guid.NewGuid().ToString();
            using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
            {
                var entity = await context.Categories.AddAsync(
                    new Category { Name = "cat1", IsMiniTruck = false });
                await context.SaveChangesAsync();
                categoryToSave = entity.Entity;
                categoryId = entity.Entity.CategoryId;
            }

            using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                categoryToSave = await categoryRepository.GetCategory(categoryId);

                categoryToSave.Name = updateCategoryName;
                categoryRepository.UpdateCategory(categoryToSave);
                await context.SaveChangesAsync();
            }

            using (var context = new CatalogDbContext(GetDbContextOptions(dbContextGuid)))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                var returnedCategory = await categoryRepository.GetCategory(categoryId);
                Assert.Equal(updateCategoryName, returnedCategory.Name);
            }
        }

        [Fact]
        public void UpdateCategory_EmptyCategory_ThrowsNullException()
        {
            using (var context = new CatalogDbContext(GetDbContextOptions(Guid.NewGuid())))
            {
                Mock<ILogger<CategoryRepository>> moqLogger = new Mock<ILogger<CategoryRepository>>();
                var categoryRepository = new CategoryRepository(context, moqLogger.Object);
                Assert.Throws<ArgumentNullException>(() => categoryRepository.UpdateCategory(null));
            }
        }
    }
}
