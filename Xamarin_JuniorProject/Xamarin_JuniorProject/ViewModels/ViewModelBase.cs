using Prism.Mvvm;
using Prism.Navigation;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {

        //TODO: readonly
        protected INavigationService NavigationService { get; }


        //TODO: REGIONS
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        //TODO: move each service to the next line
        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
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


