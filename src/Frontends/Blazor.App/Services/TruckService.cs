using Blazor.App.Extensions;
using Blazor.App.Models;
using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.App.Services
{
    public class TruckService : ITruckService
    {
    //    private readonly HttpClient _client;
        private readonly ILogger<TruckService> _logger;
        private readonly IHttpClientFactory _httpFactory;
        public TruckService(IHttpClientFactory httpFactory, ILogger<TruckService> logger)
        {
        
            _httpFactory = httpFactory;
            _logger = logger;
        }
        public async Task<TruckDto> GetTruckById(Guid truckId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/trucks/{truckId}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var _client = _httpFactory.CreateClient("CategoryHttpClient");
            try
            {
                using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    return await response.DeserializeStreamAsJson<TruckDto>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<TruckDto>> GetTrucks()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TruckDto>> GetTrucksByCategoryId(int categoryId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/trucks/{categoryId}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var _client = _httpFactory.CreateClient("CategoryHttpClient");
            try
            {
                using (var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                  
                    return await response.DeserializeStreamAsJson<IEnumerable<TruckDto>>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<bool> UpdateTruck(TruckDto truck)
        {
            if (truck == null)
                throw new ArgumentNullException(nameof(truck));
            var truckAsJson = JsonSerializer.Serialize(truck);
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/trucks");
            
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(truckAsJson);
            request.Content.Headers.ContentType.MediaType = "application/json";
            var _client = _httpFactory.CreateClient("CategoryHttpClient");
            try
            {
                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Error updating movie");
                return false;
            }
        }
    }
}
