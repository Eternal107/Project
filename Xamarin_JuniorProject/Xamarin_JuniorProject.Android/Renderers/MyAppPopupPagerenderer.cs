using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Rg.Plugins.Popup.Droid.Renderers;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin_JuniorProject.Droid.Renderers;

[assembly: ExportRenderer(typeof(PopupPage), typeof(MyAppPopupPageRenderer))]
namespace Xamarin_JuniorProject.Droid.Renderers
{
    public class MyAppPopupPageRenderer : PopupPageRenderer
    {

        public MyAppPopupPageRenderer(Context context) : base(context)
        {

        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            try
            {
                return base.DispatchTouchEvent(e);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}