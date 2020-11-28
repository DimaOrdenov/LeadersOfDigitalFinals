using System;
using CoreGraphics;
using LodFinals.iOS.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ProgressBar), typeof(BaseProgressBarRenderer))]

namespace LodFinals.iOS.CustomRenderer
{
    public class BaseProgressBarRenderer : ProgressBarRenderer
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            try
            {
                Control.Transform = CGAffineTransform.MakeScale(1.0f, Convert.ToSingle(Element.HeightRequest));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
