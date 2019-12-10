using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public DelegateCommand TabbedPage;
        public DelegateCommand ToTabbedPage =>
            TabbedPage ?? (TabbedPage = new DelegateCommand(PushTabbedPage));

        public DelegateCommand RegistrationPage;
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



        public MainPageViewModel(INavigationService navigationService, IRepository<User> user)
            : base(navigationService,user)
        {
            Title = "Login Page";
        }

        private async void PushTabbedPage()
        {
         
                    await NavigationService.NavigateAsync("/NavigationPage/TabbedPage");
                
            
        }
        private async void PushRegistrationPage()
        {
            await NavigationService.NavigateAsync("/NavigationPage/RegistrationPage");
        }
    }
}
