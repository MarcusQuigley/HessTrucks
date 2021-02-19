using Blazor.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Blazor.Client.Services
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
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                using(var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader  = new JsonTextReader(streamReader))
                    {
                        var serializer = new JsonSerializer();
                        var categories =  serializer.Deserialize<IEnumerable<CategoryDto>>(jsonReader);
                        return categories;
                    }
                }
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesBySize(bool isMini = false)
        {
            throw new NotImplementedException();
        }
    }
}
