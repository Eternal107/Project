using System;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class TabbedMapPageViewModel:ViewModelBase
    {


        public ICommand SignOut => new Command(OnSignOut);



        public TabbedMapPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            
        }

        private async void OnSignOut()
        {
            Prism.PrismApplicationBase.Current.Properties.Clear();
            await Prism.PrismApplicationBase.Current.SavePropertiesAsync() ;
            App.CurrentUserId = -1;
            await NavigationService.NavigateAsync("/NavigationPage/MainPage");
        }
       
    }
}
