using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Services.CategoryService
{
    public interface ICategoryService
    {
        Task SaveOrUpdateCategoryAsync(CategoryModel Category);
        Task DeleteCategoryAsync(CategoryModel Category);
        Task<List<CategoryModel>> GetCategoriesAsync(int userId);
    }
}
