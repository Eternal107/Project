using System;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Xamarin_JuniorProject.Views.ModalViews
{
    public class BaseModalView : PopupPage
    {
        public BaseModalView()
        {
            Animation = new MoveAnimation
            {
                PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom
            };
            BackgroundColor = Color.Transparent;
        }
    }
}
