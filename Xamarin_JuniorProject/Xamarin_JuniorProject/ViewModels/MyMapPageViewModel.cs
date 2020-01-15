using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Helpers;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.CategoryService;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Views.ModalViews;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MyMapPageViewModel : ViewModelBase
    {
        private IPinService _pinService { get; }
        private ICategoryService _categoryService { get; }

        public MyMapPageViewModel(INavigationService navigationService,
                                  IPinService pinService,
                                  ICategoryService categoryService)
                                  : base(navigationService)
        {
            Pins = new ObservableCollection<Pin>();
            CategoryList = new ObservableCollection<CategoryViewModel>();
            MapCameraPosition = new CameraPosition(new Position(0, 0), 0);
            Title = AppResources.Map;
            _pinService = pinService;
            _categoryService = categoryService;
            
        }
        #region -- Public properties --
        public ICommand LongClickedCommand =>  ExtendedCommand.Create<Position>(OnLongclickedCommand);

        public ICommand PinClickedCommand =>  ExtendedCommand.Create<Pin>(OnPinClickedCommand);

        public ICommand TextChangedCommand =>  ExtendedCommand.Create(OnTextChangedCommand);

        public ICommand CategoryTappedCommand =>  ExtendedCommand.Create<CategoryViewModel>(OnCategoryTappedCommand);

        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        private ObservableCollection<CategoryViewModel> _categoryList;
        public ObservableCollection<CategoryViewModel> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }

        private CameraPosition _mapCameraPosition;
        public CameraPosition MapCameraPosition
        {
            get { return _mapCameraPosition; }
            set { SetProperty(ref _mapCameraPosition, value); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        private bool _isLocationEnabled;
        public bool IsLocationEnabled
        {
            get { return _isLocationEnabled; }
            set { SetProperty(ref _isLocationEnabled, value); }
        }
        #endregion

        #region -- Private helpers--

        private async void ShowPin(PinViewViewModel pin)
        {
            
            var newPin = (await _pinService.GetPinsAsync(App.CurrentUserId)).LastOrDefault(x => x.ID == pin.ID);
            var Pin = newPin.ToPin();
            MapCameraPosition = new CameraPosition(Pin.Position, 5);
            if (!Pins.Contains(Pin))
            {
                Pins.Add(Pin);
                MessagingCenter.Subscribe<PinModalView>(this, Constants.MessagingCenter.DeletePin, (seconSender) =>
                {
                    Pins.Remove(Pin);
                    MessagingCenter.Unsubscribe<PinModalView>(this, Constants.MessagingCenter.DeletePin);
                });
            }

            await OnPinClickedCommand(Pin);
        }

        private async Task OnCategoryTappedCommand(CategoryViewModel categoryView)
        {
            categoryView.IsSelected = !categoryView.IsSelected;
            await OnCategoryTapped();   
        }


        private async Task OnPinClickedCommand(Pin pin)
        {
            var parameters = new NavigationParameters();
            parameters.Add(Constants.NavigationParameters.SelectedPin, pin);
            await NavigationService.NavigateAsync($"{nameof(PinModalView)}", parameters, useModalNavigation: true);
        }

        private async Task LoadPinsFromDataBase()
        {
            Pins.Clear();
            var PinModels = (await _pinService.GetPinsAsync(App.CurrentUserId)).Where(x => x.IsFavorite == true);
            if (PinModels != null)
            {
                foreach (PinModel model in PinModels)
                {
                    Pins.Add(model.ToPin());
                }
            }
        }

        private async Task LoadCategoriesFromDataBase()
        {
            CategoryList.Clear();
            var CategoryModels = await _categoryService.GetCategoriesAsync(App.CurrentUserId);
            CategoryList.Add(new CategoryViewModel(
               -1,
               App.CurrentUserId,
               "No Category",
               CategoryTappedCommand
               ));

            if (CategoryModels != null)
            {
                foreach (var model in CategoryModels)
                {
                    CategoryList.Add(model.ToViewModel(CategoryTappedCommand));
                }
            }
        }

        private async Task OnLongclickedCommand(Position point)
        {
            var lat = point.Latitude;
            var lng = point.Longitude;
            var pin = Pins.LastOrDefault(x => x.Position == point);
            if (pin == null)
            {
                PromptResult result = await UserDialogs.Instance.PromptAsync(string.Format("{0}, {1}", lat, lng), AppResources.DoYouWantToAddNewPin, AppResources.OK, AppResources.Cancel, AppResources.Name);
                if (result.Ok)
                {
                    Pins.Add(new Pin() {
                        Position = new Position(lat, lng),
                        Type = PinType.SavedPin,
                        Label = result.Text,
                        Tag = string.Empty });
                    await _pinService.SaveOrUpdatePinAsync(Pins.Last().ToPinModel());
                }
            }
        }

        private Task OnCategoryTapped()
        {
            return OnTextChangedCommand();
        }

        private async Task OnTextChangedCommand()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Pins.Clear();
                var Text = SearchText.ToLower();
                var PinModels = (await _pinService.GetPinsAsync(App.CurrentUserId))
                    .Where(x => x.IsFavorite == true && (x.Name.ToLower().Contains(Text) ||
                    x.Description.ToLower().Contains(Text) || x.Latitude.ToString().Contains(Text)
                    || x.Longtitude.ToString().Contains(Text)));

                if (PinModels != null)
                {
                    var Filter = CategoryList.Where(x => x.IsSelected == true);

                    var categoryFilter = new List<int>();

                    foreach (var category in Filter)
                    {
                        categoryFilter.Add(category.ID);
                    }

                    if (categoryFilter.Count != 0)
                    {
                        var pins = PinModels.Where(x => categoryFilter.Contains(x.CategoryID));

                        foreach (PinModel model in pins)
                        {
                            Pins.Add(model.ToPin());
                        }
                    }
                    else
                    {
                        foreach (PinModel model in PinModels)
                        {
                            Pins.Add(model.ToPin());
                        }
                    }
                }
            }
            else 
            {
                Pins.Clear();
                var PinModels = await _pinService.GetPinsAsync(App.CurrentUserId);
                if (PinModels != null)
                {
                    var Filter = CategoryList.Where(x => x.IsSelected == true);

                    var categoryFilter = new List<int>();

                    foreach (var category in Filter)
                    {
                        categoryFilter.Add(category.ID);
                    }

                    if (categoryFilter.Count != 0)
                    {
                        var pinModels = PinModels.Where(x => categoryFilter.Contains(x.CategoryID));

                        foreach (PinModel model in pinModels)
                        {
                            Pins.Add(model.ToPin());
                        }
                    }
                    else
                    {
                        await LoadPinsFromDataBase();
                    }

                }
            }
        }

        private async Task GetPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

                if (status != PermissionStatus.Granted)
                {
                    status = (await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location))[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    IsLocationEnabled = true;
                }
                else
                {
                    IsLocationEnabled = false;
                }
            }
            catch 
            {
                //Something went wrong
            }
        }

        #endregion

        #region --Overrides--

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(Constants.NavigationParameters.AddPin, out PinViewViewModel pinView))
            {
                ShowPin(pinView);
            }

            if (parameters.ContainsKey(Constants.NavigationParameters.LoadFromDataBase))
            {
                await LoadPinsFromDataBase();
                await LoadCategoriesFromDataBase();
            }
            else if (parameters.TryGetValue(Constants.NavigationParameters.DeletePin, out Pin oldPin))
            {
                Pins.Remove(oldPin);
            }

            await GetPermission();
        }

        public override  void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        #endregion
    }
}
