﻿using Prism;
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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Xamarin_JuniorProject
{
    public partial class App
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
            if (Current.Properties.ContainsKey("LoggedIn"))
            {
                CurrentUserId = (int)Current.Properties["LoggedIn"];
                var p = new NavigationParameters();
                p.Add("LoadFromDataBase", true);
                await NavigationService.NavigateAsync("NavigationPage/TabbedMapPage", p);
            }
            else

                await NavigationService.NavigateAsync("NavigationPage/MainPage");

        }



        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<RegistrationPage, RegistrationPageViewModel>();
            containerRegistry.RegisterForNavigation<MyMapPage, MyMapPageViewModel>();
            containerRegistry.RegisterForNavigation<SavedPinsPage, SavePinsPageViewModel>();
            containerRegistry.RegisterForNavigation<AddPinPage, AddPinPageViewModel>();
            containerRegistry.RegisterForNavigation<PinModalView, PinModalViewModel>();
            containerRegistry.RegisterForNavigation<TabbedMapPage>();
        }
    }
}
