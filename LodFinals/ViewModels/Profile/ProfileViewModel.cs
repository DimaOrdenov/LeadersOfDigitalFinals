using System.Windows.Input;
using LodFinals.BusinessLayer;
using LodFinals.Views.Profile;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Services;

namespace LodFinals.ViewModels.Profile
{
    public class ProfileViewModel : AccentInfoViewModel
    {
        public ICommand ChooseAccentCommand { get; }

        public ProfileViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            ExtendedUserContext userContext)
            : base(navigationService, dialogService, debuggerService, exceptionHandler, userContext)
        {
            ChooseAccentCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await NavigationService.NavigateModalAsync<AccentSettingPage>();

                    State = PageStateType.Default;
                });

            Name = userContext.Name;
        }

        public string Name { get; }
    }
}
