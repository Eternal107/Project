using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels
{
    public class SavePinsPageViewModel : ViewModelBase
    {
        IPinService PinService { get; }

        public SavePinsPageViewModel(INavigationService navigationService, IPinService pinService)
            : base(navigationService)
        {
            //TODO: to resources
            PinService = pinService;
            Title = "Saved Pins";
        }

        private ObservableCollection<CustomPinView> _pins = new ObservableCollection<CustomPinView>();
        public ObservableCollection<CustomPinView> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        public ICommand TextChanged => new Command(OnTextChanged);

        public DelegateCommand AddPinPage => new DelegateCommand(ToAddPinPage);

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }


        private async void ToAddPinPage()
        {
            await NavigationService.NavigateAsync($"{nameof(AddPinPage)}");
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadFromDataBaseAsync();
        }

        private async void ToSetPin(object o, EventArgs e)
        {
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.UpdatePin, (CustomPinView)o);
            //TODO: constants
            MessagingCenter.Send(this, "ToFirstPage");
            MessagingCenter.Send(this, "AddPin", (CustomPinView)o);
        }

        private async Task LoadFromDataBaseAsync()
        {
            Pins.Clear();
            var MapPins = await PinService.GetPinsAsync(App.CurrentUserId);
            if (MapPins != null)
            {
                foreach (var pin in MapPins)
                {
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

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(Constants.NavigationParameters.LoadFromDataBase, true);
        }
    }
}

