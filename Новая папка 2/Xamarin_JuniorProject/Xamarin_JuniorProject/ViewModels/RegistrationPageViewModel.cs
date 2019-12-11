using System;
using Prism.Commands;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;

namespace Xamarin_JuniorProject.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        public DelegateCommand TabbedPage;
        public DelegateCommand ToTabbedPage =>
            TabbedPage ?? (TabbedPage = new DelegateCommand(PushTabbedPage));


        private string login;
        private string password;
        private string name;


        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
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



        public RegistrationPageViewModel(INavigationService navigationService, IRepository<User> user)
            : base(navigationService, user)
        {
            Title = "Login Page";
        }

        private async void PushTabbedPage()
        {

            await NavigationService.NavigateAsync("/NavigationPage/TabbedMapPage");


        }
       
    }
}
