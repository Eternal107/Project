using Prism;
using Prism.Commands;
using Prism.Navigation;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            Title = "Login Page";

        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private DelegateCommand _tabbedPage;
        public DelegateCommand ToTabbedPage =>
            _tabbedPage ?? (_tabbedPage = new DelegateCommand(PushTabbedPage));

        private DelegateCommand RegistrationPage;
        public DelegateCommand ToRegistrationPage =>
            RegistrationPage ?? (RegistrationPage = new DelegateCommand(PushRegistrationPage));



        private async void PushTabbedPage()
        {
            var Loginization = await AuthorizationService.Login(Login, Password);
            if (Loginization)
            {
                PrismApplicationBase.Current.Properties.Add("LoggedIn", App.CurrentUserId);
                await PrismApplicationBase.Current.SavePropertiesAsync();
                await NavigationService.NavigateAsync("/NavigationPage/TabbedMapPage");
            }

        }
        private async void PushRegistrationPage()
        {
            await NavigationService.NavigateAsync("NavigationPage/RegistrationPage");
        }
    }
}