using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Services.CategoryService
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryModel Category);
        Task DeleteCategoryAsync(CategoryModel Category);
        Task UpdateCategoryAsync(CategoryModel pin);
        Task<List<CategoryModel>> GetCategoriesAsync(int userId);
    }
}
