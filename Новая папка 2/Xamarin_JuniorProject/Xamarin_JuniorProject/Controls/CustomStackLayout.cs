using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomStackLayout : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(List<View>),
            typeof(CustomStackLayout),
            null,
            propertyChanged: ItemAdded);


        public List<View> ItemsSource
        {
            get => (List<View>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public CustomStackLayout()
        {

        }




        private static void ItemAdded(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CustomStackLayout;
            if (newValue != null)
            {
                control.Children.Clear();
                foreach (var n in (List<View>)newValue)
                    control.Children.Add(n);
            }

        }
    }
}
