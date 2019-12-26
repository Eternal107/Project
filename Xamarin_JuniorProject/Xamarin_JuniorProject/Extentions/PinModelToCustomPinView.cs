using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Extentions
{
    public static class PinModelToCustomPinView
    {
        public static CustomPinView PinModelToPinView(this PinModel model)
        {
            var PinView = new CustomPinView();
            PinView.UserID = model.UserID;
            PinView.PinID = model.ID;
            PinView.PinName.Text = model.Name;
            PinView.PinLat.Text = model.Latitude.ToString();
            PinView.PinLng.Text = model.Longtitude.ToString();
            PinView.PinText.Text = model.Description;
            PinView.IsFavorite = model.IsFavorite;
            return PinView;
        }
    }
}
