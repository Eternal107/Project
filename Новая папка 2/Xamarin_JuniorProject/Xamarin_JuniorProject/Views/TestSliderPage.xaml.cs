using System;
using System.Collections.Generic;
using SlideOverKit;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Views
{
    public partial class TestSliderPage : MenuContainerPage
    { 
        public TestSliderPage()
        {
            ShowSlider += ShowMenu;
        }

        public static readonly BindableProperty ShowSliderProperty = BindableProperty.Create(
            nameof(ShowSlider),
            typeof(Action),
            typeof(TestSliderPage),
            null, BindingMode.OneWayToSource);




        public Action ShowSlider
        {
            get => (Action)GetValue(ShowSliderProperty);
            set => SetValue(ShowSliderProperty, value);
        }
    }
}

