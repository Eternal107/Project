using System.IO;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class PinViewModelToPin
    {
        public static Pin ToPin(this PinViewModel model)
        {
            Pin pin = new Pin();
            pin.Label = model.Name;
            pin.Tag = model.Description;
            pin.Position = new Position(model.Latitude, model.Longitude);
            pin.Type = model.IsFavorite ? PinType.SavedPin : PinType.Place;
            if (!string.IsNullOrEmpty(model.ImagePath))
            {
                if (File.Exists(model.ImagePath))
                {
                    pin.Icon = BitmapDescriptorFactory.FromStream(File.OpenRead(model.ImagePath));
                }
            }
            return pin;
        }
    }
}
