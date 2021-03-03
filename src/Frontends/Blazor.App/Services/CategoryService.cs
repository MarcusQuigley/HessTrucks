using Blazor.App.Extensions;
using Blazor.App.Models;
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
        private readonly HttpClient _client;

        public CategoryService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/categories");
            request.Headers.Accept.Add(new  MediaTypeWithQualityHeaderValue("application/json"));

            using (var response =await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                return await response.DeserializeStreamAsJson<IEnumerable<CategoryDto>>();
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesBySize(bool isMini = false)
        {
            throw new NotImplementedException();
        }
    }
}
