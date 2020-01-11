using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Xamarin_JuniorProject.Views.ModalViews
{
    public partial class PinModalView : BaseModalView
    {
        public PinModalView()
        {
            InitializeComponent();
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, Constants.MessagingCenter.DeletePin);
        }
    }
}
