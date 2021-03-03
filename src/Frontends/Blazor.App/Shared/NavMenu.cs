using Blazor.App.Models;
using Blazor.App.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.App.Shared
{
    public partial class NavMenu
    {
        public IEnumerable<CategoryDto> Categories { get; set; }

        [Inject]
        public ICategoryService CategoryService { get; set; }


        protected async override Task OnInitializedAsync()
        {

            Categories = await CategoryService.GetCategories();
            if (Categories == null)
                Categories = await FakeCategories();
        }

        private async Task<IEnumerable<CategoryDto>> FakeCategories()
        {
            return await Task.Run<IEnumerable<CategoryDto>>(() =>
            {
                return new List<CategoryDto> {
                    new CategoryDto{Name = "1980s" },
                      new CategoryDto{Name = "1990s" },
                        new CategoryDto{Name = "2000s" },
                };
            });
        }
    }
}
