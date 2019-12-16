using System;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Services.Authorization;
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
            
        public TabbedMapPageViewModel(INavigationService navigationService, IRepositoryService repositoryService, IAuthorizationService authorizationService)
            : base(navigationService, repositoryService, authorizationService)
        {
            PinsPageViewModel = new SavePinsPageViewModel(navigationService, repositoryService, authorizationService);
            MapPageViewModel = new MyMapPageViewModel(navigationService, repositoryService, authorizationService);
        }
    }
}
