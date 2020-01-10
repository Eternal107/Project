using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System.IO;
using System.Linq;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels.ModalViewModels
{
    public class PinModalViewModel : ViewModelBase
    {
        private IPinService PinService { get; }

        public PinModalViewModel(INavigationService navigationService,
                                 IPinService pinService,
                                 Pin pin)
                                 : base(navigationService)
        {
            CurrentPin = pin;
            PinService = pinService;
        }

        #region -- Public properties --

        public DelegateCommand AddPinPage => new DelegateCommand(ToAddPinPage);

        public DelegateCommand DeletePin => new DelegateCommand(ToDeletePin);

        public DelegateCommand AddImage => new DelegateCommand(AddImageToPin);

        private Pin _currentPin;
        public Pin CurrentPin
        {
            get { return _currentPin; }
            set { SetProperty(ref _currentPin, value); }
        }

        private string _imageSource;
        public string ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }

        #endregion

        #region -- Private helpers--

        private async void ToDeletePin()
        {
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.DeletePin, CurrentPin);
            var pinModel = await PinService.FindPinModelAsync(CurrentPin);
            await PinService.DeletePinAsync(pinModel);

            await NavigationService.GoBackAsync(p, useModalNavigation: true);

        }

        public async void AddImageToPin()
        {
            PickMediaOptions options = new PickMediaOptions();

            options.CustomPhotoSize = 6;
            options.PhotoSize=PhotoSize.Custom;
            try
            {
                var file = await CrossMedia.Current.PickPhotoAsync(options);

                if (file != null)
                {
                    var pinModel = await PinService.FindPinModelAsync(CurrentPin);
                    pinModel.ImagePath = file.Path;
                    await PinService.UpdatePinAsync(pinModel);

                    CurrentPin.Icon = BitmapDescriptorFactory.FromStream(File.OpenRead(file.Path));
                    var p = new NavigationParameters();
                    p.Add(Constants.NavigationParameters.LoadFromDataBase, true);
                    await NavigationService.GoBackAsync(p, useModalNavigation: true);
                }
            }
            catch
            {
                await UserDialogs.Instance.AlertAsync("Wrong image format");
            }
        }

        private async void ToAddPinPage()
        {
            var pinModel = await PinService.FindPinModelAsync(CurrentPin);
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.PinSettings, pinModel);
            await PopupNavigation.Instance.PopAsync();
            await NavigationService.NavigateAsync($"{nameof(AddPinPage)}", p);
        }

        #endregion
    }
}

