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

        private Action _showSlider;
        public Action ShowSlider
        {
            get { return _showSlider; }
            set { SetProperty(ref _showSlider, value); }
        }

        private Action _hideSlider;
        public Action HideSlider
        {
            get { return _hideSlider; }
            set { SetProperty(ref _hideSlider, value); }
        }

        private SliderPageViewModel _sliderViewModel;
        public SliderPageViewModel SliderViewModel
        {
            get { return _sliderViewModel; }
            set { SetProperty(ref _sliderViewModel, value); }
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


        private ObservableCollection<Pin> pins;
        public ObservableCollection<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }



        public MyMapPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            SliderViewModel = new SliderPageViewModel(navigationService, repository, authorizationService, pinService);
            Pins = new ObservableCollection<Pin>();
            Title = "Map";
            LongClicked = OnLongclicked;
            PinClicked = OnPinClicked;
            LoadFromDataBase();

        }


        private async void OnPinClicked(object sender, PinClickedEventArgs e)
        {

            var p = new NavigationParameters();
            p.Add("SelectedPin", e.Pin);
            await NavigationService.NavigateAsync("PinModalView",p,useModalNavigation:true);
            
           
        }


        private async void LoadFromDataBase()
        {

            var PinModels = await PinService.GetPins(App.CurrentUserId);
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

            PromptResult result = await UserDialogs.Instance.PromptAsync(string.Format("{0}, {1}", lat, lng), "Add pin?", "Ok", "Cancel", "Name");
            if (result.Ok)
            {

                Pins.Add(new Pin() { Position = new Position(lat, lng), Type = PinType.Place, Label = result.Text,Tag= "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW" });
                await PinService.AddPin(Pins.Last().ToPinModel((string)Pins.Last().Tag));
               
            }

        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            
            
            parameters.Add("PinList", Pins);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
          
            if(parameters.ContainsKey("ChangedPin") && parameters.ContainsKey("InnitialPin"))
            {
                var innitialPin = parameters.GetValue<Pin>("InnitialPin");
                var newPin = parameters.GetValue<Pin>("ChangedPin");

                Pins.Remove(innitialPin);
                Pins.Add(newPin);
              
               
            }
        }
    }
}