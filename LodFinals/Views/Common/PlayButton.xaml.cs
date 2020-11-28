using System.Windows.Input;
using Xamarin.Forms;

namespace LodFinals.Views.Common
{
    public partial class PlayButton : ContentView
    {
        public PlayButton()
        {
            InitializeComponent();
        }

        public static BindableProperty ButtonTextProperty = BindableProperty.Create(
            nameof(ButtonText),
            typeof(string),
            typeof(PlayButton),
            "Нажмите, чтобы прослушать");

        public static BindableProperty ButtonCommandProperty = BindableProperty.Create(
            nameof(ButtonCommand),
            typeof(ICommand),
            typeof(PlayButton),
            null);

        public static BindableProperty IsPlayingProperty = BindableProperty.Create(
            nameof(IsPlaying),
            typeof(bool),
            typeof(PlayButton),
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

        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }
    }
}
