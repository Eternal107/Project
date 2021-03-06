﻿using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class PinToModelExtention
    {
        public static PinModel ToPinModel(this Pin pin,int categoryId=-1)
        {
            PinModel Model = new PinModel();
            Model.Name = pin.Label;
            Model.Latitude = pin.Position.Latitude;
            Model.Longtitude = pin.Position.Longitude;
            Model.UserID = App.CurrentUserId;
            Model.Description = (string)pin.Tag;
            Model.IsFavorite = pin.Type == PinType.SavedPin;
            Model.ImagePath = pin.Icon != null ? pin.Icon.AbsolutePath : string.Empty;
            Model.CategoryID = categoryId;
            return Model;
        }
    }
}