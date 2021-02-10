using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.Controllers
{
    [Route("api/trucks")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        [HttpGet]
        public Task<string> Greeting()
        {
            //return new Task<string>(()=> "Hello world");

         return   Task.FromResult("Hello world");
        }
    }
}
