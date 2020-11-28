using System;
using Android.Content;
using LodFinals.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ProgressBar), typeof(BaseProgressBarRenderer))]

namespace LodFinals.Droid.CustomRenderers
{
    public class BaseProgressBarRenderer : ProgressBarRenderer
    {
        public BaseProgressBarRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);

            try
            {
                Control.ScaleY = Convert.ToSingle(Element.HeightRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
