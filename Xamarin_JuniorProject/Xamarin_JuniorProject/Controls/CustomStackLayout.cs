using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Controls
{
    public class CustomStackLayout : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(ObservableCollection<CustomPinView>),
            typeof(CustomStackLayout),
            null,
            propertyChanged: ItemAdded);


        public ObservableCollection<CustomPinView> ItemsSource
        {
            get => (ObservableCollection<CustomPinView>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }





        private static void ItemAdded(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomStackLayout castedMap)
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
                    foreach (var pin in e.NewItems.Cast<CustomPinView>())
                        Children.Add(pin);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var pin in e.OldItems.Cast<CustomPinView>())
                        Children.Remove(pin);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Children.Clear();
                    break;
            }
        }
    }
}
