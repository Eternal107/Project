using System;
using System.Collections.Generic;
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
        private List<CustomPinView> pins=new List<CustomPinView>();

        public List<CustomPinView> Pins
        {
            get { return pins; }
            set { SetProperty(ref pins, value); }
        }



        public SavePinsPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {
            Title = "Saved Pins";

        }

        private DelegateCommand _addPinPage;
        public DelegateCommand AddPinPage =>
            _addPinPage ?? (_addPinPage = new DelegateCommand(ToAddPinPage));

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        private async void ToAddPinPage ()
        {
            await NavigationService.NavigateAsync("NavigationPage/AddPinPage");
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PinList"))
            {
                var MapPins = parameters.GetValue<List<Pin>>("PinList");
                if (MapPins != null)
                {
                    var Temp = new List<CustomPinView>();
                    {
                        foreach (var pin in MapPins)

                            Temp.Add(new CustomPinView(pin.ToPinModel((string)pin.Tag)) {Tapped=ToSetPin });
                    }
                    Pins = Temp;
                }
            }
        }

        private async void ToSetPin(object o, EventArgs e)
        {
            await NavigationService.NavigateAsync("NavigationPage/AddPinPage");
        }

    }
}

