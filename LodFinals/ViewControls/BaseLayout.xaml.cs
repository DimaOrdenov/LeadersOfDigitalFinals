using Xamarin.Forms;

namespace LodFinals.ViewControls
{
    public partial class BaseLayout : AbsoluteLayout
    {
        public BaseLayout()
        {
            InitializeComponent();
        }

        public View Content
        {
            get => content.Content;
            set => content.Content = value;
        }
    }
}
