using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using LodFinals.ViewModels.Common;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LodFinals.ViewModels.Exercises
{
    public class ExercisesViewModel : PageViewModel
    {
        private readonly ICommand _exerciseItemTapCommand;

        private ObservableCollection<ExerciseItemViewModel> _exercisesCollection;

        public ExercisesViewModel(
                INavigationService navigationService,
                IDialogService dialogService,
                IDebuggerService debuggerService,
                IExceptionHandler exceptionHandler)
                : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            ExercisesCollection = new ObservableCollection<ExerciseItemViewModel>
            {
                new ExerciseItemViewModel
                {
                    BackgroundColor = AppColors.Navy,
                    Title = "Гостиница",
                    CurrentLesson = 3,
                    TotalLessons = 10,
                    Image = AppImages.ImageBooking,
                    StarsCollection = new List<StarItemViewModel>
                    {
                        new StarItemViewModel(true),
                        new StarItemViewModel(true),
                        new StarItemViewModel(true),
                        new StarItemViewModel(false),
                        new StarItemViewModel(false),
                    },
                    TapCommand = _exerciseItemTapCommand,
                },
                new ExerciseItemViewModel
                {
                    BackgroundColor = AppColors.Orange,
                    Title = "Карьера",
                    CurrentLesson = 1,
                    TotalLessons = 5,
                    Image = AppImages.ImageCareer,
                    StarsCollection = new List<StarItemViewModel>
                    {
                        new StarItemViewModel(true),
                        new StarItemViewModel(false),
                        new StarItemViewModel(false),
                        new StarItemViewModel(false),
                        new StarItemViewModel(false),
                    },
                    TapCommand = _exerciseItemTapCommand,
                },
                new ExerciseItemViewModel
                {
                    BackgroundColor = AppColors.Turqouise,
                    Title = "Путешествия",
                    CurrentLesson = 4,
                    TotalLessons = 15,
                    Image = AppImages.ImageTravel,
                    StarsCollection = new List<StarItemViewModel>
                    {
                        new StarItemViewModel(true),
                        new StarItemViewModel(true),
                        new StarItemViewModel(true),
                        new StarItemViewModel(true),
                        new StarItemViewModel(false),
                    },
                    TapCommand = _exerciseItemTapCommand,
                },
            };
        }

        public ObservableCollection<ExerciseItemViewModel> ExercisesCollection
        {
            get => _exercisesCollection;
            set => SetProperty(ref _exercisesCollection, value);
        }
    }
}
