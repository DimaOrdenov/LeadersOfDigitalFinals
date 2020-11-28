using LodFinals.ViewModels.Chat;
using Xamarin.Forms;

namespace LodFinals.Views.Chat
{
    public class ChatItemTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _sentMessage;
        private readonly DataTemplate _receivedMessage;

        public ChatItemTemplateSelector()
        {
            _sentMessage = new DataTemplate(typeof(SentMessageView));
            _receivedMessage = new DataTemplate(typeof(ReceivedMessageView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is ChatItemViewModel viewModel))
            {
                return null;
            }

            return viewModel.IsSent ? _sentMessage : _receivedMessage;
        }
    }
}
