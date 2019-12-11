using System;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;

namespace Xamarin_JuniorProject.ViewModels
{
    public class SavePinsPageViewModel: ViewModelBase
    {
    
        

            public SavePinsPageViewModel(INavigationService navigationService, IRepository<User> user)
                : base(navigationService, user)
            {
                Title = "Saved Pins";
            }

    }
}

