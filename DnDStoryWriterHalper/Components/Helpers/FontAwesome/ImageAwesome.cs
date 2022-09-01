using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Wpf.Ui.Common;

namespace DnDStoryWriterHalper.Components.Helpers.FontAwesome
{
    public class ImageAwesome
    {
        public static readonly DependencyProperty FontAwesomeProperty =
            DependencyProperty.RegisterAttached(
                "FontAwesome",
                typeof(Symbols),
                typeof(ImageAwesome),
                new UIPropertyMetadata(Symbols.Symbol0, FontAwesomePropertyChanged));
        public static string GetFontAwesome(DependencyObject obj)
        {
            return (string)obj.GetValue(FontAwesomeProperty);
        }

        public static void SetFontAwesome(DependencyObject obj, Symbols value)
        {
            obj.SetValue(FontAwesomeProperty, value);
        }

        public static void FontAwesomePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            int unicodeValue = (int)(Symbols)args.NewValue;
            var tb = (Label)o;
            FontFamily[] fonts = new[]
            {
                new FontFamily(new Uri("pack://application:,,,/"), "./files/Font Awesome 6 Free-Regular-400.otf#Font Awesome 6 Free Regular"),
                new FontFamily(new Uri("pack://application:,,,/"), "./files/Font Awesome 6 Free-Solid-900.otf#Font Awesome 6 Free Solid"),
                new FontFamily(new Uri("pack://application:,,,/"), "./files/Font Awesome 6 Brands-Regular-400.otf#Font Awesome 6 Brands Regular")

            };
            foreach (var family in fonts)
            {
                var typefaces = family.GetTypefaces();
                foreach (Typeface typeface in typefaces)
                {
                    typeface.TryGetGlyphTypeface(out var glyph);
                    if (glyph != null && glyph.CharacterToGlyphMap.TryGetValue(unicodeValue, out var glyphIndex))
                    {
                        tb.FontFamily = family;
                        break;
                    }
                }
                tb.Content = Convert.ToChar(unicodeValue);

            }

            tb.Padding = new Thickness(0);
            //tb.Content = Encoding.Unicode.GetString(BitConverter.GetBytes((int)(Symbols)args.NewValue));
            //tb.Content = ((int)(Symbols)args.NewValue).ToString("X");
        }
    }
}
