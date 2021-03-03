using Blazor.App.Models;
using Blazor.App.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.App.Pages
{
    public partial class TruckEdit
    {
        [Parameter]
        public Guid TruckId { get; set; }
        public TruckDto Truck { get; set; } = new TruckDto();

        [Inject]
        public ITruckService TruckService { get; set; }

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected async override Task OnInitializedAsync()
        {
            Saved = false;
            if (TruckId != Guid.Empty)
                Truck = await TruckService.GetTruckById(TruckId);
           //return base.OnInitializedAsync();
        }

        public async Task HandleValidSubmit()
        {
            Saved = false;
            if (await TruckService.UpdateTruck(Truck))
                Saved = true;
            else {
                StatusClass = "alert-danger";
                Message = "Something happened upadating the truck";
                // Saved
            }
        }
        public void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are validation errors. Try again";
        }
    }
}
