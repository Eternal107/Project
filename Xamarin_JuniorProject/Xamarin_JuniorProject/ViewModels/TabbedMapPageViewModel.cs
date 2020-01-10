using System;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels
{
    public class TabbedMapPageViewModel:ViewModelBase
    {
        public TabbedMapPageViewModel(INavigationService navigationService): base(navigationService)
        {
            
        }
        #region -- Public properties --
        public ICommand SignOut => new Command(OnSignOut);
        #endregion
        #region -- Private helpers--
        private async void OnSignOut()
        {
            Settings.SavedUserId = -1;
     
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LoginPage)}");
        }
        #endregion
    }
}
