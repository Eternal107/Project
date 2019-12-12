using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomMap:Map
    {
        public static readonly BindableProperty PinSourceProperty = BindableProperty.Create(
            nameof(PinSource),
            typeof(List<Pin>),
            typeof(CustomMap),
            null,
            propertyChanged: ItemAdded);


        public List<Pin> PinSource
        {
            get => (List<Pin>)GetValue(PinSourceProperty);
            set => SetValue(PinSourceProperty, value);
        }


        public static readonly BindableProperty MyMapLongClickedProperty = BindableProperty.Create(
            nameof(MyMapLongClicked),
            typeof(EventHandler<MapLongClickedEventArgs>),
            typeof(CustomMap),
            null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var Map = (CustomMap)bindable;
                Map.MapLongClicked += (EventHandler<MapLongClickedEventArgs>)newValue;
            });



        public EventHandler<MapLongClickedEventArgs> MyMapLongClicked
        {
            get => (EventHandler<MapLongClickedEventArgs>)GetValue(PinSourceProperty);
            set => SetValue(PinSourceProperty, value);
        }







        public CustomMap()
        {
            this.MapType = MapType.Street;

            this.MyLocationEnabled = true;



            this.UiSettings.CompassEnabled = true;

            this.UiSettings.RotateGesturesEnabled = true;

            this.UiSettings.MyLocationButtonEnabled = true;


            // IndoorLevelPickerEnabled
            this.UiSettings.IndoorLevelPickerEnabled = true;

            // ScrollGesturesEnabled

            this.UiSettings.ScrollGesturesEnabled = true;


            // TiltGesturesEnabled

            this.UiSettings.TiltGesturesEnabled = true;


            // ZoomControlsEnabled

            this.UiSettings.ZoomControlsEnabled = true;


            // ZoomGesturesEnabled

            this.UiSettings.ZoomGesturesEnabled = true;

        }





        private static void ItemAdded(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CustomMap;
            if (newValue != null)
            {
                control.Pins.Clear();
                foreach (var n in (List<Pin>)newValue)
                    control.Pins.Add(n);
            }
        }

    }

}

