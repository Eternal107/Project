using Acr.UserDialogs;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IAuthorizationService AuthorizationService { get; }

        public MainPageViewModel(INavigationService navigationService,IAuthorizationService authorizationService)
            : base(navigationService)
        {
            Title = "Login Page";
            AuthorizationService = authorizationService;
        }

        #region -- Public properties --

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


        public DelegateCommand ToTabbedPage => new DelegateCommand(PushTabbedPage);

        public DelegateCommand ToRegistrationPage => new DelegateCommand(PushRegistrationPage);

        #endregion

        #region -- Private helpers -

        private async void PushTabbedPage()
        {
            var Loginization = await AuthorizationService.LoginAsync(Login, Password);
            if (Loginization)
            {
                PrismApplicationBase.Current.Properties.Add("LoggedIn", App.CurrentUserId);

                await PrismApplicationBase.Current.SavePropertiesAsync();
                var p = new NavigationParameters();
                p.Add(Constants.NavigationParameters.LoadFromDataBase, true);
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(TabbedMapPage)}", p);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Wrong Login or password");
            }

        }

        private async void PushRegistrationPage()
        {
            await NavigationService.NavigateAsync($"{nameof(RegistrationPage)}");
        }

        #endregion
    }
}