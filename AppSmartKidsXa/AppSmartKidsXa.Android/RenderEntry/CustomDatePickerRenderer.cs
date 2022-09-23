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

[assembly: ExportRenderer(typeof(BorderlessDatePicker), typeof(CustomDatePickerRenderer))]

namespace AppSmartKidsXa.Droid.RenderEntry
{
    /// <summary>
    /// Render For data picker to remove border bettom
    /// </summary>
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        public static void Init() { }
        #region on elemnt chenge
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
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