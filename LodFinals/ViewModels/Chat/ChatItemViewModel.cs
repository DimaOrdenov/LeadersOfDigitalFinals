using NoTryCatch.Xamarin.Portable.ViewModels;

namespace LodFinals.ViewModels.Chat
{
    public class ChatItemViewModel : BaseViewModel
    {
        public ChatItemViewModel(string message, bool isSent)
        {
            Message = message;
            IsSent = isSent;
        }

        public string Message { get; }

        public bool IsSent { get; }
    }
}
