using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using SlideOverKit;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;
using Xamarin_JuniorProject.SlideUp;

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


        private List<Pin> pins = new List<Pin>();

        public List<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }



        public MyMapPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            SliderViewModel = new SliderPageViewModel(navigationService, repository, authorizationService, pinService);
            Title = "Map";
            LongClicked = OnLongclicked;
            PinClicked = OnPinClicked;
            LoadFromDataBase();

        }


        private async void OnPinClicked(object sender, PinClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Pin.Label))
                SliderViewModel.NameText = e.Pin.Label;

            else SliderViewModel.NameText = "Pin Name";

            
            SliderViewModel.DescriptionText = (string)e.Pin.Tag;
            ShowSlider?.Invoke();
        }


        private async void LoadFromDataBase()
        {

            var PinModels = await PinService.GetPins(App.CurrentUserId);
            if (PinModels != null)
            {
                List<Pin> temp = new List<Pin>();
                foreach (PinModel model in PinModels)
                {
                    Pin newPin = new Pin() { Label = model.Name, Position = new Position(model.Latitude, model.Longtitude), Type = model.IsFavorite == true ? PinType.SavedPin : PinType.Place,Tag= model.Description };
                    temp.Add(newPin);
                }
                Pins = temp;
            }
        }

        private async void OnLongclicked(object sender, MapLongClickedEventArgs e)
        {

            var lat = e.Point.Latitude;
            var lng = e.Point.Longitude;

            PromptResult result = await UserDialogs.Instance.PromptAsync(string.Format("{0}, {1}", lat, lng), "Add pin?", "Ok", "Cancel", "Name");
            if (result.Ok)
            {
                List<Pin> temp = new List<Pin>();
                foreach (var n in Pins)
                    temp.Add(n);
                temp.Add(new Pin() { Position = new Position(lat, lng), Type = PinType.Place, Label = result.Text,Tag= "WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW" });
                await PinService.AddPin(temp.Last().ToPinModel((string)temp.Last().Tag));
                Pins = temp;
            }

        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add("PinList", Pins);
        }

    }
}