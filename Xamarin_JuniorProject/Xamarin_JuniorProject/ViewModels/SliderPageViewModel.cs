﻿using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class SliderPageViewModel : ViewModelBase
    {

        private Pin _currentPin;
        public Pin CurrentPin
        {
            get { return _currentPin; }
            set { SetProperty(ref _currentPin, value); }
        }

       

        private DelegateCommand _addPinPage;
        public DelegateCommand AddPinPage =>
            _addPinPage ?? (_addPinPage = new DelegateCommand(ToAddPinPage));

        public SliderPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            
        }



        private async  void  ToAddPinPage()
        {
            var p = new NavigationParameters();
            p.Add("PinSettings",CurrentPin);

            CurrentPin = null;
            await NavigationService.NavigateAsync("AddPinPage",p);

        }
    }
}
