using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Catalog.Api.Filters;
using Services.Catalog.Api.Models;
using Services.Catalog.Api.Services;
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
        private readonly ITruckRepository _service;
        private IMapper _mapper;
        private readonly ILogger<TruckController> _logger;
        public TruckController(ITruckRepository service, IMapper mapper , ILogger<TruckController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        [Route("{categoryId:int}")]
        [TrucksFilter]
        public async Task<ActionResult<IEnumerable<TruckDto>>> TrucksByCategory(int categoryId)
        {
            _logger.LogInformation("TrucksByCategory being called!!");
            var trucks = await _service.GetTrucksByCategoryId(categoryId);
            if (trucks == null)
                return NotFound();
            return Ok(trucks);
        }

        [HttpGet]
        [Route("{truckId:Guid}")]
        [TruckFilter]
        public async Task<ActionResult<TruckDto>> TruckById(Guid truckId)
        {
            if (truckId == Guid.Empty)
                return BadRequest();

            var truck=await _service.GetTruckById(truckId);
            if (truck == null)
                return NotFound();
            return Ok(truck);
        }
    }
}
