using System;
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

        private int _tabbedPageIndex;
        public int TabbedPageIndex
        {
            get { return _tabbedPageIndex; }
            set { SetProperty(ref _tabbedPageIndex, value); }
        }


        public TabbedMapPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            MessagingCenter.Subscribe<SavePinsPageViewModel>(this, "ToFirstPage", (sender) =>
            {
                TabbedPageIndex = 0;
            });
        }

       
    }
}
