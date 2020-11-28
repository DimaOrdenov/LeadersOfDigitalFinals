using System;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(LodFinals.Droid.CustomRenderers.BaseEntryRenderer))]

namespace LodFinals.Droid.CustomRenderers
{
    public class BaseEntryRenderer : EntryRenderer
    {
        public BaseEntryRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || Control == null)
            {
                return;
            }

            Control.Background = null;
        }
    }
}
