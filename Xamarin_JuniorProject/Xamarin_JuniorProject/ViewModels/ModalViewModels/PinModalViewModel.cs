using Prism.Commands;
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels.ModalViewModels
{
    public class PinModalViewModel:ViewModelBase
    {
        public PinModalViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
        }

        private Pin _currentPin;
        public Pin CurrentPin
        {
            get { return _currentPin; }
            set { SetProperty(ref _currentPin, value); }
        }



        private DelegateCommand _addPinPage;
        public DelegateCommand AddPinPage =>
            _addPinPage ?? (_addPinPage = new DelegateCommand(ToAddPinPage));

        private DelegateCommand _deletePin;
        public DelegateCommand DeletePin =>
            _deletePin ?? (_deletePin = new DelegateCommand(ToDeletePin));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            CurrentPin = parameters.GetValue<Pin>("SelectedPin");
      
        }

        private async void ToDeletePin()
        {
            var p = new NavigationParameters();
            p.Add("DeletePin", CurrentPin);
            var pinModel = (await PinService.GetPins(App.CurrentUserId)).LastOrDefault(x => x.Latitude == CurrentPin.Position.Latitude && x.Longtitude == CurrentPin.Position.Longitude);
            await PinService.DeletePin(pinModel);
            await NavigationService.GoBackAsync(p,useModalNavigation:true);

        }

        private async void ToAddPinPage()
        {

            var pinModel = (await PinService.GetPins(App.CurrentUserId)).LastOrDefault(x=>x.Latitude==CurrentPin.Position.Latitude && x.Longtitude== CurrentPin.Position.Longitude);
            var p = new NavigationParameters();
            p.Add("PinSettings", pinModel);
            await NavigationService.NavigateAsync("AddPinPage", p);

        }
    }
}

