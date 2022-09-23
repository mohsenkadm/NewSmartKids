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
[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(CustomPickerRenderer))]
namespace AppSmartKidsXa.Droid.RenderEntry
{
    /// <summary>
    /// Render For picker or combobox to remove border bettom
    /// </summary>
    class CustomPickerRenderer : PickerRenderer
    {
        public static void Init() { }
        #region on element change
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
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