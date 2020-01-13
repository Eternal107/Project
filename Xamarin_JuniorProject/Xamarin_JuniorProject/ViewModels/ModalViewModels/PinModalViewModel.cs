using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Helpers;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.CategoryService;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels.ModalViewModels
{
    public class PinModalViewModel : ViewModelBase
    {
        private IPinService _pinService { get; }
        private ICategoryService _categoryService { get;}

        public PinModalViewModel(INavigationService navigationService,
                                 IPinService pinService,
                                 ICategoryService categoryService)
                                 : base(navigationService)
        {
            _pinService = pinService;
            _categoryService = categoryService;

        }

        #region -- Public properties --

        public ICommand AddPinPageCommand => ExtendedCommand.Create(OnAddPinPageCommand);

        public ICommand DeletePinCommand => ExtendedCommand.Create(OnDeletePinCommand);
       
        public ICommand AddImageCommand => ExtendedCommand.Create(OnAddImageCommand);

        public ICommand CategoryTappedCommand => ExtendedCommand.Create<CategoryViewModel>(OnCategoryTappedCommand);

        private ObservableCollection<CategoryViewModel> _categoryList;
        public ObservableCollection<CategoryViewModel> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }

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

        private CategoryViewModel _currentPinCategory;
        public CategoryViewModel CurrentPinCategory
        {
            get { return _currentPinCategory; }
            set { SetProperty(ref _currentPinCategory, value); }
        }

        #endregion

        #region -- Private helpers--

        private async Task LoadCategoriesAsync()
        {
            var categoryList = await _categoryService.GetCategoriesAsync(App.CurrentUserId);
            var categories = new ObservableCollection<CategoryViewModel>();

            categories.Add(new CategoryViewModel(
                -1,
                App.CurrentUserId,
                "No Category",
                CategoryTappedCommand
                ));

            if (categoryList != null)
            {
                foreach (var category in categoryList)
                {
                    categories.Add(category.ToViewModel(CategoryTappedCommand));
                }
            }

            CategoryList = categories;
        }

        private async Task LoadPincategoryAsync()
        {
            var pinModel = await _pinService.FindPinModelAsync(CurrentPin);
            
            foreach (var category in CategoryList)
            {
                if (category.ID == pinModel.CategoryID)
                {
                    category.IsSelected = true;
                    CurrentPinCategory = category;
                    break;
                }
            }
        }

        private async Task OnCategoryTappedCommand(CategoryViewModel viewModel)
        {
            if (CurrentPinCategory != viewModel)
            {
                viewModel.IsSelected = true;
                var pinModel = await _pinService.FindPinModelAsync(CurrentPin);

                var categoryIndex = CategoryList.IndexOf(CurrentPinCategory);
                CategoryList[categoryIndex].IsSelected = false;
                CurrentPinCategory = viewModel;
                pinModel.CategoryID = viewModel.ID;
                await _pinService.UpdatePinAsync(pinModel);
            }
        }

        private async Task OnDeletePinCommand()
        {
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.DeletePin, CurrentPin);

            var pinModel = await _pinService.FindPinModelAsync(CurrentPin);
            await _pinService.DeletePinAsync(pinModel);

            await NavigationService.GoBackAsync(p, useModalNavigation: true);
        }

        public async Task OnAddImageCommand()
        {
            PickMediaOptions options = new PickMediaOptions();

            options.CustomPhotoSize = 6;
            options.PhotoSize=PhotoSize.Custom;
            try
            {
                var file = await CrossMedia.Current.PickPhotoAsync(options);

                if (file != null)
                {
                    var pinModel = await _pinService.FindPinModelAsync(CurrentPin);
                    pinModel.ImagePath = file.Path;
                    await _pinService.UpdatePinAsync(pinModel);

                    CurrentPin.Icon = BitmapDescriptorFactory.FromStream(File.OpenRead(file.Path));
                    var p = new NavigationParameters();
                    p.Add(Constants.NavigationParameters.LoadFromDataBase, true);
                    await NavigationService.GoBackAsync(p, useModalNavigation: true);
                }
            }
            catch
            {
                await UserDialogs.Instance.AlertAsync(AppResources.WrongImageFormat);
            }
        }

        private async Task OnAddPinPageCommand()
        {
            var pinModel = await _pinService.FindPinModelAsync(CurrentPin);
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.PinSettings, pinModel);
            await PopupNavigation.Instance.PopAsync();
            await NavigationService.NavigateAsync($"{nameof(AddPinPage)}", p);
        }

        #endregion

        #region --Overrides--

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadCategoriesAsync();
         
            if (parameters.TryGetValue(Constants.NavigationParameters.SelectedPin, out Pin pin))
            {
                CurrentPin = pin;
                await LoadPincategoryAsync();
            }
        }



        #endregion
    }
}

