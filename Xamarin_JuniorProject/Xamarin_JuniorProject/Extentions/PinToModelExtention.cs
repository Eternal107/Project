
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class PinToModelExtention
    {

        public static PinModel ToPinModel(this Pin pin)
        {
            PinModel Model = new PinModel();
            Model.Name = pin.Label;
            Model.Latitude = pin.Position.Latitude;
            Model.Longtitude = pin.Position.Longitude;
            Model.UserID = App.CurrentUserId;
            Model.Description = (string)pin.Tag;
            Model.IsFavorite = pin.Type == PinType.SavedPin;
            return Model;

        }
    }
}