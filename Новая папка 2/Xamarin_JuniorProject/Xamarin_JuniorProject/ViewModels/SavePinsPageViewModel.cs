using System;
using Prism.Navigation;
using Xamarin_JuniorProject.Database;
using Xamarin_JuniorProject.Services;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class SavePinsPageViewModel: ViewModelBase
    {
    
        

            public SavePinsPageViewModel(INavigationService navigationService, IRepositoryService repositoryService, IAuthorizationService authorizationService)
            : base(navigationService, repositoryService, authorizationService)
        {
                Title = "Saved Pins";
            }

    }
}

