using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.Pin;


namespace Xamarin_JuniorProject.ViewModels
{
    public class AddPinPageViewModel : ViewModelBase
    {
        private IPinService PinService { get; }

        private bool UpdatePin = false;

        public AddPinPageViewModel(INavigationService navigationService,
                                   IPinService pinService)
                                   : base(navigationService)
        {
            Title = AppResources.PinSettings;
            PinService = pinService;
        }

        #region -- Public properties --

        //TODO: add "Command" word to command names and methods names
        public ICommand AddOrSave => new Command(OnAddOrSaveClicked);

        public ICommand MapClicked => new Command<MapClickedEventArgs>(OnMapclicked);

        public ICommand DeleteImage => new Command(DeletePinImage);

        public ICommand ChangeImage => new Command(ChangeOrAddImage);

        //TODO: Use pin view model
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


        private string _toolbarButtonText = AppResources.Add;
        public string ToolbarButtonText
        {
            get { return _toolbarButtonText; }
            set { SetProperty(ref _toolbarButtonText, value); }
        }
        //TODO: Use pin view model
        private ObservableCollection<Pin> _pins = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        #endregion

        #region -- Private helpers--

        private  void DeletePinImage()
        {
            if(!string.IsNullOrEmpty(CurrentPin.ImagePath))
            {
                CurrentPin.ImagePath = string.Empty;

                Pins.Clear();
                Pins.Add(CurrentPin.ToPin());
            }
        }

        private async void ChangeOrAddImage()
        {
            PickMediaOptions options = new PickMediaOptions();

            options.CustomPhotoSize = 6;
            options.PhotoSize = PhotoSize.Custom;

            var file = await CrossMedia.Current.PickPhotoAsync(options);
            if (file != null)
            {
                CurrentPin.ImagePath = file.Path;

                Pins.Clear();
                Pins.Add(CurrentPin.ToPin());
            }
        }

        private async void OnAddOrSaveClicked()
        {
            var pinModel = await PinService.FindPinModelAsync(Pins.LastOrDefault());

            if (pinModel != null)
            {             
                CurrentPin.ID = pinModel.ID;
                await PinService.UpdatePinAsync(CurrentPin);            
            }
            else
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
        }

        private void OnMapclicked(MapClickedEventArgs e)
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

            if(!string.IsNullOrEmpty(CurrentPin.ImagePath))
            {
                newPin.Icon= BitmapDescriptorFactory.FromStream(File.OpenRead(CurrentPin.ImagePath));
            }
            //TODO: Fix after making current pin a view model
            int tempID = CurrentPin.ID;
            string tempPath=CurrentPin.ImagePath;
            CurrentPin = newPin.ToPinModel();
            CurrentPin.ID = tempID;
            CurrentPin.ImagePath = tempPath;
            Pins.Clear();
            Pins.Add(newPin);

        }

        #endregion

        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(Constants.NavigationParameters.PinSettings, out PinModel pin))
            {
                CurrentPin = pin;
                UpdatePin = true;
                ToolbarButtonText = AppResources.Update;
                MapCameraPosition = new CameraPosition(new Position(CurrentPin.Latitude, CurrentPin.Longtitude), 5);
                Pins.Add(CurrentPin.ToPin());
            }
            else
            {
                CurrentPin = new PinModel()
                {
                    Name = string.Empty,
                    UserID = App.CurrentUserId,
                    Latitude = 1.3541171,
                    Longtitude = 103.8659237,
                    IsFavorite = true,
                    Description = string.Empty
                };

                MapCameraPosition = new CameraPosition(new Position(CurrentPin.Latitude, CurrentPin.Longtitude), 5);
                Pins.Add(CurrentPin.ToPin());
            }

        }

        public override  void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (UpdatePin)
            {
                parameters.Add(Constants.NavigationParameters.LoadFromDataBase, true);
            }
        }

        #endregion

    }
}
