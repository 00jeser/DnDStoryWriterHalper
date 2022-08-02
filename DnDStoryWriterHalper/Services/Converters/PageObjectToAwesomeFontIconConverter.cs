using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DnDStoryWriterHalper.Models;
using FontAwesome.WPF;

namespace DnDStoryWriterHalper.Services.Converters
{
    public class PageObjectToAwesomeFontIconConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                TextPage => FontAwesomeIcon.AlignJustify,
                ImagePage => FontAwesomeIcon.Image,
                _ => FontAwesomeIcon.QuestionCircle
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
