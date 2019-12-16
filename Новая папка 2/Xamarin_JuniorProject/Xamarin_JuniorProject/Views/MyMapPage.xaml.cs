
using System;
using SlideOverKit;
using Xamarin.Forms;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.SlideUp;

namespace Xamarin_JuniorProject.Views
{
    public partial class MyMapPage : MenuContainerPage
    {
        public MyMapPage()
        {
            InitializeComponent();




            this.SlideMenu = new MyPage();

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






