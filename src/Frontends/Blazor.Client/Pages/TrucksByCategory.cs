using Blazor.Client.Models;
using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Client.Pages
{
    public partial class TrucksByCategory
    {
        public IEnumerable<TruckDto> Trucks { get; set; }
        
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public ITruckService TruckService { get; set; }

        public string DummyData { get; set; }

        protected async override Task OnInitializedAsync()
        {

            CategoryId = 1;
            Trucks = await TruckService.GetTrucksByCategoryIdOld(CategoryId);
            if (Trucks == null)
                Trucks = await FakeData();
        
        }

        async Task<IEnumerable<TruckDto>> FakeData()
        { 

        return await  Task.Run<List<TruckDto>>(()=> { 
          return   new List<TruckDto> {
                new TruckDto { Name = "T1", Description = " t1 desc" },
                new TruckDto { Name = "T2", Description = " t2 desc" },
                new TruckDto { Name = "T3", Description = " t3 desc" },
            };
          });
        } 
    }
}
