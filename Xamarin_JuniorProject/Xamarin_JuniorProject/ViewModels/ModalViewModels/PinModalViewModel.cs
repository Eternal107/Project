using Prism.Commands;
using Prism.Navigation;
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


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            CurrentPin = parameters.GetValue<Pin>("SelectedPin");
        }

        private async void ToAddPinPage()
        {
            var p = new NavigationParameters();
            p.Add("PinSettings", CurrentPin);

            await NavigationService.NavigateAsync("AddPinPage", p);

        }
    }
}

