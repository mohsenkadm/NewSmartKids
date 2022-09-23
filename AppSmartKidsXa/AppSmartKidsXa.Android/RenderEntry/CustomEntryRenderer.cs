using AppSmartKidsXa.Droid.RenderEntry;
using AppSmartKidsXa.RenderEntry;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderLessEntry), typeof(CustomEntryRenderer))]
namespace AppSmartKidsXa.Droid.RenderEntry
{
    /// <summary>
    /// Render For entry to remove border bettom
    /// </summary>
    public class CustomEntryRenderer : EntryRenderer
    {
        #region on element chenge 
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
        #endregion
    }
}