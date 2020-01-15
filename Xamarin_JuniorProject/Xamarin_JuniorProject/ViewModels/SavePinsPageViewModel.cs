using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Helpers;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.CategoryService;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels
{
    public class SavePinsPageViewModel : ViewModelBase
    {
        IPinService _pinService { get; }
        ICategoryService _categoryService { get; }

        public SavePinsPageViewModel(INavigationService navigationService,
                                     IPinService pinService,
                                     ICategoryService categoryService)
                                     : base(navigationService)
        {
            _categoryService = categoryService;
            _pinService = pinService;
            CategoryList = new ObservableCollection<CategoryViewModel>();
            Title = AppResources.SavedPins;
        }

        #region -- Public properties --

        public ICommand TextChangedCommand => ExtendedCommand.Create(OnTextChangedCommand);
        public ICommand AddPinPageCommand => ExtendedCommand.Create(OnAddPinPageCommand);
        public ICommand CategoryTappedCommand => ExtendedCommand.Create<CategoryViewModel>(OnCategoryTappedCommand);

        private ObservableCollection<CategoryViewModel> _categoryList;
        public ObservableCollection<CategoryViewModel> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }

        private ObservableCollection<PinViewViewModel> _pins = new ObservableCollection<PinViewViewModel>();
        public ObservableCollection<PinViewViewModel> Pins
        {
            get { return _pins; }
            set { SetProperty(ref _pins, value); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        #endregion

        #region -- Private helpers--

        private Task OnAddPinPageCommand()
        {
            return NavigationService.NavigateAsync($"{nameof(AddPinPage)}");
        }

        private async void ToMapPagePin(object o)
        {
            var parameters = new NavigationParameters();
            parameters.Add(Constants.NavigationParameters.AddPin, (PinViewViewModel)o);
            await NavigationService.NavigateAsync($"{nameof(TabbedMapPage)}?selectedTab={nameof(MyMapPage)}",parameters);
        }

        private async Task OnCategoryTappedCommand(CategoryViewModel categoryView)
        {
            categoryView.IsSelected = !categoryView.IsSelected;
            await OnTextChangedCommand();
        }

        private async Task LoadPinsFromDataBaseAsync()
        {
            Pins.Clear();
            var MapPins = await _pinService.GetPinsAsync(App.CurrentUserId);

            ICommand TappedCommand = new Command(ToMapPagePin);

            if (MapPins != null)
            {
                foreach (var pin in MapPins)
                {
                    var PinView = pin.ToViewViewModel(TappedCommand);
                    Pins.Add(PinView);
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
                        ICommand TappedCommand = new Command(ToMapPagePin);

                        var pins = PinModels.Where(x => categoryFilter.Contains(x.CategoryID));

                        foreach (PinModel model in pins)
                        {
                            Pins.Add(model.ToViewViewModel(TappedCommand));
                        }
                    }
                    else
                    {
                        ICommand TappedCommand = new Command(ToMapPagePin);

                        foreach (PinModel model in PinModels)
                        {
                            Pins.Add(model.ToViewViewModel(TappedCommand));
                        }
                    }
                }
                else
                {
                    //do nothing
                }
            }
            else
            {
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
                        Pins.Clear();
                        ICommand TappedCommand = new Command(ToMapPagePin);

                        var pinModels = PinModels.Where(x => categoryFilter.Contains(x.CategoryID));

                        foreach (PinModel model in pinModels)
                        {
                            Pins.Add(model.ToViewViewModel(TappedCommand));
                        }
                    }
                    else
                    {
                        await LoadPinsFromDataBaseAsync();
                    }
                }
                else
                {
                    //do nothing
                }
            }
        }

        #endregion

        #region --Overrides--

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadPinsFromDataBaseAsync();
            await LoadCategoriesFromDataBase();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(Constants.NavigationParameters.LoadFromDataBase, true);
        }

        #endregion
    }
}

