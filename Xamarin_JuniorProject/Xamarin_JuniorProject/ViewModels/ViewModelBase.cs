using Prism.Mvvm;
using Prism.Navigation;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {


        protected INavigationService NavigationService { get; private set; }
        protected IRepositoryService Repository { get; private set; }
        protected IAuthorizationService AuthorizationService { get; private set; }
        protected IPinService PinService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService,IPinService pinService)
        {
            NavigationService = navigationService;
            Repository = repository;
            AuthorizationService = authorizationService;
            PinService = pinService;
        }



        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
             
        }
    }
}


