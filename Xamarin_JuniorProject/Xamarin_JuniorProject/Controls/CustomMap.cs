using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Clustering;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomMap : ClusteredMap
    {
        public CustomMap()
        {
            this.UiSettings.MyLocationButtonEnabled = true;
            this.UiSettings.CompassEnabled = true;
            this.UiSettings.RotateGesturesEnabled = true;
            this.UiSettings.IndoorLevelPickerEnabled = true;
            this.UiSettings.ScrollGesturesEnabled = true;
            this.UiSettings.TiltGesturesEnabled = true;
            this.UiSettings.ZoomControlsEnabled = true;
            this.UiSettings.ZoomGesturesEnabled = true;
        }

        public static readonly BindableProperty PinSourceProperty = BindableProperty.Create(
            propertyName:    nameof(PinSource),
            returnType:      typeof(ObservableCollection<Pin>),
            declaringType:   typeof(CustomMap),
            defaultValue:    null,
            propertyChanged: ItemAdded);

        public ObservableCollection<Pin> PinSource
        {
            get => (ObservableCollection<Pin>)GetValue(PinSourceProperty);
            set => SetValue(PinSourceProperty, value);
        }

        public static readonly BindableProperty MapCameraPositionProperty = BindableProperty.Create(
            propertyName:    nameof(MapCameraPosition),
            returnType:      typeof(CameraPosition),
            declaringType:   typeof(CustomMap),
            defaultValue:    null,
            propertyChanged: MapCameraPositionPropertyChanged);

        public CameraPosition MapCameraPosition
        {
            get => (CameraPosition)GetValue(MapCameraPositionProperty);
            set => SetValue(MapCameraPositionProperty, value);
        }

        private static void MapCameraPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                CameraUpdate update = CameraUpdateFactory.NewCameraPosition((CameraPosition)newValue);
                (bindable as CustomMap).InitialCameraUpdate = (update);
                (bindable as CustomMap).MoveCamera(update);
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
                    {
                        Pins.Add(pin);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var pin in e.OldItems.Cast<Pin>())
                    {
                        Pins.Remove(pin);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Pins.Clear();
                    break;

            }
        }

    }

}