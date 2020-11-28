using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LodFinals.BusinessLayer;
using LodFinals.Definitions.Enums;
using LodFinals.Extensions;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Definitions;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LodFinals.ViewModels.Profile
{
    public class AccentSettingViewModel : PageViewModel
    {
        private readonly ICommand _accentItemTapCommand;

        public AccentSettingViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            ExtendedUserContext userContext)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _accentItemTapCommand = BuildPageVmCommand<AccentItemViewModel>(
                async item =>
                {
                    State = PageStateType.MinorLoading;

                    await ExceptionHandler.PerformCatchableTask(
                        new ViewModelPerformableAction(
                            async () =>
                            {
                                await userContext.SetAccent(item.LanguageCode.GetCode());

                                if (AccentsCollection.FirstOrDefault(x => x.IsSelected) is AccentItemViewModel selectedAccent)
                                {
                                    selectedAccent.IsSelected = false;
                                }

                                item.IsSelected = true;
                            }));

                    State = PageStateType.Default;
                });

            AccentsCollection = Enum.GetValues(typeof(LanguageCodeType))
                .Cast<LanguageCodeType>()
                .Select(x =>
                new AccentItemViewModel(x)
                {
                    TapCommand = _accentItemTapCommand,
                    IsSelected = userContext.SettingAccent == x.GetCode(),
                })
                .ToList();
        }

        public IEnumerable<AccentItemViewModel> AccentsCollection { get; }
    }
}
