using AppSmartKidsXa.iOS.RenderEntry;
using AppSmartKidsXa.RenderEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(BorderlessEditor), typeof(CustomEditorRenderer))]
namespace AppSmartKidsXa.iOS.RenderEntry
{
    /// <summary>
    /// Render For entry to remove border bettom
    /// </summary>
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                if (Control != null)
                {
                    Control.Layer.BorderWidth = 0;           
                }
            }
        }
    }
}