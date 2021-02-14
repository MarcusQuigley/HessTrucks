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
    public class PhotoRepository : IPhotoRepository
    {
        private readonly CatalogDbContext _context;
        private readonly ILogger<PhotoRepository> _logger;

        public PhotoRepository(CatalogDbContext context, ILogger<PhotoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddPhoto(Photo photo)
        {
            if (photo == null)
            {
                _logger.LogError("Photo to add is null");
                throw new ArgumentNullException(nameof(photo));
            }
             await _context.Photos.AddAsync(photo);
        }

        public async Task<IEnumerable<Photo>> GetPhotosByTruckId(Guid truckId)
        {
            if (truckId == Guid.Empty)
            {
                _logger.LogError("TruckId to get photos for is null");
                throw new ArgumentException("truckId is empty");
            }
            return await _context.Photos.Where(p => p.TruckId == truckId)
                                        .ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdatePhoto(Photo photo)
        {
            if (photo == null)
            {
                _logger.LogError("Photo to add is null");
                throw new ArgumentNullException(nameof(photo));
            }
            //return Task.CompletedTask;
            // throw new NotImplementedException();
        }
    }
}
