using Blazor.Client.Models;
using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Client.Components
{
    public partial class TruckDetail
    {
       // [Parameter]
        public Guid TruckId { get; set; }
        public TruckDto Truck { get; set; } = new TruckDto();

        [Inject]
        public ITruckService TruckService { get; set; }
 
        public bool ShowComponent { get; set; }

        public async Task ShowDetails(Guid truckId)
        {
            ShowComponent = true;
            Truck = await TruckService.GetTruckById(truckId);
            StateHasChanged();           
        }
 
        public void Close()
        {
            ShowComponent = false;
            StateHasChanged();
        }


        async Task<TruckDto> FakeDataAsync()
        {
            return await Task.Run<TruckDto>(() =>
            {
                return new TruckDto
                {
                    TruckId = Guid.Parse("f1b72b77-bca3-43ab-9427-a4373d3f51cd"),
                    Name = "Hess Truck and Racers",
                    Description = "Hess Truck and Racers Mint ",
                    Price = 41.99M,
                    Year = 1997
                };
            });
        }
    }
}
