using Blazor.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.App.Services
{
  public interface ITruckService
    {
        Task<IEnumerable<TruckDto>> GetTrucks();
        Task<IEnumerable<TruckDto>> GetTrucksByCategoryId(int categoryId);
        Task<TruckDto> GetTruckById(Guid truckId);
        //Task AddTruck(Truck truck);
        Task<bool> UpdateTruck(TruckDto truck);
        //Task<bool> SaveChanges();
    }
}
