using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazor.Client.Services;
using System.Diagnostics;

namespace Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            await DelayClient();
#endif
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

     
            builder.Services.AddHttpClient<ITruckService, TruckService>(client => client.BaseAddress = new Uri("https://localhost:7601/"));
            //builder.Services.AddHttpClient<ITruckService, TruckService>("TrucksClient", config =>
            //{
            //    config.BaseAddress = new Uri("https://localhost:7601/");
            //    config.Timeout = new TimeSpan(0, 0, 30);
            //});
            ////  .ConfigurePrimaryHttpMessageHandler(config => new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip });

            await builder.Build().RunAsync();
        }

        static async Task DelayClient()
        {
            //delay to wait for the debugging proxy to start up
             await Task.Delay(1900);
         
        }
    }
}
