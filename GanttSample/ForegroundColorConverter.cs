using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GanttSample
{
    public class ForegroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = (SolidColorBrush)value;

            return ColorContrastHelper.GetContrastForColor(brush.Color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}