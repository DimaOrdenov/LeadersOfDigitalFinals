using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using LodFinals.Containers;
using LodFinals.Droid.DependencyServices;
using Plugin.CurrentActivity;
using NoTryCatch.Core.Services;

namespace LodFinals.Droid
{
    [Activity(
        Label = "LodFinals",
        Icon = "@mipmap/icon",
        RoundIcon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            IocInitializer.Init(
                new PlatformAlertMessageService(),
                new PlatformSpeechToTextService(new DebuggerService()));

            // Init nugets
            XamEffects.Droid.Effects.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            PanCardView.Droid.CardsViewRenderer.Preserve();

            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();

            Xamarin.Essentials.Platform.OnResume();
        }
    }
}