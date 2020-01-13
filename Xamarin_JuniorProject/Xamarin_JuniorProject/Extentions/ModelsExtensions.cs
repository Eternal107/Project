using System.Windows.Input;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class ModelsExtensions
    {
        public static CategoryViewModel ToViewModel(
            this CategoryModel model,
            ICommand tappedCommand)
        {
            return new CategoryViewModel(
                model.ID,
                model.UserID,
                model.Category,
                tappedCommand);
        }

        public static CategoryModel ToModel(
            this CategoryViewModel model)
        {
            var category = new CategoryModel();
            category.ID = model.ID;
            category.UserID = model.UserID;
            category.Category= model.Category;
            return category;
        }

        public static PinViewModel ToViewModel(this PinModel model)
        {
            return new PinViewModel(
                model.ID,
                model.UserID,
                model.Name,
                model.Description,
                model.Latitude,
                model.Longtitude,
                model.IsFavorite,
                model.ImagePath,
                model.CategoryID);
        }
    }
}
