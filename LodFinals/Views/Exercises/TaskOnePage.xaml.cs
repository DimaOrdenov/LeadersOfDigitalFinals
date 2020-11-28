using FFImageLoading.Svg.Forms;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LodFinals.Views.Exercises
{
    public partial class TaskOnePage : BasePage
    {
        public TaskOnePage()
        {
            InitializeComponent();

            SvgImageSource closeIcon = AppImages.IcClose;
            closeIcon.SetTintColor(AppColors.Navy);

            SvgImageSource checkIcon = AppImages.IcCheck;
            checkIcon.SetTintColor(AppColors.Green);

            icClose.Source = closeIcon;
            icCheck.Source = checkIcon;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            icClose.SetTintColor(AppColors.Navy);
            icCheck.SetTintColor(AppColors.Green);
        }
    }
}
