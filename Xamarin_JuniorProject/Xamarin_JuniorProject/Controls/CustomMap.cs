using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
            this.MyLocationButtonClicked += GetPermission;
            this.MapType = MapType.Street;

            this.MyLocationEnabled = false;

            this.UiSettings.CompassEnabled = true;

            this.UiSettings.RotateGesturesEnabled = true;

            this.UiSettings.MyLocationButtonEnabled = true;

            this.UiSettings.IndoorLevelPickerEnabled = true;

            this.UiSettings.ScrollGesturesEnabled = true;

            this.UiSettings.TiltGesturesEnabled = true;

            this.UiSettings.ZoomControlsEnabled = true;
            
            this.UiSettings.ZoomGesturesEnabled = true;
          
        }



        private async void GetPermission (object o, MyLocationButtonClickedEventArgs e)
        {if (!MyLocationEnabled)
            {
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                    if (status != PermissionStatus.Granted)
                    {

                        status = (await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location))[Permission.Location];
                    }

                    if (status == PermissionStatus.Granted)
                    {
                        this.MyLocationEnabled = true;
                    }
                    else 
                    {
                        this.MyLocationEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    //Something went wrong
                }
            }
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

        private  void OnPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

