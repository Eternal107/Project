using System;
using Android.App;
using Android.Runtime;

namespace Xamarin_JuniorProject.Droid
{
    [Application]
    [MetaData("com.google.android.maps.v2.API_KEY",
              Value = Constants.GOOGLE_MAPS_API_KEY)]
    public class MyApp : Application
    {
        public MyApp(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}
