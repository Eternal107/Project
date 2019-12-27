using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;
using Xamarin_JuniorProject.ViewModels.ModalViewModels;
using Xamarin_JuniorProject.Views.ModalViews;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MyMapPageViewModel : ViewModelBase
    {






        public ICommand LongClicked => new Command<MapLongClickedEventArgs>(OnLongclicked);



        public ICommand PinClicked => new Command<PinClickedEventArgs>(OnPinClicked);



        public ICommand TextChanged => new Command(OnTextChanged);

        private ObservableCollection<Pin> pins;
        public ObservableCollection<Pin> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
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


        public MyMapPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {

            Pins = new ObservableCollection<Pin>();
            Title = "Map";
            MapCameraPosition=new CameraPosition(new Position(0, 0), 0);
            MessagingCenter.Subscribe<SavePinsPageViewModel,CustomPinView> (this, "AddPin",async (sender,pin) =>
            {
                await LoadFromDataBase();
                var newPin = (await PinService.GetPins(App.CurrentUserId)).LastOrDefault(x => x.ID == pin.PinID);
                var Pin = newPin.ToPin();
                MapCameraPosition = new CameraPosition(Pin.Position, 5);
                if (!Pins.Contains(Pin))
                {
                    Pins.Add(Pin);

                }
                OnPinClicked(Pin);
                MessagingCenter.Subscribe<PinModalView>(this, "DeletePin", (seconSender) =>
                {

                    Pins.Remove(Pins.LastOrDefault());
                    MessagingCenter.Unsubscribe<PinModalView>(this, "DeletePin");
                });
            });

           
        }


        private async void OnPinClicked(Pin pin)
        {
            var p = new NavigationParameters();
            p.Add("SelectedPin", pin);
            await PopupNavigation.Instance.PushAsync(new PinModalView() { BindingContext = new PinModalViewModel(NavigationService, Repository, AuthorizationService, PinService, pin) });
        }

        private async void OnPinClicked( PinClickedEventArgs e)
        {
            var p = new NavigationParameters();
            p.Add("SelectedPin", e.Pin);
            await PopupNavigation.Instance.PushAsync(new PinModalView() { BindingContext = new PinModalViewModel(NavigationService, Repository, AuthorizationService, PinService, e.Pin) });
        }



        private async Task LoadFromDataBase()
        {
            Pins.Clear();

            var PinModels = (await PinService.GetPins(App.CurrentUserId)).Where(x => x.IsFavorite == true);
            if (PinModels != null)
            {

                foreach (PinModel model in PinModels)
                {
                    Pin newPin = new Pin() { Label = model.Name, Position = new Position(model.Latitude, model.Longtitude), Type = model.IsFavorite == true ? PinType.SavedPin : PinType.Place, Tag = model.Description };
                    Pins.Add(newPin);
                }

            }
        }

        private async void OnLongclicked( MapLongClickedEventArgs e)
        {

            var lat = e.Point.Latitude;
            var lng = e.Point.Longitude;
            var pin = Pins.LastOrDefault(x => x.Position == e.Point);
            if (pin == null)
            {
                PromptResult result = await UserDialogs.Instance.PromptAsync(string.Format("{0}, {1}", lat, lng), "Add pin?", "Ok", "Cancel", "Name");
                if (result.Ok)
                {

                    Pins.Add(new Pin() { Position = new Position(lat, lng), Type = PinType.SavedPin, Label = result.Text, Tag = "" });
                    await PinService.AddPin(Pins.Last().ToPinModel((string)Pins.Last().Tag));

                }
            }

        }

        private async void OnTextChanged()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Pins.Clear();
                var Text = SearchText.ToLower();
                var PinModels = (await PinService.GetPins(App.CurrentUserId)).Where(x => x.IsFavorite == true && (x.Name.Contains(Text) || x.Description.Contains(Text) || x.Latitude.ToString().Contains(Text) || x.Longtitude.ToString().Contains(Text)));
                if (PinModels != null)
                {

                    foreach (PinModel model in PinModels)
                    {
                        Pin newPin = new Pin() { Label = model.Name, Position = new Position(model.Latitude, model.Longtitude), Type = model.IsFavorite == true ? PinType.SavedPin : PinType.Place, Tag = model.Description };
                        Pins.Add(newPin);
                    }

                }
            }
            else
            {
                await LoadFromDataBase();
            }
        }




        public override async void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey("LoadFromDataBase"))
            {

                await LoadFromDataBase();
            }
            else if (parameters.ContainsKey("DeletePin"))
            {
                var oldPin = parameters.GetValue<Pin>("DeletePin");
                Pins.Remove(oldPin);

            }
        }
    }
}
