using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {


        protected INavigationService NavigationService { get; private set; }
        protected IRepositoryService Repository { get; private set; }
        protected IAuthorizationService AuthorizationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService)
        {
            NavigationService = navigationService;
            Repository = repository;
            AuthorizationService = authorizationService;
        }



        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
    }
}

