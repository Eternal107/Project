using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class AddPinPageViewModel : ViewModelBase
    {
        public AddPinPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
             : base(navigationService, repository, authorizationService, pinService)
        {
            Title = "Pin Settings";
          
        }

        private bool UpdatePin = false;

        public DelegateCommand AddOrSave => new DelegateCommand(OnAddOrSaveClicked);

        private PinModel _currentPin;
        public PinModel CurrentPin
        {
            get { return _currentPin; }
            set { SetProperty(ref _currentPin, value); }
        }

        private CameraPosition _mapCameraPosition;
        public CameraPosition MapCameraPosition
        {
            get { return _mapCameraPosition; }
            set { SetProperty(ref _mapCameraPosition, value); }
        }


        private string _toolbarButtonText = "Add";
        public string ToolbarButtonText
        {
            get { return _toolbarButtonText; }
            set { SetProperty(ref _toolbarButtonText, value); }
        }

        private ObservableCollection<Pin> pins = new ObservableCollection<Pin>();

        public ObservableCollection<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }

        public ICommand MapClicked => new Command<MapClickedEventArgs>(OnMapclicked);
        

        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey("PinSettings"))
            {
                CurrentPin = parameters.GetValue<PinModel>("PinSettings");
                UpdatePin = true;
                ToolbarButtonText = "Update";
                MapCameraPosition = new CameraPosition(new Position(CurrentPin.Latitude, CurrentPin.Longtitude), 5);
                Pins.Add(CurrentPin.ToPin());
            }
            else if (parameters.ContainsKey("UpdatePin"))
            {
                var PinView = parameters.GetValue<CustomPinView>("UpdatePin");
                CurrentPin = new PinModel() { IsFavorite = PinView.IsFavorite, ID = PinView.PinID, UserID = PinView.UserID, Name = PinView.PinName.Text, Latitude = Convert.ToDouble(PinView.PinLat.Text), Longtitude = Convert.ToDouble(PinView.PinLng.Text), Description = PinView.PinText.Text };
                UpdatePin = true;
                MapCameraPosition = new CameraPosition(new Position(CurrentPin.Latitude, CurrentPin.Longtitude), 5);
                ToolbarButtonText = "Update";
                Pins.Add(CurrentPin.ToPin());

            }
            else
            {

                CurrentPin = new PinModel() { Name = "", UserID = App.CurrentUserId, Latitude = 40, Longtitude = 20 };

                Pins.Add(CurrentPin.ToPin());
            }

        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (UpdatePin)
            {
                parameters.Add("LoadFromDataBase", true);
            }
        }

        private async void OnAddOrSaveClicked()
        {
            if (UpdatePin)
                await PinService.UpdatePin(CurrentPin);
            else await PinService.AddPin(CurrentPin);
        }

        private void OnMapclicked( MapClickedEventArgs e)
        {

            var lat = e.Point.Latitude;
            var lng = e.Point.Longitude;

            var newPin = new Pin() { Label = CurrentPin.Name, Position = new Position(lat, lng), Type = CurrentPin.IsFavorite ? PinType.SavedPin : PinType.Place, Tag = CurrentPin.Description };
            int tempID = CurrentPin.ID;
            CurrentPin = newPin.ToPinModel((string)newPin.Tag);
            CurrentPin.ID = tempID;
            Pins.Clear();
            Pins.Add(newPin);


        }
    }
}
