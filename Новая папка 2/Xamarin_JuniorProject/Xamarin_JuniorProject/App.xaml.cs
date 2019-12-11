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

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Xamarin_JuniorProject
{
    public partial class App
    {   private string DataBasePath;
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        private void InnitDataBase()
        {
            DataBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataBase0" + ".db3");

            var connection = new SQLiteAsyncConnection(DataBasePath);
            connection.CreateTableAsync<User>();

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<RegistrationPage,RegistrationPageViewModel>();
            containerRegistry.RegisterForNavigation<MyMapPage,MyMapPageViewModel>();
            containerRegistry.RegisterForNavigation<SavedPinsPage, SavePinsPageViewModel>();
            containerRegistry.RegisterForNavigation<TabbedMapPage>();
            containerRegistry.RegisterInstance<IRepository<User>>(Container.Resolve<Repository<User>>());
        }
    }
}
