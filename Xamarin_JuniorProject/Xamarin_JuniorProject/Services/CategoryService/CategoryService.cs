using System;
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

        public Task SaveOrUpdateCategoryAsync(CategoryModel category)
        {   
            return _repositoryService.SaveOrUpdateAsync(category);
        }

        public Task DeleteCategoryAsync(CategoryModel category)
        {
            try
            {
                return _repositoryService.DeleteAsync(category);
            }
            catch(SQLite.SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync(int UserID)
        {
            List<CategoryModel> categories = new List<CategoryModel>();

            try
            {
                categories = await _repositoryService.GetAsync<CategoryModel>();
                return categories.FindAll(x => x.UserID == UserID);
            }
            catch(SQLite.SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }

            return categories.FindAll(x => x.UserID == UserID);
        }

    }
}
