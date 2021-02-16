using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.Services
{
    public class TruckRepository : ITruckRepository
    {
        private readonly CatalogDbContext _context;
        private readonly ILogger<TruckRepository> _logger;

        public TruckRepository(CatalogDbContext context, ILogger<TruckRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        async Task<bool> TruckExists(Guid truckId)
        {
            return await _context.Trucks.AnyAsync(t => t.TruckId == truckId);
        }

        public async Task AddTruck(Truck truck)
        {
            if (truck == null)
            {
                _logger.LogError("Truck to add is null");
                throw new ArgumentNullException(nameof(truck));
            }
            await _context.Trucks.AddAsync(truck);
        }

        public async Task<Truck> GetTruckById(Guid truckId)
        {
            if (truckId == Guid.Empty)
            {
                _logger.LogError("Truckid to query is null");
                throw new ArgumentException(nameof(truckId));
            }
            return await _context.Trucks.Include(t => t.Categories)
                                        .Include(t => t.Photos)
                                        .AsSplitQuery()
                                        .FirstOrDefaultAsync(t => t.TruckId == truckId);
        }

        public async Task<IEnumerable<Truck>> GetTrucks()
        {
            return await _context.Trucks.Include(t => t.Categories)
                                        .Include(t => t.Photos)
                                        .AsSplitQuery()
                                        .ToListAsync();
        }

        public async Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId)
        {
            return await _context.Trucks.Where(t => t.Categories.Any(c => c.CategoryId == categoryId))
                                        .Include(t => t.Categories)
                                        .Include(t => t.Photos)
                                        .AsSplitQuery()
                                        .ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public void UpdateTruck(Truck truck)
        {
            if (truck ==null)
            {
                _logger.LogError("Truck to update is null");
                throw new ArgumentNullException(nameof(truck));
            }
        }
    }
}
