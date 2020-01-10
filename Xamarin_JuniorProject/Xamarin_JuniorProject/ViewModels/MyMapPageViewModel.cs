using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.ViewModels.ModalViewModels;
using Xamarin_JuniorProject.Views.ModalViews;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MyMapPageViewModel : ViewModelBase
    {


        private IPinService PinService { get; }

        public MyMapPageViewModel(INavigationService navigationService,
                                  IPinService pinService)
           : base(navigationService)
        {
            Pins = new ObservableCollection<Pin>();
            MapCameraPosition = new CameraPosition(new Position(0, 0), 0);
            Title = AppResources.Map;
            PinService = pinService;
            MessagingCenter.Subscribe<SavePinsPageViewModel, CustomPinView>(this, Constants.MessagingCenter.AddPin, ShowPin);
        }
        #region -- Public properties --
        public ICommand LongClicked => new Command<MapLongClickedEventArgs>(OnLongclicked);

        public ICommand PinClicked => new Command<PinClickedEventArgs>(OnPinClicked);

        public ICommand TextChanged => new Command(OnTextChanged);

        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        private CameraPosition _mapCameraPosition;
        public CameraPosition MapCameraPosition
        {
            get { return _mapCameraPosition; }
            set { SetProperty(ref _mapCameraPosition, value); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        #endregion

        #region -- Private helpers--

        private async void ShowPin(SavePinsPageViewModel sender,CustomPinView pin)
        {
            await LoadFromDataBase();
            var newPin = (await PinService.GetPinsAsync(App.CurrentUserId)).LastOrDefault(x => x.ID == pin.PinID);
            var Pin = newPin.ToPin();
            MapCameraPosition = new CameraPosition(Pin.Position, 5);
            if (!Pins.Contains(Pin))
            {
                Pins.Add(Pin);
            }

            OnPinClicked(Pin);

            MessagingCenter.Subscribe<PinModalView>(this, Constants.MessagingCenter.DeletePin, (seconSender) =>
            {
                Pins.Remove(Pins.LastOrDefault());
                MessagingCenter.Unsubscribe<PinModalView>(this, Constants.MessagingCenter.DeletePin);
            });
        }
        
        private async void OnPinClicked(Pin pin)
        {
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.SelectedPin, pin);
            await PopupNavigation.Instance.PushAsync(new PinModalView() { BindingContext = new PinModalViewModel(NavigationService, PinService, pin) });
        }

        private async void OnPinClicked(PinClickedEventArgs e)
        {
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.SelectedPin, e.Pin);
            await PopupNavigation.Instance.PushAsync(new PinModalView() { BindingContext = new PinModalViewModel(NavigationService,PinService, e.Pin) });
        }

        private async Task LoadFromDataBase()
        {
            Pins.Clear();

            var PinModels = (await PinService.GetPinsAsync(App.CurrentUserId)).Where(x => x.IsFavorite == true);
            if (PinModels != null)
            {
                foreach (PinModel model in PinModels)
                {                  
                    Pins.Add(model.ToPin());
                }
            }
        }

        private async void OnLongclicked(MapLongClickedEventArgs e)
        {

            var lat = e.Point.Latitude;
            var lng = e.Point.Longitude;
            var pin = Pins.LastOrDefault(x => x.Position == e.Point);
            if (pin == null)
            {
    
                PromptResult result = await UserDialogs.Instance.PromptAsync(string.Format("{0}, {1}", lat, lng), AppResources.DoYouWantToAddNewPin, AppResources.OK, AppResources.Cancel, AppResources.Name);
                if (result.Ok)
                {
                    Pins.Add(new Pin() { Position = new Position(lat, lng), Type = PinType.SavedPin, Label = result.Text, Tag = string.Empty });
                    await PinService.AddPinAsync(Pins.Last().ToPinModel());
                }
            }

        }

        private async void OnTextChanged()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Pins.Clear();
                var Text = SearchText.ToLower();
                var PinModels = (await PinService.GetPinsAsync(App.CurrentUserId))
                    .Where(x => x.IsFavorite == true && (x.Name.Contains(Text) ||
                    x.Description.Contains(Text) || x.Latitude.ToString().Contains(Text)
                    || x.Longtitude.ToString().Contains(Text)));

                if (PinModels != null)
                {
                    foreach (PinModel model in PinModels)
                    {
                        Pins.Add(model.ToPin());
                    }
                }
            }
            else
            {
                await LoadFromDataBase();
            }
        }

        #endregion

        #region --Overrides--

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey(Constants.NavigationParameters.LoadFromDataBase))
            {
                await LoadFromDataBase();
            }
            else if (parameters.TryGetValue(Constants.NavigationParameters.DeletePin, out Pin oldPin))
            {
                Pins.Remove(oldPin);
            }
        }

        #endregion
    }
}
