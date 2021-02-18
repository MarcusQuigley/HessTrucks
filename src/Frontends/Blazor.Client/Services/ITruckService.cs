using Blazor.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Client.Services
{
  public interface ITruckService
    {
        Task<IEnumerable<TruckDto>> GetTrucks();
        Task<IEnumerable<TruckDto>> GetTrucksByCategoryId(int categoryId);
        Task<TruckDto> GetTruckById(Guid truckId);
        //Task AddTruck(Truck truck);
        //void UpdateTruck(Truck truck);
        //Task<bool> SaveChanges();
    }
}
