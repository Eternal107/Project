﻿using System;
using System.IO;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class PinModelToPin
    {
        public static Pin ToPin(this PinModel model)
        {
            Pin pin = new Pin();

            pin.Label = model.Name;
            pin.Tag = model.Description;
            pin.Position = new Position(model.Latitude, model.Longtitude);
            pin.Type = model.IsFavorite ? PinType.SavedPin : PinType.Place;
            if(!string.IsNullOrEmpty(model.ImagePath))
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
