using Android.Widget;
using NoTryCatch.Xamarin.Portable.Services.PlatformServices;
using Plugin.CurrentActivity;

namespace LodFinals.Droid.DependencyServices
{
    public class PlatformAlertMessageService : IPlatformAlertMessageService
    {
        public void LongAlert(string message) =>
            Toast.MakeText(CrossCurrentActivity.Current.AppContext, message, ToastLength.Long).Show();

        public void ShortAlert(string message) =>
            Toast.MakeText(CrossCurrentActivity.Current.AppContext, message, ToastLength.Short).Show();
    }
}
