using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(LodFinals.iOS.CustomRenderer.BaseEntryRenderer))]

namespace LodFinals.iOS.CustomRenderer
{
    public class BaseEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || Control == null)
            {
                return;
            }

            Control.BorderStyle = UITextBorderStyle.None;

            if (!(Element is Entry view))
            {
                return;
            }

            UIToolbar toolbar = new UIToolbar();
            toolbar.SizeToFit();

            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
            };

            Control.InputAccessoryView = toolbar;
        }
    }
}
