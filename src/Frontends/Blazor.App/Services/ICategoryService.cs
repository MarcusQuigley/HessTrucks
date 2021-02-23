using Blazor.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.App.Services
{
  public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategories();
        Task<IEnumerable<CategoryDto>> GetCategoriesBySize(bool isMini = false);
        
    }
}
