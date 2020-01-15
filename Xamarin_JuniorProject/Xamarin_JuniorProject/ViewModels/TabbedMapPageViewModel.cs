using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin_JuniorProject.Helpers;
using Xamarin_JuniorProject.Views;

namespace Xamarin_JuniorProject.ViewModels
{
    public class TabbedMapPageViewModel : ViewModelBase
    {
        public TabbedMapPageViewModel(INavigationService navigationService): base(navigationService)
        {
            
        }

        #region -- Public properties --

        public ICommand SignOutCommand => ExtendedCommand.Create(OnSignOutCommand);
        public ICommand ToCategoryListPageCommand => ExtendedCommand.Create(OnToCategoryListPageCommand);

        #endregion

        #region -- Private helpers--

        private Task OnSignOutCommand()
        {
            Settings.SavedUserId = -1;
     
            return NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LoginPage)}");
        }

        private Task OnToCategoryListPageCommand()
        {
            return NavigationService.NavigateAsync($"{nameof(CategoryListPage)}");
        }

        #endregion
    }
}
