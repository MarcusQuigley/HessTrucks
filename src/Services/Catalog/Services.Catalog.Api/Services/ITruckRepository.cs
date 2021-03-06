﻿using Services.Catalog.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.Services
{
  public interface ITruckRepository
    {
        Task<IEnumerable<Truck>> GetTrucks();
        Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId);
        Task<Truck> GetTruckById(Guid truckId);
        Task AddTruck(Truck truck);
        Task<bool> UpdateTruck(Truck truck);
        Task<bool> SaveChanges();
    }
}
