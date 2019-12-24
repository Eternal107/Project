using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class AddPinPageViewModel:ViewModelBase
    {
        public AddPinPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
             : base(navigationService, repository, authorizationService, pinService)
        {
            Title = "Pin Settings";
            LongClicked = OnMapLongclicked;
        }

        private Pin _innitialPin;

        private Pin _currentPin;
        public Pin CurrentPin
        {
            get { return _currentPin; }
            set { SetProperty(ref _currentPin, value); }
        }

        private ObservableCollection<Pin> pins = new ObservableCollection<Pin>();

        public ObservableCollection<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }

        private EventHandler<MapLongClickedEventArgs> longClicked;
        public EventHandler<MapLongClickedEventArgs> LongClicked
        {
            get { return longClicked; }
            set { SetProperty(ref longClicked, value); }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.ContainsKey("PinSettings"))
            {
                CurrentPin = parameters.GetValue<Pin>("PinSettings");
                if (CurrentPin.Tag == null)
                    CurrentPin.Tag = "";
                _innitialPin = CurrentPin;
                Pins.Add(CurrentPin);
            }
          
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add("InnitialPin", _innitialPin);
            parameters.Add("ChangedPin", CurrentPin);
            PinService.AddPin(CurrentPin.ToPinModel((string)CurrentPin.Tag));
       

        }

        private  void OnMapLongclicked(object sender, MapLongClickedEventArgs e)
        {

            var lat = e.Point.Latitude;
            var lng = e.Point.Longitude;

            var newPin = new Pin() { Label = CurrentPin.Label ,Position = new Position(lat, lng), Type = PinType.SavedPin,Tag=CurrentPin.Tag };
            
            CurrentPin = newPin;
            Pins.Clear();
            Pins.Add(newPin);
          

        }
    }
}
