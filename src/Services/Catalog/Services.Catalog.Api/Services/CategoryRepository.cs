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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(CatalogDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> AddCategory(Category category)
        {
            if (category == null)
            {
                _logger.LogError("Category to add is null");
                throw new ArgumentNullException(nameof(category));
            }
            var addedCategory =  await _context.Categories.AddAsync(category);
            return addedCategory.Entity.CategoryId;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesBySize(bool isMini = false)
        {
            return await _context.Categories.Where(c=>c.IsMiniTruck == isMini)
                                            .ToListAsync();
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            return await _context.Categories.Where(c => c.CategoryId == categoryId)
                                            .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateCategory(Category category)
        {
            if (category == null)
            {
                _logger.LogError("Category to add is null");
                throw new ArgumentNullException(nameof(category));
            }
            
        }
    }
}
