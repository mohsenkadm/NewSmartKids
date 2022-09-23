using AppSmartKidsXa.iOS.RenderEntry;
using AppSmartKidsXa.RenderEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderLessEntry), typeof(CustomEntryRenderer))]
namespace AppSmartKidsXa.iOS.RenderEntry
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                if (Control != null)
                {
                    Control.Layer.BorderWidth = 0;
                    Control.BorderStyle = UIKit.UITextBorderStyle.None;
                }
            }
        }
    }
}