using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Entities;
using Services.Catalog.Api.Controllers;
using Microsoft.Extensions.Logging;
using Services.Catalog.Api.Services;
using AutoMapper;
using Services.Catalog.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Services.Catalog.Api.UnitTests
{
   public class CategoryControllerTests
    {
      //  private readonly DbContextOptions<CatalogDbContext> _dbOptions; 
        public CategoryControllerTests()
        {
           
        }

        private IEnumerable<Category> SeedFakeCategories =>
              new List<Category>() {
                  new Category {  CategoryId=1, Name = "80s", IsMiniTruck=false}
                , new Category {  CategoryId=2, Name = "90s", IsMiniTruck=false}
                , new Category {  CategoryId=3, Name = "200s", IsMiniTruck=false}
              };

        private IEnumerable<CategoryDto> SeedFakeCategoryDtos =>
             new List<CategoryDto>() {
                  new CategoryDto {  CategoryId=1, Name = "80s", IsMiniTruck=false}
                , new CategoryDto {  CategoryId=2, Name = "90s", IsMiniTruck=false}
                , new CategoryDto {  CategoryId=3, Name = "200s", IsMiniTruck=false}
             };

        [Fact]
        public async Task AllCategories_Returns_Correct_Results()
        {

            DbContextOptions<CatalogDbContext> _dbOptions = new DbContextOptionsBuilder<CatalogDbContext>()
                   .UseInMemoryDatabase("in-memory")
                   .Options;
            using (var context = new CatalogDbContext(_dbOptions))
            {
                context.AddRange(SeedFakeCategories);
                context.SaveChanges();
            }

            Mock<ICategoryRepository> mockCategoryRepository = new Mock<ICategoryRepository>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            Mock<ILogger<CategoryController>> mockLogger = new Mock<ILogger<CategoryController>>();
            mockCategoryRepository.Setup(repo => repo.GetCategories())
                                  .Returns(  Task.FromResult(SeedFakeCategories));
            //mockMapper.Setup(mapper => mapper.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()))
            //          .Returns(SeedFakeCategoryDtos);
            var controller = new CategoryController(mockCategoryRepository.Object, mockLogger.Object, mockMapper.Object);
            var results =await controller.AllCategories();

            var okResult = Assert.IsType<ActionResult<IEnumerable<CategoryDto>>>(results);
             var model = (OkObjectResult) okResult.Result;

             Assert.Equal(3, ((IEnumerable<Category>) model.Value).Count());
        
        }

        [Fact]
        public async Task AllCategories_Empty_Returns_NoData_Results()
        {
            Mock<ICategoryRepository> mockRepo = new Mock<ICategoryRepository>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            Mock<ILogger<CategoryController>> mockLogger = new Mock<ILogger<CategoryController>>();

            mockRepo.Setup(s => s.GetCategories())
                    .Returns(Task.FromResult<IEnumerable<Category>>(null));
            var controller = new CategoryController(mockRepo.Object, mockLogger.Object, mockMapper.Object);
            var result = await controller.AllCategories();
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CategoryDto>>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
 
        }
    }
}
