using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DnDStoryWriterHalper.Components.Helpers.FontAwesome;
using DnDStoryWriterHalper.Models;

namespace DnDStoryWriterHalper.Services.Converters
{
    public class PageObjectToAwesomeFontIconConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                TextPage => Symbols.alignJustify,
                ImagePage => Symbols.image,
                BrowserPage => Symbols.globe,
                AddonPage => Symbols.squarePlus,
                CanvasPage => Symbols.pencil,
                _ => Symbols.circleQuestion
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
