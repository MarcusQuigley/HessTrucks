using Blazor.App.Extensions;
using Blazor.App.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Blazor.App.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly IHttpClientFactory _httpFactory;
        public CategoryService(IHttpClientFactory httpFactory, ILogger<CategoryService> logger)
        {
            _httpFactory = httpFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/categories");

            var _client = _httpFactory.CreateClient("CategoryHttpClient");

            try
            {
                using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    return await response.DeserializeStreamAsJson<IEnumerable<CategoryDto>>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Categories");
            }
            return null;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesBySize(bool isMini = false)
        {
            throw new NotImplementedException();
        }
    }
}
