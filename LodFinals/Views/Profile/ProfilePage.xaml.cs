using FFImageLoading.Svg.Forms;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LodFinals.Views.Profile
{
    public partial class ProfilePage : BasePage
    {
        public ProfilePage()
        {
            InitializeComponent();

            SvgImageSource settingsIcon = AppImages.IcSetting;
            settingsIcon.SetTintColor(AppColors.Navy);

            icSettings.Source = settingsIcon;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            icSettings.SetTintColor(AppColors.Navy);
            icChooseAccent.SetTintColor(AppColors.Navy);
        }
    }
}
