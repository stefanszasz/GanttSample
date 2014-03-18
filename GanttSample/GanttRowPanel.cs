using System;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class GanttRowPanel : Panel
    {
        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(GanttRowPanel), new FrameworkPropertyMetadata(DateTime.Now.AddHours(12), FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(GanttRowPanel), new FrameworkPropertyMetadata(DateTime.Now.AddHours(-12), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public DateTime MaxDate
        {
            get { return (DateTime)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }

        public DateTime MinDate
        {
            get { return (DateTime)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return new Size(0, 0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double range = (MaxDate - MinDate).Ticks;
            double pixelsPerTick = finalSize.Width / range;

            foreach (GanttItem child in Children)
            {
                ArrangeChild(child, MinDate, pixelsPerTick, finalSize.Height);
            }

            return finalSize;
        }

        private void ArrangeChild(GanttItem child, DateTime minDate, double pixelsPerTick, double elementHeight)
        {
            DateTime childStartDate = child.StartDate;
            DateTime childEndDate = child.EndDate;
            TimeSpan childDuration = childEndDate - childStartDate;

            double offset = (childStartDate - minDate).Ticks * pixelsPerTick;
            double width = childDuration.Ticks * pixelsPerTick;

            int y = 22 * child.Order;
            var finalRect = new Rect(offset, 5 + y, width, elementHeight);
            child.Arrange(finalRect);
        }
    }
}