using Acr.UserDialogs;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_JuniorProject.Helpers;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private IAuthorizationService _authorizationService { get; }

        public LoginPageViewModel(INavigationService navigationService,
                                  IAuthorizationService authorizationService)
                                  : base(navigationService)
        {
            Title = AppResources.LoginPage;
            _authorizationService = authorizationService;
        }

        #region -- Public properties --

        public ICommand ToTabbedPageCommand => ExtendedCommand.Create(OnToTabbedPageCommand);

        public ICommand ToRegistrationPageCommand => ExtendedCommand.Create(OnToRegistrationPageCommand);

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

        #region -- Private helpers --

        private async Task OnToTabbedPageCommand()
        {
            var Loginization = await _authorizationService.LoginAsync(Login, Password);
            if (Loginization)
            {
                Settings.SavedUserId = App.CurrentUserId;
                var parameters = new NavigationParameters();
                parameters.Add(Constants.NavigationParameters.LoadFromDataBase, true);
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(TabbedMapPage)}", parameters);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(AppResources.WrongLoginOrPassword);
            }
        }

        private Task OnToRegistrationPageCommand()
        {
            return NavigationService.NavigateAsync($"{nameof(RegistrationPage)}");
        }

        #endregion
    }
}