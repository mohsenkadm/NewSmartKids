using AppSmartKidsXa.iOS.RenderEntry;
using AppSmartKidsXa.RenderEntry;
using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(BorderlessDatePicker), typeof(CustomDatePickerRenderer))]

namespace AppSmartKidsXa.iOS.RenderEntry
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        public static void Init() { }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control != null)
            { 
            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}