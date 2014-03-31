using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class MainGanttPanel : Panel
    {
        readonly Dictionary<Guid, double> itemsHeight = new Dictionary<Guid, double>();

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

            var ganttItems = Children.OfType<GanttGroupItem>();
            foreach (var ganttItem in ganttItems)
            {
                ganttItem.Measure(availableSize);
                desiredHeight = ganttItem.DesiredSize.Height;
                double height = ganttItem.DesiredSize.Height * ganttItem.Order;
                if (height > maxHeight)
                    maxHeight = height;
            }

            var totalHeight = maxHeight + desiredHeight;
            return new Size(0, totalHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double range = (MaxDate - MinDate).Ticks;
            double pixelsPerTick = finalSize.Width / range;

            var ganttItems = Children.OfType<GanttGroupItem>();
            foreach (GanttGroupItem ganttItem in ganttItems)
            {
                Rect rect = ArrangeChild(ganttItem, MinDate, pixelsPerTick, finalSize.Height);
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
            double newHeight = y;
            if (Math.Abs(y) > 0.1)
            {
                double existingHeight = itemsHeight.Sum(x => x.Value);
                double tempHeight = Math.Max(y, existingHeight) - child.DesiredSize.Height;
                newHeight = Math.Max(tempHeight, newHeight);
            }

            itemsHeight[child.Id] = child.DesiredSize.Height;
            
            var finalRect = new Rect(offset, newHeight, width, elementHeight);
            return finalRect;
        }
    }
}