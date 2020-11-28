using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace LodFinals.Views.Common
{
    public partial class RecordButton : ContentView
    {
        public RecordButton()
        {
            InitializeComponent();
        }

        public static BindableProperty ButtonTextProperty = BindableProperty.Create(
            nameof(ButtonText),
            typeof(string),
            typeof(RecordButton),
            "Сказать");

        public static BindableProperty ButtonCommandProperty = BindableProperty.Create(
            nameof(ButtonCommand),
            typeof(ICommand),
            typeof(RecordButton),
            null);

        public static BindableProperty IsRecordingProperty = BindableProperty.Create(
            nameof(IsRecording),
            typeof(bool),
            typeof(RecordButton),
            false);

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        public bool IsRecording
        {
            get => (bool)GetValue(IsRecordingProperty);
            set => SetValue(IsRecordingProperty, value);
        }
    }
}
