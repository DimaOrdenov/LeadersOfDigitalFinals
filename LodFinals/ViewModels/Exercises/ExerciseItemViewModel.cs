using System;
using System.Collections.Generic;
using System.Windows.Input;
using LodFinals.ViewModels.Common;
using NoTryCatch.Xamarin.Portable.ViewModels;
using Xamarin.Forms;

namespace LodFinals.ViewModels.Exercises
{
    public class ExerciseItemViewModel : BaseViewModel
    {
        public ICommand TapCommand { get; set; }

        public ExerciseItemViewModel()
        {
        }

        public Color BackgroundColor { get; set; }

        public ImageSource Image { get; set; }

        public string Title { get; set; }

        public IEnumerable<StarItemViewModel> StarsCollection { get; set; }

        public int CurrentLesson { get; set; }

        public int TotalLessons { get; set; }
    }
}
