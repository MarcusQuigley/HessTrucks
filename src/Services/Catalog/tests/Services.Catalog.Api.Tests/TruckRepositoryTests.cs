using Microsoft.EntityFrameworkCore;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Services.Catalog.Api.UnitTests
{
   public class TruckRepositoryTests
    {


        [Fact]
        public async void GetTruckById_EmptyTruckId_ThrowsException()
        {
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase("CatalogDbForTesting")
                .Options;

            using (var context = new CatalogDbContext(options))
            {
                var repo = new TruckRepository(context, null);
                await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetTruckById(Guid.Empty));
            }
        }
    }
}
