using System.Linq;
using System.Threading.Tasks;
using FFImageLoading.Svg.Forms;
using NoTryCatch.Xamarin.Portable.Extensions;
using NoTryCatch.Xamarin.Portable.ViewControls;

namespace LodFinals.Views.Chat
{
    public partial class ChatPage : BasePage
    {
        public ChatPage()
        {
            InitializeComponent();

            SvgImageSource chatIcon = AppImages.IcChat;
            chatIcon.SetTintColor(AppColors.Navy);

            icChat.Source = chatIcon;

            messagesLayout.ChildAdded += async (sender, e) =>
            {
                await Task.Delay(100);

                await messagesScrollLayout.ScrollToAsync(messagesLayout, Xamarin.Forms.ScrollToPosition.End, false);
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            icChat.SetTintColor(AppColors.Navy);
        }
    }
}
