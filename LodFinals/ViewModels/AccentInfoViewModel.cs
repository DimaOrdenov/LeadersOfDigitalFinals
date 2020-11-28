using System;
using LodFinals.BusinessLayer;
using LodFinals.Definitions.Enums;
using LodFinals.Extensions;
using NoTryCatch.Core.Extensions;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LodFinals.ViewModels
{
    public abstract class AccentInfoViewModel : PageViewModel
    {
        private readonly ExtendedUserContext _userContext;

        private string _accent;
        private ImageSource _accentFlag;

        protected AccentInfoViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            ExtendedUserContext userContext)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _userContext = userContext;

            userContext.UserContextChanged += (sender, e) => SetAccentValues();

            SetAccentValues();
        }

        public string Accent
        {
            get => _accent;
            set => SetProperty(ref _accent, value);
        }

        public ImageSource AccentFlag
        {
            get => _accentFlag;
            set => SetProperty(ref _accentFlag, value);
        }

        private void SetAccentValues()
        {
            LanguageCodeType codeType = _userContext.SettingAccent.GetCodeType();

            Accent = codeType.GetEnumDescription();
            AccentFlag = codeType.GetFlagImage();
        }
    }
}
