using FFImageLoading.Svg.Forms;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LodFinals.Views.Exercises
{
    public partial class TaskCompletionPage : BasePage
    {
        public TaskCompletionPage()
        {
            InitializeComponent();

            SvgImageSource closeIcon = AppImages.IcClose;
            closeIcon.SetTintColor(AppColors.Navy);

            icClose.Source = closeIcon;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            icClose.SetTintColor(AppColors.Navy);
        }
    }
}
