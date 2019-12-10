using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;

namespace Xamarin_JuniorProject.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {


        protected INavigationService NavigationService { get; private set; }
        protected IRepository<User> user { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService, IRepository<User> user)
        {
            NavigationService = navigationService;
            this.user = user;
        }



        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
    }
}

