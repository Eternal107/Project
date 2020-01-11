using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin_JuniorProject.Controls;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.CategoryService;
using Xamarin_JuniorProject.Services.Pin;


namespace Xamarin_JuniorProject.ViewModels.ModalViewModels
{
    public class PinModalViewModel : ViewModelBase
    {
        private IPinService _pinService { get; }
        private ICategoryService _categoryService { get;}

        public PinModalViewModel(INavigationService navigationService,
                                 IPinService pinService,
                                 ICategoryService categoryService,
                                 Pin pin)
                                 : base(navigationService)
        {
            CurrentPin = pin;
            _pinService = pinService;
            _categoryService = categoryService;
            LoadCategories();
        }

        #region -- Public properties --

        //TODO: Rename
        public ICommand AddPinPage => new Command(ToAddPinPage);

        public ICommand DeletePin => new Command(ToDeletePin);
       
        public ICommand AddImage => new Command(AddImageToPin);

        public ICommand CategoryTappedCommand => new Command<CategoryViewModel>(OnCategoryTappedCommand);

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

        private string _currentPinCategoryText=string.Empty;
        public string CurrentPinCategoryText
        {
            get { return _currentPinCategoryText; }
            set { SetProperty(ref _currentPinCategoryText, value); }
        }

        #endregion

        #region -- Private helpers--

        private async void LoadCategories()
        {
            var categoryList = await _categoryService.GetCategoriesAsync(App.CurrentUserId);
            var pinModel = await _pinService.FindPinModelAsync(CurrentPin);
            var categories = new ObservableCollection<CategoryViewModel>();
            if (pinModel != null)
            {
                if (pinModel.Categories != null)
                {
                    foreach(var str in pinModel.Categories)
                    CurrentPinCategoryText += pinModel.Categories+" ";
                }
            }
            if (categoryList != null)
            {
                foreach (var category in categoryList)
                {
                    categories.Add(category.ToViewModel(CategoryTappedCommand));
                }
            }

            CategoryList = categories;
        }

        private async void OnCategoryTappedCommand(CategoryViewModel viewModel)
        {
            viewModel.IsSelected = !viewModel.IsSelected;
            var pinModel = await _pinService.FindPinModelAsync(CurrentPin);

            foreach (var str in CurrentPinCategoryText.Split())
            {
                pinModel.Categories.Add(new CategoryModel() { Category = str, UserID = App.CurrentUserId });
            }

            await _pinService.UpdatePinAsync(pinModel);
        }

        private async void ToDeletePin()
        {
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.DeletePin, CurrentPin);
            var pinModel = await _pinService.FindPinModelAsync(CurrentPin);
            await _pinService.DeletePinAsync(pinModel);

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

        private async void ToAddPinPage()
        {
            var pinModel = await _pinService.FindPinModelAsync(CurrentPin);
            var p = new NavigationParameters();
            p.Add(Constants.NavigationParameters.PinSettings, pinModel);
            await PopupNavigation.Instance.PopAsync();
            await NavigationService.NavigateAsync($"{nameof(AddPinPage)}", p);
        }

        #endregion

        #region --Overrides--

       

        #endregion
    }
}

