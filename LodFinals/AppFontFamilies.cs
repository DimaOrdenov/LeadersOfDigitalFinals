using Xamarin.Essentials;

namespace LodFinals
{
    public static class AppFontFamilies
    {
        public static string Bold => DeviceInfo.Platform == DevicePlatform.iOS ? "Nunito-Bold" : "Nunito-Bold.ttf#Nunito-Bold";

        public static string SemiBold => DeviceInfo.Platform == DevicePlatform.iOS ? "Nunito-SemiBold" : "Nunito-SemiBold.ttf#Nunito-SemiBold";

        public static string Regular => DeviceInfo.Platform == DevicePlatform.iOS ? "Nunito-Regular" : "Nunito-Regular.ttf#Nunito-Regular";
    }
}
