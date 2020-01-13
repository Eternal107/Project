using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class PinViewModelToModel
    {
        public static PinModel ToModel(this PinViewModel model)
        {
            return new PinModel()
            {
                ID = model.ID,
                UserID = model.UserID,
                Name = model.Name,
                Latitude = model.Latitude,
                Longtitude = model.Longitude,
                Description = model.Description,
                IsFavorite = model.IsFavorite,
                ImagePath = model.ImagePath,
                CategoryID = model.CategoryID
            };
        }
    }
}
