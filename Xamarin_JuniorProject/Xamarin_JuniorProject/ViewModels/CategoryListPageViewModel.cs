using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin_JuniorProject.Extentions;
using Xamarin_JuniorProject.Helpers;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.CategoryService;
using Xamarin_JuniorProject.Services.Pin;

namespace Xamarin_JuniorProject.ViewModels
{
    public class CategoryListPageViewModel:ViewModelBase
    {
        private ICategoryService _categoryService { get; }
        private IPinService _pinService { get; }

        public CategoryListPageViewModel(INavigationService navigationService,
                                         ICategoryService categoryService,
                                         IPinService pinservice)
                                         : base(navigationService)
        {
            _pinService = pinservice;
            _categoryService = categoryService;
            CategoryList = new ObservableCollection<CategoryViewModel>();
        }

        #region -- Public Properties --

        public ICommand AddCommand => ExtendedCommand.Create(OnAddCommand);
        public ICommand ItemTappedCommand => ExtendedCommand.Create<CategoryViewModel>(OnItemTappedCommand);

        private ObservableCollection<CategoryViewModel> _categoryList;
        public ObservableCollection<CategoryViewModel> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }

        #endregion


        #region -- Private helpers --

        private async Task OnItemTappedCommand(CategoryViewModel viewModel)
        {
            var result = await UserDialogs.Instance.ConfirmAsync
                ( AppResources.DoYouWantToDeleteThisCategory, string.Empty,
                AppResources.OK, AppResources.Cancel);

            if (result)
            {
                CategoryList.Remove(viewModel);
                await _categoryService.DeleteCategoryAsync(viewModel.ToModel());
                var pinModels = await _pinService.GetPinsAsync(App.CurrentUserId);
                if(pinModels!=null)
                {
                    foreach(var model in pinModels)
                    {
                        if(model.CategoryID==viewModel.ID)
                        {
                            model.CategoryID = -1;
                            await _pinService.SaveOrUpdatePinAsync(model);
                        }
                    }
                }
            }
        }

        private async Task OnAddCommand()
        {
            PromptResult result = await UserDialogs.Instance.PromptAsync
                (string.Empty, AppResources.DoYouWantToAddNewCategory,
                AppResources.OK, AppResources.Cancel,
                AppResources.Name);

            if (result.Ok)
            {
                if (!string.IsNullOrEmpty(result.Text))
                {
                    
                    var newCategory= new CategoryModel()
                    {
                        UserID = App.CurrentUserId,
                        Category = result.Text
                    };

                    await _categoryService.SaveOrUpdateCategoryAsync(newCategory);
                    CategoryList.Add(newCategory.ToViewModel(ItemTappedCommand));
                }
            }
        }
        #endregion

        #region -- Overrides --

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var categoryList = await _categoryService.GetCategoriesAsync(App.CurrentUserId);

            foreach(var category in categoryList)
            {
                CategoryList.Add(category.ToViewModel(ItemTappedCommand));
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(Constants.NavigationParameters.LoadFromDataBase, true);
        }

        #endregion
    }
}
