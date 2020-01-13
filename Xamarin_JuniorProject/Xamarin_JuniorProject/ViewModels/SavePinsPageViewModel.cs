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

        private void ToSetPin(object o)
        {
            MessagingCenter.Send(this, Constants.MessagingCenter.ToFirstPage);
            MessagingCenter.Send(this, Constants.MessagingCenter.AddPin, (PinViewViewModel)o);
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
            if (MapPins != null)
            {
                foreach (var pin in MapPins)
                {
                    var PinView = pin.ToViewViewModel(new Command(ToSetPin));
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
                    var categoryFilter = new List<int>();
                    foreach (var category in CategoryList)
                    {
                        if (category.IsSelected)
                        {
                            categoryFilter.Add(category.ID);
                        }
                    }
                    if (categoryFilter.Count != 0)
                    {
                        foreach (PinModel model in PinModels)
                        {
                            if (categoryFilter.Contains(model.CategoryID))
                            {
                                Pins.Add(model.ToViewViewModel(new Command(ToSetPin)));
                            }
                        }
                    }
                    else
                    {
                        foreach (PinModel model in PinModels)
                        {
                            Pins.Add(model.ToViewViewModel(new Command(ToSetPin)));
                        }
                    }
                }
            }
            else
            {
                var PinModels = await _pinService.GetPinsAsync(App.CurrentUserId);
                if (PinModels != null)
                {
                    var categoryFilter = new List<int>();
                    foreach (var category in CategoryList)
                    {
                        if (category.IsSelected)
                        {
                            categoryFilter.Add(category.ID);
                        }
                    }
                    if (categoryFilter.Count != 0)
                    {
                        Pins.Clear();
                        foreach (PinModel model in PinModels)
                        {
                            if (categoryFilter.Contains(model.CategoryID))
                            {
                                Pins.Add(model.ToViewViewModel(new Command(ToSetPin)));
                            }
                        }
                    }
                    else
                    {
                        await LoadPinsFromDataBaseAsync();
                    }

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

