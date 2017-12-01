using Android.App;
using Android.Widget;
using Android.OS;

namespace android_photo_syncr
{
    [Activity(Label = "android_photo_syncr", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

