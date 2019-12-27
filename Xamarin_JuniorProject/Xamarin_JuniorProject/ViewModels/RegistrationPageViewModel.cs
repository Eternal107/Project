using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        public DelegateCommand TabbedPage;
        public DelegateCommand ToTabbedPage =>
            TabbedPage ?? (TabbedPage = new DelegateCommand(PushTabbedPage, CanRegister));


        private string login;
        private string password;
        private string email;

        private bool  isValid;
  

        public bool IsValid
        {
            get { return isValid; }
            set { SetProperty(ref isValid, value); }
        }


        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public string Login
        {
            get { return login; }
            set { SetProperty(ref login, value); }
        }
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

       

        public RegistrationPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            Title = "Registration Page";
            TabbedPage = new DelegateCommand(PushTabbedPage, CanRegister);

        }

        private async void PushTabbedPage()
        {
            var CurrentUser = new UserRegistrationModel() { Email = Email, Login = Login, Password = Password };
            var CheckRegistration = await AuthorizationService.Register(CurrentUser);
            if (CheckRegistration)
                await NavigationService.GoBackAsync();
            else
            {
                await UserDialogs.Instance.AlertAsync("Login is already taken");
            }
        }

        private bool CanRegister()
        {


            return IsValid;

            
        }


      

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(IsValid) )
                TabbedPage.RaiseCanExecuteChanged();
        }

    }

}

