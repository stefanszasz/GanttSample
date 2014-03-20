using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class GanttRowPanel : Panel
    {
        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(GanttRowPanel), new FrameworkPropertyMetadata(DateTime.Now.AddDays(0), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(GanttRowPanel), new FrameworkPropertyMetadata(DateTime.Now.AddHours(0), FrameworkPropertyMetadataOptions.AffectsMeasure));

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
            double maxHeight = 0;
            double desiredHeight = 0;

            foreach (GanttItem child in Children.OfType<GanttItem>().Where(x => x.IsItemVisible))
            {
                child.Measure(availableSize);
                desiredHeight = child.DesiredSize.Height;
                double height = child.DesiredSize.Height * child.Order;
                if (height > maxHeight)
                    maxHeight = height;
            }

            return new Size(0, maxHeight + desiredHeight + 50);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double range = (MaxDate - MinDate).Ticks;
            double pixelsPerTick = finalSize.Width / range;

            foreach (GanttItem child in Children.OfType<GanttItem>().Where(x => x.IsItemVisible))
            {
                Rect rect = ArrangeChild(child, MinDate, pixelsPerTick, finalSize.Height);
                child.Arrange(rect);
            }

            return finalSize;
        }

        private Rect ArrangeChild(GanttItem child, DateTime minDate, double pixelsPerTick, double elementHeight)
        {
            DateTime childStartDate = child.StartDate;
            DateTime childEndDate = child.EndDate;
            TimeSpan childDuration = childEndDate - childStartDate;

            double offset = (childStartDate - minDate).Ticks * pixelsPerTick;
            double width = childDuration.Ticks * pixelsPerTick;

            double y = child.DesiredSize.Height * child.Order;
            if (offset < 0)
                offset = 0;

            if (width < 0)
                width = 0;

            var finalRect = new Rect(offset, y + 50, width, elementHeight);
            return finalRect;
        }
    }
}