using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Helpers;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.Pin;


namespace Xamarin_JuniorProject.ViewModels
{
    public class AddPinPageViewModel : ViewModelBase
    {
        private IPinService _pinService { get; }

        private bool _updatePin = false;

        public AddPinPageViewModel(INavigationService navigationService,
                                   IPinService pinService)
                                   : base(navigationService)
        {
            Title = AppResources.PinSettings;
            _pinService = pinService;
        }

        #region -- Public properties --

        public ICommand AddOrSaveCommand => ExtendedCommand.Create(OnAddOrSaveClickedCommand);

        public ICommand MapClickedCommand => new Command<Position>(OnMapClickedCommand);

        public ICommand DeleteImageCommand => new Command(OnDeletePinImageCommand);

        public ICommand ChangeImageCommand => ExtendedCommand.Create(OnChangeOrAddImageCommand);

        public ICommand CancelButtonCommand => ExtendedCommand.Create(OnCancelButtonCommand);

        private PinViewModel _currentPin;
        public PinViewModel CurrentPin
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
        private ObservableCollection<Pin> _pins = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        #endregion

        #region -- Private helpers--

        private Task OnCancelButtonCommand()
        {
            return NavigationService.GoBackAsync();
        }

        private void OnDeletePinImageCommand()
        {
            if(!string.IsNullOrEmpty(CurrentPin.ImagePath))
            {
                CurrentPin.ImagePath = string.Empty;

                Pins.Clear();
                Pins.Add(CurrentPin.ToPin());
            }
        }

        private async Task OnChangeOrAddImageCommand()
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

        private async Task OnAddOrSaveClickedCommand()
        {
            var pinModel = await _pinService.FindPinModelAsync(Pins.LastOrDefault());
            CurrentPin.CategoryID = -1;

            if (pinModel != null)
            {             
                CurrentPin.ID = pinModel.ID;
                await _pinService.SaveOrUpdatePinAsync(CurrentPin.ToModel());            
            }
            else
            {
                    await _pinService.SaveOrUpdatePinAsync(CurrentPin.ToModel());
            }

            await NavigationService.GoBackAsync();
        }

        private void OnMapClickedCommand(Position point)
        {
            var lat = point.Latitude;
            var lng = point.Longitude;

            CurrentPin.Latitude = lat;
            CurrentPin.Longitude = lng;

            var newPin = CurrentPin.ToPin();

            Pins.Clear();
            Pins.Add(newPin);
        }

        #endregion

        #region --Overrides--

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(Constants.NavigationParameters.PinSettings, out PinModel pin))
            {
                CurrentPin = pin.ToViewModel();
                _updatePin = true;
                ToolbarButtonText = AppResources.Update;
                MapCameraPosition = new CameraPosition(new Position(CurrentPin.Latitude, CurrentPin.Longitude), 5);
                Pins.Add(CurrentPin.ToPin());
            }
            else
            {
                CurrentPin = new PinViewModel()
                {
                    Name = string.Empty,
                    UserID = App.CurrentUserId,
                    Latitude = 1.3541171,
                    Longitude = 103.8659237,
                    IsFavorite = true,
                    Description = string.Empty
                };

                MapCameraPosition = new CameraPosition(new Position(CurrentPin.Latitude, CurrentPin.Longitude), 5);
                Pins.Add(CurrentPin.ToPin());
            }
        }

        public override  void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (_updatePin)
            {
                parameters.Add(Constants.NavigationParameters.LoadFromDataBase, true);
            }
        }

        #endregion

    }
}
