using System.Windows.Input;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class ToPinViewViewModel
    {
        public static PinViewViewModel ToViewViewModel(this PinModel model,ICommand command)
        {
            return new PinViewViewModel(
                model.ID,
                model.Name,
                model.Description,
                model.Latitude,
                model.Longtitude,
                model.ImagePath,
                command);
        }
    }
}
