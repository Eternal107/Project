using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.Pin;


namespace Xamarin_JuniorProject.ViewModels
{
    public class SavePinsPageViewModel : ViewModelBase
    {
        //TODO: You know
        IPinService PinService { get; }

        public SavePinsPageViewModel(INavigationService navigationService,
                                     IPinService pinService)
                                     : base(navigationService)
        {
            PinService = pinService;
            Title = AppResources.SavedPins;
        }

        #region -- Public properties --

        //TODO: Fix view model
        private ObservableCollection<CustomPinView> _pins = new ObservableCollection<CustomPinView>();
        public ObservableCollection<CustomPinView> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        //TODO: come on
        public ICommand TextChanged => new Command(OnTextChanged);
        public ICommand AddPinPage => new Command(ToAddPinPage);

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        #endregion

        #region -- Private helpers--

        private async void ToAddPinPage()
        {
            await NavigationService.NavigateAsync($"{nameof(AddPinPage)}");
        }

        private void ToSetPin(object o, EventArgs e)
        {
            //TODO: Do something with this
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.UpdatePin, (CustomPinView)o);
            MessagingCenter.Send(this, Constants.MessagingCenter.ToFirstPage);
            MessagingCenter.Send(this, Constants.MessagingCenter.AddPin, (CustomPinView)o);
        }

        private async Task LoadFromDataBaseAsync()
        {
            Pins.Clear();
            var MapPins = await PinService.GetPinsAsync(App.CurrentUserId);
            if (MapPins != null)
            {
                foreach (var pin in MapPins)
                {
                    //TODO: Fix view model
                    var PinView = pin.PinModelToPinView();
                    PinView.Tapped = ToSetPin;
                    Pins.Add(PinView);
                }
            }
        }

        private async void OnTextChanged()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Pins.Clear();
                var Text = SearchText.ToLower();
                var MapPins = (await PinService.GetPinsAsync(App.CurrentUserId))
                    .Where(x => x.IsFavorite == true && (x.Name.Contains(Text) || x.Description.Contains(Text)
                    || x.Latitude.ToString().Contains(Text) || x.Longtitude.ToString().Contains(Text)));

                if (MapPins != null)
                {
                    foreach (var pin in MapPins)
                    {
                        //TODO: Fix view model
                        var PinView = pin.PinModelToPinView();
                        PinView.Tapped = ToSetPin;
                        Pins.Add(PinView);
                    }
                }
            }
            else
            {
                await LoadFromDataBaseAsync();
            }
        }
        #endregion
        #region --Overrides--

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadFromDataBaseAsync();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(Constants.NavigationParameters.LoadFromDataBase, true);
        }

        #endregion
    }
}

