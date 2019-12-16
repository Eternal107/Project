using Prism;
using Prism.Ioc;
using Xamarin_JuniorProject.ViewModels;
using Xamarin_JuniorProject.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Database;
using System.IO;
using System;
using SQLite;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Repository;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Xamarin_JuniorProject
{
    public partial class App
    {   
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<RegistrationPage, RegistrationPageViewModel>();
            containerRegistry.RegisterForNavigation<MyMapPage,MyMapPageViewModel>();
            containerRegistry.RegisterForNavigation<SavedPinsPage,SavePinsPageViewModel>();
            containerRegistry.RegisterForNavigation<TabbedMapPage,TabbedMapPageViewModel>();
        }
    }
}
