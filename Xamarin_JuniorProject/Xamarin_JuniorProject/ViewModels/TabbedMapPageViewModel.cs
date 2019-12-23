using System;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class TabbedMapPageViewModel:ViewModelBase
    {

        private MyMapPageViewModel mapPageViewModel;

        public MyMapPageViewModel MapPageViewModel
        {
            get { return mapPageViewModel; }
            set { SetProperty(ref mapPageViewModel, value); }
        }

        private SavePinsPageViewModel pinsPageViewModel;

        public SavePinsPageViewModel PinsPageViewModel
        {
            get { return pinsPageViewModel; }
            set { SetProperty(ref pinsPageViewModel, value); }
        }
            
        public TabbedMapPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            PinsPageViewModel = new SavePinsPageViewModel(navigationService, repository, authorizationService, pinService);
            MapPageViewModel = new MyMapPageViewModel(navigationService, repository, authorizationService, pinService);
        }

       
    }
}
