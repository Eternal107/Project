using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System.Linq;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels.ModalViewModels
{
    public class PinModalViewModel : ViewModelBase
    {
        public PinModalViewModel(INavigationService navigationService,IPinService pinService,Pin pin)
            : base(navigationService)
        {
            CurrentPin = pin;
            PinService = pinService;
        }

        IPinService PinService { get; }

        private Pin _currentPin;
        public Pin CurrentPin
        {
            get { return _currentPin; }
            set { SetProperty(ref _currentPin, value); }
        }




        public DelegateCommand AddPinPage => new DelegateCommand(ToAddPinPage);

        public DelegateCommand DeletePin =>new DelegateCommand(ToDeletePin);

        public DelegateCommand Nfc => new DelegateCommand(ToNfc);

        private async void ToDeletePin()
        {
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.DeletePin, CurrentPin);
            var pinModel = (await PinService.GetPinsAsync(App.CurrentUserId))
                .LastOrDefault(x => x.Latitude == CurrentPin.Position.Latitude && x.Longtitude == CurrentPin.Position.Longitude);
            await PinService.DeletePinAsync(pinModel);

            await NavigationService.GoBackAsync(p, useModalNavigation: true);

        }

        private async void ToNfc()
        {

            await PopupNavigation.Instance.PopAsync();
            await NavigationService.NavigateAsync("NFCModalView");

        }

        private async void ToAddPinPage()
        {

            var pinModel = (await PinService.GetPinsAsync(App.CurrentUserId))
                .LastOrDefault(x => x.Latitude == CurrentPin.Position.Latitude && x.Longtitude == CurrentPin.Position.Longitude);
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.PinSettings, pinModel);
            await PopupNavigation.Instance.PopAsync();
            await NavigationService.NavigateAsync("AddPinPage", p);

        }
        
    }
}

