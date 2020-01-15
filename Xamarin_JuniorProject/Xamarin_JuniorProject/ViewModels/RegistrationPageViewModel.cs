using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.Authorization;

namespace Xamarin_JuniorProject.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        IAuthorizationService _authorizationService { get; }

        public RegistrationPageViewModel(INavigationService navigationService,
                                         IAuthorizationService authorizationService)
                                         : base(navigationService)
        {
            Title = AppResources.RegistrationPage;
            _authorizationService = authorizationService;
        }

        #region -- Public properties --

        private DelegateCommand _toTabbedPageCommand;
        public DelegateCommand ToTabbedPageCommand =>
          _toTabbedPageCommand ?? (_toTabbedPageCommand=new DelegateCommand(OnToTabbedPageCommand, CanRegister));

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
        private async void OnToTabbedPageCommand()
        {
            var CurrentUser = new UserRegistrationModel()
            {
                Email = Email,
                Login = Login,
                Password = Password
            };

            var CheckRegistration = await _authorizationService.RegisterAsync(CurrentUser);

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
            {
                _toTabbedPageCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}

