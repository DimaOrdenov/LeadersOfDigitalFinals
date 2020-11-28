using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LodFinals.Views.Profile
{
    public partial class AccentSettingPage : BasePage
    {
        public AccentSettingPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            icClose.SetTintColor(AppColors.Navy);
        }
    }
}
