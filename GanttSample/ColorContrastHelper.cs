using System.Windows.Media;

namespace GanttSample
{
    public static class ColorContrastHelper
    {
        public static Brush GetContrastForColor(Color sourceColor)
        {
            var luma = 0.2126 * sourceColor.ScR + 0.7152 * sourceColor.ScG + 0.0722 * sourceColor.ScB;
            if (luma < 0.5)
                return Brushes.White;

            return Brushes.Black;
        }
    }
}