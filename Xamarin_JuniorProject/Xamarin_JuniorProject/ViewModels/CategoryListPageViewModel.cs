using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Acr.UserDialogs;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin_JuniorProject.Models;
using Xamarin_JuniorProject.Resources;
using Xamarin_JuniorProject.Services.CategoryService;

namespace Xamarin_JuniorProject.ViewModels
{
    public class CategoryListPageViewModel:ViewModelBase
    {
        private ICategoryService CategoryService { get; }
        public CategoryListPageViewModel(INavigationService navigationService,
                                         ICategoryService categoryService)
                                         : base(navigationService)
        {
            CategoryService = categoryService;
            CategoryList = new ObservableCollection<string>();
        }

        public ICommand Add => new Command(OnAddClicked);

        private ObservableCollection<string> _categoryList;
        public ObservableCollection<string> CategoryList
        {
            get { return _categoryList; }
            set { SetProperty(ref _categoryList, value); }
        }

        private async void OnAddClicked()
        {
            PromptResult result = await UserDialogs.Instance.PromptAsync
                ("",AppResources.DoYouWantToAddNewCategory,
                AppResources.OK, AppResources.Cancel,
                AppResources.Name);

            if (result.Ok)
            {
                if (!string.IsNullOrEmpty(result.Text))
                {
                    CategoryList.Add(result.Text);
                    await CategoryService.AddCategoryAsync(new CategoryModel()
                    {
                        UserID = App.CurrentUserId,
                        Category = result.Text
                    });
                }
            }
        }
        

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var categoryList = await CategoryService.GetCategoriesAsync(App.CurrentUserId);

            foreach(var category in categoryList)
            {
                CategoryList.Add(category.Category);
            }
        }
    }
}
