using Prism.Commands;
using Prism.Navigation;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {


        private DelegateCommand TabbedPage;
        public DelegateCommand ToTabbedPage =>
            TabbedPage ?? (TabbedPage = new DelegateCommand(PushTabbedPage));

        private DelegateCommand RegistrationPage;
        public DelegateCommand ToRegistrationPage =>
            RegistrationPage ?? (RegistrationPage = new DelegateCommand(PushRegistrationPage));

        private string login;
        private string password;

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



        public MainPageViewModel(INavigationService navigationService, IRepositoryService repositoryService, IAuthorizationService authorizationService)
            : base(navigationService, repositoryService, authorizationService)
        {
            Title = "Login Page";

        }

        private async void PushTabbedPage()
        {
           // var Loginization = await AuthorizationService.Login(Login, Password);
            //if (Loginization)
                await NavigationService.NavigateAsync("/NavigationPage/TabbedMapPage");


        }
        private async void PushRegistrationPage()
        {
            await NavigationService.NavigateAsync("NavigationPage/RegistrationPage");
        }
    }
}