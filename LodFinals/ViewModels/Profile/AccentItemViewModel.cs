using System.Windows.Input;
using LodFinals.Definitions.Enums;
using LodFinals.Extensions;
using NoTryCatch.Core.Extensions;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LodFinals.ViewModels.Profile
{
    public class AccentItemViewModel : BaseViewModel
    {
        private bool _isSelected;

        public ICommand TapCommand { get; set; }

        public AccentItemViewModel(LanguageCodeType languageCode)
        {
            LanguageCode = languageCode;
            Title = languageCode.GetEnumDescription();
            FlagImage = languageCode.GetFlagImage();

            var checkMark = AppImages.IcCheck;
            checkMark.SetTintColor(AppColors.Green);

            CheckMarkImage = checkMark;
        }

        public ImageSource FlagImage { get; }

        public string Title { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public ImageSource CheckMarkImage { get; }

        public LanguageCodeType LanguageCode { get; }
    }
}
