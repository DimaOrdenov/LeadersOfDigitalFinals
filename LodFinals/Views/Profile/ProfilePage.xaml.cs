using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LodFinals.Views.Profile
{
    public partial class ProfilePage : BasePage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            icSettings.SetTintColor(AppColors.Navy);
            icChooseAccent.SetTintColor(AppColors.Navy);
        }
    }
}
