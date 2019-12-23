using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomStackLayout : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(List<CustomPinView>),
            typeof(CustomStackLayout),
            null,
            propertyChanged: ItemAdded);


        public List<CustomPinView> ItemsSource
        {
            get => (List<CustomPinView>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }





        private static void ItemAdded(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CustomStackLayout;
            if (newValue != null)
            {
                control.Children.Clear();
                foreach (var n in (List<CustomPinView>)newValue)
                    control.Children.Add(n);
            }

        }
    }
}
