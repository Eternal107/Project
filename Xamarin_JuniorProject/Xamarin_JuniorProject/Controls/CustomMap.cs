using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Clustering;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomMap:ClusteredMap
    {
        public static readonly BindableProperty PinSourceProperty = BindableProperty.Create(
            nameof(PinSource),
            typeof(ObservableCollection<Pin>),
            typeof(CustomMap),
            null,
            propertyChanged: ItemAdded);


        public ObservableCollection<Pin> PinSource
        {
            get => (ObservableCollection<Pin>)GetValue(PinSourceProperty);
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
                if(newValue!=null)
                {
                    Map.MapLongClicked -= (EventHandler<MapLongClickedEventArgs>)oldValue;
                    Map.MapLongClicked += (EventHandler<MapLongClickedEventArgs>)newValue;
                }
               
            });



        public EventHandler<MapLongClickedEventArgs> MyMapLongClicked
        {
            get => (EventHandler<MapLongClickedEventArgs>)GetValue(PinSourceProperty);
            set => SetValue(PinSourceProperty, value);
        }


        public static readonly BindableProperty MyPinClickedProperty = BindableProperty.Create(
            nameof(MyPinClicked),
            typeof(EventHandler<PinClickedEventArgs>),
            typeof(CustomMap),
            null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var Map = (CustomMap)bindable;
                if (newValue != null)
                {
                    Map.PinClicked -= (EventHandler<PinClickedEventArgs>)oldValue;
                    Map.PinClicked += (EventHandler<PinClickedEventArgs>)newValue;
                }

            });



        public EventHandler<PinClickedEventArgs> MyPinClicked
        {
            get => (EventHandler<PinClickedEventArgs>)GetValue(PinSourceProperty);
            set => SetValue(PinSourceProperty, value);
        }




        public CustomMap()
        {
           
            this.MapType = MapType.Street;

            this.MyLocationEnabled = false;

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
            if (bindable is CustomMap castedMap)
            {
                if (oldValue is INotifyCollectionChanged castedOldCollection)
                {
                    castedOldCollection.CollectionChanged -= castedMap.OnPinsCollectionChanged;
                }

                if (newValue is INotifyCollectionChanged castedNewCollection)
                {
                    castedNewCollection.CollectionChanged += castedMap.OnPinsCollectionChanged;
                }
            }
        }

        private async void OnPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(var pin in e.NewItems.Cast<Pin>())
                    Pins.Add(pin);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var pin in e.OldItems.Cast<Pin>())
                        Pins.Remove(pin);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Pins.Clear();
                    break;
                    
            }
        }

    }

}

