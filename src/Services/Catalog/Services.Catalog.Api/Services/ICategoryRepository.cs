using Services.Catalog.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Catalog.Api.Services
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<IEnumerable<Category>> GetCategoriesBySize(bool isMini = false);
        Task<Category> GetCategory(int categoryId);
        Task<int> AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task<bool> SaveChanges();
    }
}
