using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;


namespace Xamarin_JuniorProject.ViewModels
{
    public class MyMapPageViewModel : ViewModelBase
    {





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


        private ObservableCollection<Pin> pins;
        public ObservableCollection<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }



        public MyMapPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {

            Pins = new ObservableCollection<Pin>();
            Title = "Map";
            LongClicked = OnLongclicked;
            PinClicked = OnPinClicked;
 

        }


        private async void OnPinClicked(object sender, PinClickedEventArgs e)
        {

            

            var p = new NavigationParameters();
            p.Add("SelectedPin", e.Pin);
            await NavigationService.NavigateAsync("PinModalView",p,useModalNavigation:true);
            
           
        }


        private async void LoadFromDataBase()
        {
            Pins.Clear();
            
            var PinModels = (await PinService.GetPins(App.CurrentUserId)).Where(x=>x.IsFavorite==true);
            if (PinModels != null)
            {
                
                foreach (PinModel model in PinModels)
                {
                    Pin newPin = new Pin() { Label = model.Name, Position = new Position(model.Latitude, model.Longtitude), Type = model.IsFavorite == true ? PinType.SavedPin : PinType.Place,Tag= model.Description };
                    Pins.Add(newPin);
                }
                
            }
        }

        private async void OnLongclicked(object sender, MapLongClickedEventArgs e)
        {

            var lat = e.Point.Latitude;
            var lng = e.Point.Longitude;
            var pin = Pins.LastOrDefault(x => x.Position == e.Point);
            if (pin==null)
            {
                PromptResult result = await UserDialogs.Instance.PromptAsync(string.Format("{0}, {1}", lat, lng), "Add pin?", "Ok", "Cancel", "Name");
                if (result.Ok)
                {

                    Pins.Add(new Pin() { Position = new Position(lat, lng), Type = PinType.SavedPin, Label = result.Text, Tag = "" });
                    await PinService.AddPin(Pins.Last().ToPinModel((string)Pins.Last().Tag));

                }
            }

        }



        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
          
            if(parameters.ContainsKey("LoadFromDataBase"))
            {
                
                LoadFromDataBase();
            }
            else if(parameters.ContainsKey("DeletePin"))
            {
                var oldPin = parameters.GetValue<Pin>("DeletePin");
                Pins.Remove(oldPin);

            }
        }
    }
}