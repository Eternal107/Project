using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomPinView : StackLayout
    {
        private TapGestureRecognizer Tap = new TapGestureRecognizer();

        public CustomPinView()
        {
            GestureRecognizers.Add(Tap);
            Children.Add(PinName);
            Children.Add(PinLat);
            Children.Add(PinLng);
            Children.Add(PinText);
            Children.Add(PinImage);
        }

        public static readonly BindableProperty ImagePathProperty = BindableProperty.Create(
            nameof(ImageSource),
            typeof(string),
            typeof(CustomPinView),
            null,
            propertyChanged: ImagePathChanged);

        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        #region -- Public properties --

        public Label PinName = new Label() { HorizontalOptions = LayoutOptions.Center };
        public Label PinLat = new Label() { HorizontalOptions = LayoutOptions.CenterAndExpand };
        public Label PinLng = new Label() { HorizontalOptions = LayoutOptions.CenterAndExpand };
        public Editor PinText = new Editor() { AutoSize = EditorAutoSizeOption.TextChanges, IsEnabled = false };
        public Image PinImage = new Image() { IsVisible = false };
        public bool IsFavorite;
        public int PinID;
        public int UserID;

        public static readonly BindableProperty TappedProperty =
        BindableProperty.Create(nameof(Tapped), typeof(EventHandler), typeof(CustomPinView), null, propertyChanged: TappedPropertyChanged);
        public EventHandler Tapped
        {
            set { SetValue(TappedProperty, value); }
            get { return (EventHandler)GetValue(TappedProperty); }
        }

        #endregion

        private static void TappedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customPin = (CustomPinView)bindable;

            if (oldValue != null)
            {
                customPin.Tap.Tapped -= (oldValue as EventHandler).Invoke;
            }
            if (newValue!=null)
            {
                customPin.Tap.Tapped += (newValue as EventHandler).Invoke;
            }
        }

        private static void ImagePathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var customPinView = bindable as CustomPinView;
            if (newValue != null)
            {
                customPinView.PinImage.IsVisible = true;
                customPinView.PinImage.Source = (string)newValue;
            }
        }

    }
}