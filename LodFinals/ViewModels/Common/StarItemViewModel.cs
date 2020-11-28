using FFImageLoading.Svg.Forms;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LodFinals.ViewModels.Common
{
    public class StarItemViewModel : BaseViewModel
    {
        public StarItemViewModel(bool isFilled)
        {
            IsFilled = isFilled;
        }

        public bool IsFilled { get; }

        public ImageSource Image
        {
            get
            {
                SvgImageSource image = AppImages.IcStar;

                image.SetTintColor(IsFilled ? AppColors.Yellow : Color.White);

                return image;
            }
        }
    }
}
