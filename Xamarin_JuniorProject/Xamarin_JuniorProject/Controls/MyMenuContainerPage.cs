using System;
using System.Windows.Input;
using Prism.Commands;
using SlideOverKit;
using Xamarin.Forms;
using Xamarin_JuniorProject.SlideUp;
using Xamarin_JuniorProject.ViewModels;

namespace Xamarin_JuniorProject.Controls
{
    public class MyMenuContainerPage<T> : MenuContainerPage where T : SlideMenuView,new()
    {
        public SlideMenuView Slider;
        public MyMenuContainerPage()
        {
            ShowSlider += ShowMenu;
            Slider = new T();
            SlideMenu = Slider;
        }

        public static readonly BindableProperty ShowSliderProperty = BindableProperty.Create(
            nameof(ShowSlider),
            typeof(Action),
            typeof(MyMenuContainerPage<T>),
            null, BindingMode.OneWayToSource);

        public Action ShowSlider
        {
            get => (Action)GetValue(ShowSliderProperty);
            set => SetValue(ShowSliderProperty, value);
        }

        public static readonly BindableProperty SliderBindableContextProperty = BindableProperty.Create(
            nameof(SliderBindableContext),
            typeof(ViewModelBase),
            typeof(MyMenuContainerPage<T>),
            null, propertyChanged: SliderBindingContextChanged);

        public ViewModelBase SliderBindableContext
        {
            get => (ViewModelBase)GetValue(SliderBindableContextProperty);
            set => SetValue(SliderBindableContextProperty, value);
        }

        private static void SliderBindingContextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var Page = bindable as MyMenuContainerPage<T>;
            if (newValue != null)
                Page.Slider.BindingContext = newValue;
            
        }
    }
}
