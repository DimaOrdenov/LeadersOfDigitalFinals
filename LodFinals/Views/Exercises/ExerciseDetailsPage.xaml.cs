using FFImageLoading.Svg.Forms;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LodFinals.Views.Exercises
{
    public partial class ExerciseDetailsPage : BasePage
    {
        public ExerciseDetailsPage()
        {
            InitializeComponent();

            SvgImageSource arrowBackImage = AppImages.IcArrowLeft;
            arrowBackImage.SetTintColor(AppColors.Navy);

            icArrowBack.Source = arrowBackImage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            icArrowBack.SetTintColor(AppColors.Navy);
        }
    }
}
