using NoTryCatch.Xamarin.Portable.ViewControls;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace LodFinals.Views
{
    public partial class MainTabbedPage : BaseTabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();

            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}
