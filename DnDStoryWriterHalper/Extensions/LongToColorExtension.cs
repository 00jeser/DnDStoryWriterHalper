using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DnDStoryWriterHalper.Extensions
{
    public static class LongToColorExtension
    {
        public static Color ToARGBColor(this long value)
        {
            return Color.FromArgb(
                (byte)((value & 0xFF000000) >> 4 * 6),
                (byte)((value & 0x00FF0000) >> 4 * 4),
                (byte)((value & 0x0000FF00) >> 4 * 2),
                (byte)((value & 0x000000FF) >> 0)
            );
        }

        public static long ToLong(this Color value)
        {
            return 0L |
                   ((long)value.A << 4 * 6) |
                   ((long)value.R << 4 * 4) |
                   ((long)value.G << 4 * 2) |
                   ((long)value.B << 4 * 0);

        }
    }
}
