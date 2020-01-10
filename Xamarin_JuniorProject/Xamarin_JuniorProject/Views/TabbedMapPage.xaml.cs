
using Xamarin.Forms;
using Xamarin_JuniorProject.ViewModels;

namespace Xamarin_JuniorProject.Views
{
    public partial class TabbedMapPage : TabbedPage
    {
        public TabbedMapPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<SavePinsPageViewModel>(this, Constants.MessagingCenter.ToFirstPage, (sender) =>
            {
                CurrentPage = this.Children[0];
            });
        }
    }
}
