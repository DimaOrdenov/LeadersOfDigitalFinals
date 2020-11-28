using System;
using System.Collections.Generic;
using System.Windows.Input;
using LodFinals.ViewModels.Common;
using LodFinals.Views.Exercises;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Definitions.Enums;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LodFinals.ViewModels.Exercises
{
    public class ExerciseDetailsViewModel : PageViewModel
    {
        public ICommand StartCommand { get; }

        public ExerciseDetailsViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            StartCommand = BuildPageVmCommand(
                async () =>
                {
                    State = PageStateType.MinorLoading;

                    await NavigationService.NavigateAsync<TaskOnePage>();

                    State = PageStateType.Default;
                });

            StarsCollection = new List<StarItemViewModel>
            {
                new StarItemViewModel(true),
                new StarItemViewModel(true),
                new StarItemViewModel(true),
                new StarItemViewModel(false),
                new StarItemViewModel(false),
            };
        }

        public IEnumerable<StarItemViewModel> StarsCollection { get; }
    }
}
