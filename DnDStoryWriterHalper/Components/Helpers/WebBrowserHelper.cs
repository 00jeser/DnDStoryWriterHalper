using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DnDStoryWriterHalper.Components.Helpers
{
    //https://stackoverflow.com/questions/263551/databind-the-source-property-of-the-webbrowser-in-wpf
    public class WebBrowserHelper
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserHelper), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is WebBrowser browser && e.NewValue is string uri)
            {
                browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
            }
        }
    }
}
