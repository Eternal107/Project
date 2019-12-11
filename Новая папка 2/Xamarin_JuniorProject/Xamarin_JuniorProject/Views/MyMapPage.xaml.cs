using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Xamarin_JuniorProject.Views
{
    public partial class MyMapPage : ContentPage
        {
            public MyMapPage()
            {
                InitializeComponent();

                // MapTypes


                map.MapType = MapType.Street;

            

                map.MyLocationEnabled = false;


                // IndoorEnabled

                map.IsIndoorEnabled = true;


                // CompassEnabled

                map.UiSettings.CompassEnabled = true;


                // RotateGesturesEnabled

                map.UiSettings.RotateGesturesEnabled = true;


                // MyLocationButtonEnabled

                map.UiSettings.MyLocationButtonEnabled = true;


                // IndoorLevelPickerEnabled

                map.UiSettings.IndoorLevelPickerEnabled = true;


                // ScrollGesturesEnabled

                map.UiSettings.ScrollGesturesEnabled = true;


                // TiltGesturesEnabled

                map.UiSettings.TiltGesturesEnabled = true;


                // ZoomControlsEnabled

                map.UiSettings.ZoomControlsEnabled = true;


                // ZoomGesturesEnabled

                map.UiSettings.ZoomGesturesEnabled = true;



                // Map Clicked
                map.MapClicked += (sender, e) =>
                {
                    var lat = e.Point.Latitude.ToString("0.000");
                    var lng = e.Point.Longitude.ToString("0.000");
                    this.DisplayAlert("MapClicked", $"{lat}/{lng}", "CLOSE");
                   

                };

                // Map Long clicked
                map.MapLongClicked += (sender, e) =>
                {
                    var lat = e.Point.Latitude.ToString("0.000");
                    var lng = e.Point.Longitude.ToString("0.000");
                    this.DisplayAlert("MapLongClicked", $"{lat}/{lng}", "CLOSE");
                };

            }
        }
}

    

