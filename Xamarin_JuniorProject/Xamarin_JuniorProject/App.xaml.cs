using Prism;
using Prism.Ioc;
using Xamarin_JuniorProject.ViewModels;
using Xamarin_JuniorProject.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Repository;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Views.ModalViews;
using Xamarin_JuniorProject.ViewModels.ModalViewModels;
using Prism.Plugin.Popups;
using Prism.Navigation;
using System.Threading.Tasks;
using Prism.Unity;
using Xamarin_JuniorProject.Services.CategoryService;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Xamarin_JuniorProject
{
    public partial class App : PrismApplication
    {
        public static int CurrentUserId;

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await SetupNavigation();
        }

        private async Task SetupNavigation()
        { 
            if (Settings.SavedUserId!=-1)
            {
                CurrentUserId = Settings.SavedUserId;
                var p = new NavigationParameters();
                p.Add(Constants.NavigationParameters.LoadFromDataBase, true);
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(TabbedMapPage)}", p);
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(LoginPage)}");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //services
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterInstance<ICategoryService>(Container.Resolve<CategoryService>());

            //navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<RegistrationPage, RegistrationPageViewModel>();
            containerRegistry.RegisterForNavigation<MyMapPage, MyMapPageViewModel>();
            containerRegistry.RegisterForNavigation<SavedPinsPage, SavePinsPageViewModel>();
            containerRegistry.RegisterForNavigation<AddPinPage, AddPinPageViewModel>();
            containerRegistry.RegisterForNavigation<PinModalView, PinModalViewModel>();
            containerRegistry.RegisterForNavigation<CategoryListPage, CategoryListPageViewModel>();
            containerRegistry.RegisterForNavigation<TabbedMapPage>();
        }
    }
}
