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
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;

namespace Services.Catalog.Api.UnitTests
{
    public class TruckControllerTests
    {
        // TruckController controller = new TruckController();
        //[Fact] 
        //public void getreturnshellowworld()
        //{
        //    var expected = "Hello world";
        //    Assert.Equal(expected, controller.Greeting().Result);
        //}

        private IEnumerable<Truck> SeedTrucks()
        {
            var categories = new List<Category> { new Category { CategoryId = 1, Name = "Cat1" } };
            return new List<Truck>
            {
                new Truck{ TruckId = Guid.NewGuid(), Name = "Truck1",Description="none", Categories = categories},
                new Truck{ TruckId = Guid.NewGuid(), Name = "Truck2",Description="none", Categories = categories},
                new Truck{ TruckId = Guid.NewGuid(), Name = "Truck3",Description="none", Categories = categories},
            };
        }
        private IEnumerable<TruckDto> SeedTruckDtos()
        {
            var categories = new List<CategoryDto> { new CategoryDto { CategoryId = 1, Name = "Cat1" } };
            return new List<TruckDto>
            {
                new TruckDto{ TruckId = Guid.NewGuid(), Name = "Truck1",Description="none", Categories = categories},
                new TruckDto{ TruckId = Guid.NewGuid(), Name = "Truck2",Description="none", Categories = categories},
                new TruckDto{ TruckId = Guid.NewGuid(), Name = "Truck3",Description="none", Categories = categories},
            };
        }

        private Truck SeedTruck()
        {
         return    new Truck { TruckId = Guid.NewGuid(), Name = "Truck1", Description = "none" };//, Categories = categories },
        }


        [Fact]
        public async Task TrucksByCategory_CorrectData_returns_validdata()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new CatalogDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                await context.AddRangeAsync(SeedTrucks());
                await context.SaveChangesAsync();
            }

            Mock<ITruckRepository> mockRepository = new Mock<ITruckRepository>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();
            Mock<ILogger<TruckController>> mockLogger = new Mock<ILogger<TruckController>>();
            mockRepository.Setup(mr => mr.GetTrucksByCategoryId(It.IsAny<int>()))
                          .Returns(Task.FromResult(SeedTrucks()));


            var truckController = new TruckController(mockRepository.Object, mockMapper.Object, mockLogger.Object);
            var results = await truckController.TrucksByCategory(11);
            Assert.IsType<ActionResult<IEnumerable<TruckDto>>>(results);
            var model = (OkObjectResult)results.Result;
            Assert.Equal(3, ((IEnumerable<Truck>)model.Value).Count());
        }

        [Fact]
        public async Task TrucksByCategory_BadData_returns_Nodata()
        {
            var mockRepository = new Mock<ITruckRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<TruckController>>();

            mockRepository.Setup(mr => mr.GetTrucksByCategoryId(It.IsAny<int>()))
                          .Returns(Task.FromResult<IEnumerable<Truck>>(null));

            var truckController = new TruckController(
                mockRepository.Object, mockMapper.Object, mockLogger.Object);

            var result = await truckController.TrucksByCategory(It.IsAny<int>());

            Assert.IsType<ActionResult<IEnumerable<TruckDto>>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task TruckById_BadData_returns_Nodata()
        {
            var mockRepository = new Mock<ITruckRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<TruckController>>();

            mockRepository.Setup(mr => mr.GetTruckById(It.IsAny<Guid>()))
                          .Returns(Task.FromResult<Truck>(null));

            var controller = new TruckController(mockRepository.Object, mockMapper.Object, mockLogger.Object);

            var result = await controller.TruckById(Guid.NewGuid());
            Assert.IsType<ActionResult<TruckDto>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task TruckById_EmptyData_returns_BadRequest()
        {
            var mockRepository = new Mock<ITruckRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<TruckController>>();

            mockRepository.Setup(mr => mr.GetTruckById(It.IsAny<Guid>()))
                          .Returns(Task.FromResult<Truck>(null));

            var controller = new TruckController(mockRepository.Object, mockMapper.Object, mockLogger.Object);

            var result = await controller.TruckById(Guid.Empty);
            Assert.IsType<ActionResult<TruckDto>>(result);

            Assert.IsType<BadRequestResult>(result.Result);

        }

        [Fact]
        public async Task TruckById_CorrectData_returns_Validdata()
        {
            var mockRepository = new Mock<ITruckRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<TruckController>>();
            var guid = Guid.NewGuid();
            mockRepository.Setup(mr => mr.GetTruckById (It.IsAny<Guid>()))
                          .Returns(Task.FromResult(SeedTruck()));
 

            var controller = new TruckController(mockRepository.Object, mockMapper.Object, mockLogger.Object);

            //var result = await controller.TruckById(It.Is<Guid>(g => g == Guid.NewGuid()));
            var result = await controller.TruckById(Guid.NewGuid());

            Assert.IsType<ActionResult<TruckDto>>(result);
            Assert.IsType<OkObjectResult>(result.Result);
            //Assert.Equal(3, ((IEnumerable<Truck>)model.Value).Count());
        }
    }
}
