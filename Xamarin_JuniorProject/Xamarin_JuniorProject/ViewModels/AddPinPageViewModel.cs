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
        private IPinService PinService { get; }

        private bool UpdatePin = false;

        public AddPinPageViewModel(INavigationService navigationService,IPinService pinService)
             : base(navigationService)
        {
            Title = "Pin Settings";
            PinService = pinService;
        }

        #region -- Public properties --

        public ICommand AddOrSave => new Command(OnAddOrSaveClicked);

        public ICommand MapClicked => new Command<MapClickedEventArgs>(OnMapclicked);

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

        private ObservableCollection<Pin> _pins = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        #endregion

        #region -- Private helpers--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            
            if (parameters.TryGetValue(Constants.NavigationParameters.PinSettings, out PinModel CurrentPin))
            {
                this.CurrentPin = CurrentPin;
                UpdatePin = true;
                ToolbarButtonText = "Update";
                MapCameraPosition = new CameraPosition(new Position(CurrentPin.Latitude, CurrentPin.Longtitude), 5);
                Pins.Add(CurrentPin.ToPin());
            }
            else if (parameters.TryGetValue(Constants.NavigationParameters.UpdatePin, out CustomPinView PinView))
            {
                
                this.CurrentPin = new PinModel()
                {
                    IsFavorite = PinView.IsFavorite,
                    ID = PinView.PinID,
                    UserID = PinView.UserID, Name = PinView.PinName.Text,
                    Latitude = Convert.ToDouble(PinView.PinLat.Text),
                    Longtitude = Convert.ToDouble(PinView.PinLng.Text),
                    Description = PinView.PinText.Text
                };

                UpdatePin = true;
                MapCameraPosition = new CameraPosition(new Position(CurrentPin.Latitude, CurrentPin.Longtitude), 5);
                ToolbarButtonText = "Update";
                Pins.Add(CurrentPin.ToPin());

            }
            else
            {

                CurrentPin = new PinModel()
                {
                    Name = string.Empty,
                    UserID = App.CurrentUserId,
                    Latitude = 40,
                    Longtitude = 20
                };

                Pins.Add(CurrentPin.ToPin());
            }

        }

        private async void OnAddOrSaveClicked()
        {
            if (UpdatePin)
            {
                await PinService.UpdatePinAsync(CurrentPin);
            }
            else
            {
                await PinService.AddPinAsync(CurrentPin);
            }
        }

        private void OnMapclicked( MapClickedEventArgs e)
        {

            var lat = e.Point.Latitude;
            var lng = e.Point.Longitude;

            var newPin = new Pin()
            {
                Label = CurrentPin.Name,
                Position = new Position(lat, lng),
                Type = CurrentPin.IsFavorite ? PinType.SavedPin : PinType.Place,
                Tag = CurrentPin.Description
            };

            int tempID = CurrentPin.ID;
            CurrentPin = newPin.ToPinModel();
            CurrentPin.ID = tempID;
            Pins.Clear();
            Pins.Add(newPin);


        }

        #endregion

        #region --Overrides--
        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (UpdatePin)
            {
                parameters.Add(Constants.NavigationParameters.LoadFromDataBase, true);
            }
        }

        #endregion

    }
}
