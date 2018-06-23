using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Plugin.InAppBilling;
namespace Inside.Xamarin.Droid
{
    [Activity(Label = "Inside Parking", Icon = "@drawable/inside", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            Rg.Plugins.Popup.Popup.Init(this,bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init(this, bundle);

            //Status Bar color
            Window window = this.Window;
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.SetStatusBarColor(Android.Graphics.Color.Rgb(49, 125, 38));

            LoadApplication(new App());
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}

