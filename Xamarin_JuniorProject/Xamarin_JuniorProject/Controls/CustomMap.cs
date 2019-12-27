using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Clustering;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomMap : ClusteredMap
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


        public static readonly BindableProperty MapCameraPositionProperty = BindableProperty.Create(
            nameof(MapCameraPosition),
            typeof(CameraPosition),
            typeof(CustomMap),
            null,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue != null)
                {
                    
                    
                    CameraUpdate update = CameraUpdateFactory.NewCameraPosition((CameraPosition)newValue);
                    (bindable as CustomMap).InitialCameraUpdate=(update);
                    (bindable as CustomMap).MoveCamera(update);
                }
            });


        public CameraPosition MapCameraPosition
        {
            get => (CameraPosition)GetValue(MapCameraPositionProperty);
            set => SetValue(MapCameraPositionProperty, value);
        }

        public static readonly BindableProperty MyMapLongClickedProperty = BindableProperty.Create(
            nameof(MyMapLongClicked),
            typeof(ICommand),
            typeof(CustomMap),
            null);



        public ICommand MyMapLongClicked
        {
            get => (ICommand)GetValue(MyMapLongClickedProperty);
            set => SetValue(MyMapLongClickedProperty, value);
        }


        public static readonly BindableProperty MyMapClickedProperty = BindableProperty.Create(
           nameof(MyPinClicked),
           typeof(ICommand),
           typeof(CustomMap),
           null);


        public ICommand MyMapClicked
        {
            get => (ICommand)GetValue(MyMapClickedProperty);
            set => SetValue(MyMapClickedProperty, value);
        }


        public static readonly BindableProperty MyPinClickedProperty = BindableProperty.Create(
            nameof(MyPinClicked),
            typeof(ICommand),
            typeof(CustomMap),
            null);


        public ICommand MyPinClicked
        {
            get => (ICommand)GetValue(MyPinClickedProperty);
            set => SetValue(MyPinClickedProperty, value);
        }




        public CustomMap()
        {
            MapLongClicked += (o, e) => { MyMapLongClicked?.Execute(e); };
            MapClicked += (o, e) => { MyMapClicked?.Execute(e); };
            PinClicked += (o, e) => { MyPinClicked?.Execute(e); };
            

            this.MyLocationButtonClicked += GetPermission;
            this.MapType = MapType.Street;
            this.UiSettings.MyLocationButtonEnabled = true;
            this.MyLocationEnabled = false;

            this.UiSettings.CompassEnabled = true;

            this.UiSettings.RotateGesturesEnabled = true;

            this.UiSettings.IndoorLevelPickerEnabled = true;

            this.UiSettings.ScrollGesturesEnabled = true;

            this.UiSettings.TiltGesturesEnabled = true;

            this.UiSettings.ZoomControlsEnabled = true;

            this.UiSettings.ZoomGesturesEnabled = true;
            GetPermission(null, null);

        }



        private async void GetPermission(object o, MyLocationButtonClickedEventArgs e)
        {
            if (!MyLocationEnabled)
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

        private void OnPinsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var pin in e.NewItems.Cast<Pin>())
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