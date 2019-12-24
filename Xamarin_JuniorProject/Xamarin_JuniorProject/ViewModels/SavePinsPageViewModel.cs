using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class SavePinsPageViewModel : ViewModelBase
    {
        private ObservableCollection<CustomPinView> pins=new ObservableCollection<CustomPinView>();

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


        private async void ToAddPinPage ()
        {
            await NavigationService.NavigateAsync("AddPinPage");
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PinList"))
            {
                var MapPins = parameters.GetValue<ObservableCollection<Pin>>("PinList");
                if (MapPins != null)
                {
                   foreach (var pin in MapPins)
                     Pins.Add(new CustomPinView(pin.ToPinModel((string)pin.Tag)) {Tapped=ToSetPin });
                }
            }
        }

        private async void ToSetPin(object o, EventArgs e)
        {
            await NavigationService.NavigateAsync("AddPinPage");
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            Pins.Clear();
        }
    }
}

