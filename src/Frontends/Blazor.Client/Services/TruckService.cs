using Blazor.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Blazor.Client.Services
{
    public class TruckService : ITruckService
    {
        private readonly HttpClient _client;
        public TruckService(HttpClient client)
        {
            _client = client;
        }
        public Task<TruckDto> GetTruckById(Guid truckId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TruckDto>> GetTrucks()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TruckDto>> GetTrucksByCategoryId(int categoryId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/trucks/{categoryId}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {            
            using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                using (var streamReader = new StreamReader(stream))
                {
                    using(var jsonReader = new JsonTextReader(streamReader))
                    {
                        var serializer = new JsonSerializer();
                        var trucks = serializer.Deserialize<IEnumerable<TruckDto>>(jsonReader);
                        return trucks;
                    }
                }
            }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }
    }
}
