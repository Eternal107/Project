using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MyMapPageViewModel:ViewModelBase
    {
        private EventHandler<MapLongClickedEventArgs> longClicked;
        public EventHandler<MapLongClickedEventArgs> LongClicked
        {
            get { return longClicked; }
            set { SetProperty(ref longClicked, value); }
        }

        private List<Pin> pins= new List<Pin>();

        public List<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }

        public MyMapPageViewModel(INavigationService navigationService, IRepository<User> user)
            : base(navigationService, user)
        {
            Title = "Map";
            LongClicked+=OnLongclicked;
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
                test.Add(new Pin() { Position = new Position(lat, lng), Type = PinType.Place, Label = "kek" });
                Pins = test;
            }
        }

    }
}
