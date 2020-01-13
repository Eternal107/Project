using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.Services.CategoryService
{
    public class CategoryService:ICategoryService
    {
        private IRepositoryService _repositoryService;
        public CategoryService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public Task AddCategoryAsync(CategoryModel category)
        {
            return _repositoryService.InsertAsync(category);
        }

        public Task DeleteCategoryAsync(CategoryModel category)
        {
            return _repositoryService.DeleteAsync(category);        
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync(int UserID)
        {
            var categories= await _repositoryService.GetAsync<CategoryModel>();
            return categories.FindAll(x => x.UserID == UserID);
        }

        public Task UpdateCategoryAsync(CategoryModel category)
        {
            return _repositoryService.UpdateAsync(category);
        }
    }
}
