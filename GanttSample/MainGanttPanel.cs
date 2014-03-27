using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class MainGanttPanel : Panel
    {
        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(MainGanttPanel), new FrameworkPropertyMetadata(DateTime.Now.AddDays(0), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(MainGanttPanel), new FrameworkPropertyMetadata(DateTime.Now.AddHours(0), FrameworkPropertyMetadataOptions.AffectsMeasure));

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

            var ganttItems = Children.OfType<GanttGroupItem>().Where(x => x.IsItemVisible);
            foreach (var ganttItem in ganttItems)
            {
                ganttItem.Measure(availableSize);
                desiredHeight = ganttItem.DesiredSize.Height;
                double height = ganttItem.DesiredSize.Height * ganttItem.Order;
                if (height > maxHeight)
                    maxHeight = height;
            }

            var totalHeight = maxHeight + desiredHeight + 50;
            return new Size(0, totalHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double range = (MaxDate - MinDate).Ticks;
            double pixelsPerTick = finalSize.Width / range;

            var ganttItems = Children.OfType<GanttGroupItem>().Where(x => x.IsItemVisible);
            foreach (GanttGroupItem ganttItem in ganttItems)
            {
                var count = ganttItem.Items.Count;
                Rect rect = ArrangeChild(ganttItem, MinDate, pixelsPerTick, count * 22 + 20);
                ganttItem.Arrange(rect);
            }

            return finalSize;
        }

        private Rect ArrangeChild(GanttGroupItem child, DateTime minDate, double pixelsPerTick, double elementHeight)
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