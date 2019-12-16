using System;
using Prism.Commands;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        public DelegateCommand TabbedPage;
        public DelegateCommand ToTabbedPage =>
            TabbedPage ?? (TabbedPage = new DelegateCommand(PushTabbedPage));


        private string login;
        private string password;
        private string email;
        private string confirmPassword;

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

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { SetProperty(ref confirmPassword, value); }
        }

        public RegistrationPageViewModel(INavigationService navigationService, IRepositoryService repositoryService, IAuthorizationService authorizationService)
            : base(navigationService, repositoryService, authorizationService)
        {
            Title = "Login Page";
        }

        private async void PushTabbedPage()
        {
            var CurrentUser = new User() { Email = Email, Login = Login, Password = Password };
            var CheckRegistration = await AuthorizationService.Register(CurrentUser);
            if (CheckRegistration)
                await NavigationService.NavigateAsync("/NavigationPage/TabbedMapPage");

        }

    }
}
