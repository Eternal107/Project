using Acr.UserDialogs;
using Prism;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        //TODO: make another name
        private IAuthorizationService AuthorizationService { get; }

        public LoginPageViewModel(INavigationService navigationService,
                                 IAuthorizationService authorizationService)
                                 : base(navigationService)
        {
            Title = AppResources.LoginPage;
            AuthorizationService = authorizationService;
        }

        #region -- Public properties --


        public ICommand ToTabbedPage => new Command(PushTabbedPage);

        public ICommand ToRegistrationPage => new Command(PushRegistrationPage);

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

        #endregion

        #region -- Private helpers -

        private async void PushTabbedPage()
        {
            var Loginization = await AuthorizationService.LoginAsync(Login, Password);
            if (Loginization)
            {
                Settings.SavedUserId = App.CurrentUserId;
                //TODO: rename to parameters or something else what is not one lettered
                var p = new NavigationParameters();
                p.Add(Constants.NavigationParameters.LoadFromDataBase, true);
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(TabbedMapPage)}", p);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(AppResources.WrongLoginOrPassword);
            }
        }

        private async void PushRegistrationPage()
        {
            await NavigationService.NavigateAsync($"{nameof(RegistrationPage)}");
        }

        #endregion
    }
}