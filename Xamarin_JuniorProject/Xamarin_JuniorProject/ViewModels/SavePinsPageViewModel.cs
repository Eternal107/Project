using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels
{
    public class SavePinsPageViewModel : ViewModelBase
    {
        private ObservableCollection<CustomPinView> pins = new ObservableCollection<CustomPinView>();

        public ObservableCollection<CustomPinView> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }



        public SavePinsPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            Title = "Saved Pins";
            pins = new ObservableCollection<CustomPinView>();
        }

        private DelegateCommand _addPinPage;
        public DelegateCommand AddPinPage =>
            _addPinPage ?? (_addPinPage = new DelegateCommand(ToAddPinPage));


        private async void ToAddPinPage()
        {
            await NavigationService.NavigateAsync("AddPinPage");
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var MapPins = await PinService.GetPins(App.CurrentUserId);
            if (MapPins != null)
                foreach (var pin in MapPins)
                {
                    var PinView = pin.PinModelToPinView();
                    PinView.Tapped = ToSetPin;
                    Pins.Add(PinView);
                }
        }

        private async void ToSetPin(object o, EventArgs e)
        {
            var p = new NavigationParameters();
            p.Add("UpdatePin", (CustomPinView)o);
            MessagingCenter.Send(this, "ToFirstPage");
            MessagingCenter.Send(this, "AddPin",(CustomPinView)o);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add("LoadFromDataBase", true);
            Pins.Clear();
        }
    }
}

