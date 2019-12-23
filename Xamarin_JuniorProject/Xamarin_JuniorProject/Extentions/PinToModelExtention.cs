
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class PinToModelExtention
    {

        public static PinModel ToPinModel(this Pin pin, string description)
        {
            PinModel Model = new PinModel();
            Model.Name = pin.Label;
            Model.Latitude = pin.Position.Latitude;
            Model.Longtitude = pin.Position.Longitude;
            Model.UserID = App.CurrentUserId;
            Model.Description = description;
            Model.IsFavorite = pin.Type == PinType.SavedPin;
            return Model;

        }
    }
}