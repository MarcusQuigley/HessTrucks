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
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _service;
        private IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository service, ILogger<CategoryController> logger, IMapper mapper)
        {
            _service = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [CategoriesFilter]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> AllCategories()
        {
            var categories = await _service.GetCategories();
            if (categories == null)
                return  NotFound();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{isMini:bool}")]
        [CategoriesFilter]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> CategoriesBySize(bool isMini)
        {
            var categories = await _service.GetCategoriesBySize(isMini);
            if (categories == null)
                return NotFound();
            return Ok(categories);
        }
    }
}
