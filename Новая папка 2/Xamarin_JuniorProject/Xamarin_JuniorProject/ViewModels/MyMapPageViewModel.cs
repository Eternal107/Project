using System;
using Prism.Commands;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;

namespace Xamarin_JuniorProject.ViewModels
{
    public class MyMapPageViewModel:ViewModelBase
    {
        
        public MyMapPageViewModel(INavigationService navigationService, IRepository<User> user)
            : base(navigationService, user)
        {
            Title = "Map";
        }

    }
}
