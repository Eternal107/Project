using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.Authorization;


namespace Xamarin_JuniorProject.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        IAuthorizationService AuthorizationService { get; }

        public RegistrationPageViewModel(INavigationService navigationService,
                                         IAuthorizationService authorizationService)
                                         : base(navigationService)
        {
            Title = AppResources.RegistrationPage;
            TabbedPage = new DelegateCommand(PushTabbedPage, CanRegister);
            AuthorizationService = authorizationService;
        }

        #region -- Public properties --

        private DelegateCommand TabbedPage;
        public DelegateCommand ToTabbedPage =>
          TabbedPage??(new DelegateCommand(PushTabbedPage, CanRegister));


        private bool  _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
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

        #endregion

        #region -- Private helpers--
        private async void PushTabbedPage()
        {
            var CurrentUser = new UserRegistrationModel()
            {
                Email = Email,
                Login = Login,
                Password = Password
            };
            var CheckRegistration = await AuthorizationService.RegisterAsync(CurrentUser);
            if (CheckRegistration)
            {
                await NavigationService.GoBackAsync();
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(AppResources.LoginIsAlreadyTaken);
            }
        }

        private bool CanRegister()
        {
            return IsValid;    
        }
        #endregion

        #region --Overrides--

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(IsValid))
                ToTabbedPage.RaiseCanExecuteChanged();
        }

        #endregion
    }
}

