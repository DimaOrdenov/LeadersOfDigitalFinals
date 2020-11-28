using System;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LodFinals.ViewModels.Rating
{
    public class RatingViewModel : PageViewModel
    {
        public RatingViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
        }
    }
}
