using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_JuniorProject.Services.Authorization;
using Xamarin_JuniorProject.Services.Pin;
using Xamarin_JuniorProject.Services.Repository;

namespace Xamarin_JuniorProject.ViewModels
{
    public class SliderPageViewModel : ViewModelBase
    {

        private string _nameText;
        public string NameText
        {
            get { return _nameText; }
            set { SetProperty(ref _nameText, value); }
        }

        private string _descriptionText;
        public string DescriptionText
        {
            get { return _descriptionText; }
            set { SetProperty(ref _descriptionText, value); }
        }

        public SliderPageViewModel(INavigationService navigationService, IRepositoryService repository, IAuthorizationService authorizationService, IPinService pinService)
            : base(navigationService, repository, authorizationService, pinService)
        {

        }
    }
}
