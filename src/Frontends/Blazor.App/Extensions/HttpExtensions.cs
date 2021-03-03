using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.App.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<T> DeserializeStreamAsJson<T>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var stream =await response.Content.ReadAsStreamAsync();
            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var deserializer = new JsonSerializer();
                    return deserializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}
