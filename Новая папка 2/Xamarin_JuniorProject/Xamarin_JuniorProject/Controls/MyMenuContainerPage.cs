using System;
using System.Windows.Input;
using Prism.Commands;
using SlideOverKit;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Controls
{
    public class MyMenuContainerPage:MenuContainerPage
    {
  
        public MyMenuContainerPage()
        {
            ShowSlider += ShowMenu;
        }

        public static readonly BindableProperty ShowSliderProperty = BindableProperty.Create(
            nameof(ShowSlider),
            typeof(Action),
            typeof(MyMenuContainerPage),
            null, BindingMode.TwoWay);
           



        public Action ShowSlider
        {
            get => (Action)GetValue(ShowSliderProperty);
            set => SetValue(ShowSliderProperty, value);
        }
    }
}
