using System;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;

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
            
        public TabbedMapPageViewModel(INavigationService navigationService, IRepository<User> user)
            : base(navigationService, user)
        {
            PinsPageViewModel = new SavePinsPageViewModel(navigationService, user);
            MapPageViewModel = new MyMapPageViewModel(navigationService, user);
        }
    }
}
