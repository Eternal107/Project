using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Models;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomPinView : StackLayout
    {

        public Label PinName = new Label() { HorizontalOptions = LayoutOptions.Center };
        public Label PinLat = new Label() { HorizontalOptions = LayoutOptions.CenterAndExpand };
        public Label PinLng = new Label() { HorizontalOptions = LayoutOptions.CenterAndExpand };
        public Editor PinText = new Editor() { AutoSize = EditorAutoSizeOption.TextChanges, IsEnabled = false };
        public bool IsFavorite;
        public int PinID;
        public int UserID;
        public static readonly BindableProperty TappedProperty =
         BindableProperty.Create(nameof(Tapped), typeof(EventHandler), typeof(CustomPinView), null, propertyChanged: (bindable, oldValue, newValue) =>
         {
             var customPin = (CustomPinView)bindable;

             customPin.Tap.Tapped += (sender, e) =>
             {
                 (newValue as EventHandler)?.Invoke(sender, e);
             };

         });


        public EventHandler Tapped
        {
            set { SetValue(TappedProperty, value); }
            get { return (EventHandler)GetValue(TappedProperty); }
        }

        private TapGestureRecognizer Tap = new TapGestureRecognizer();
        public CustomPinView()
        {
            GestureRecognizers.Add(Tap);
            Children.Add(PinName);
            Children.Add(PinLat);
            Children.Add(PinLng);
            Children.Add(PinText);

        }

    }

}