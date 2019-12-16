using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using SlideOverKit;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Repository;
using Xamarin_JuniorProject.SlideUp;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MyMapPageViewModel:ViewModelBase
    {

        private Action test;
        public Action Test
        {
            get { return test; }
            set { SetProperty(ref test, value); }
        }





        private EventHandler<MapLongClickedEventArgs> longClicked;
        public EventHandler<MapLongClickedEventArgs> LongClicked
        {
            get { return longClicked; }
            set { SetProperty(ref longClicked, value); }
        }

        private EventHandler<PinClickedEventArgs> pinClicked;
        public EventHandler<PinClickedEventArgs> PinClicked
        {
            get { return pinClicked; }
            set { SetProperty(ref pinClicked, value); }
        }


        private List<Pin> pins= new List<Pin>();

        public List<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }

        private SlideMenuView slideMenu = new MyPage();

        public SlideMenuView SlideMenu
        {
            get { return slideMenu; }
            set { SetProperty(ref slideMenu, value); }
        }

        public MyMapPageViewModel(INavigationService navigationService, IRepositoryService repositoryService, IAuthorizationService authorizationService)
            : base(navigationService, repositoryService, authorizationService)
        {
            Title = "Map";
            LongClicked=OnLongclicked;
            PinClicked = OnPinClicked;
            
        }


        private async void OnPinClicked(object sender, PinClickedEventArgs e)
        {
            Console.WriteLine(e.Pin.Label);
            Test?.Invoke();
        }




private async void OnLongclicked (object sender,MapLongClickedEventArgs e)
        {
            
            var lat = e.Point.Latitude;
            var lng = e.Point.Longitude;
           
            PromptResult result = await UserDialogs.Instance.PromptAsync(string.Format("{0}, {1}",lat,lng),"Add pin?","Ok","Cancel","Name");
            if (result.Ok)
            {
                List<Pin> test = new List<Pin>();
                foreach (var n in Pins)
                    test.Add(n);
                test.Add(new Pin() { Position = new Position(lat, lng), Type = PinType.Place, Label = result.Text });
                Pins = test;
            }
            
        }

    }
}
