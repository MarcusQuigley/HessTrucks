using Services.Catalog.Api.Controllers;
using System;
using Xunit;

namespace Services.Catalog.Api.Tests
{
    public class TruckControllerTests
    {
        TruckController controller = new TruckController();
        [Fact] 
        public void getreturnshellowworld()
        {
            var expected = "Hello world";
            Assert.Equal(expected, controller.Greeting().Result);
        }
    }
}
