using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public GanttRowPanel()
        {
            Background = new SolidColorBrush(Colors.SeaShell);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (GanttItem child in Children)
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
                Rect rect = ArrangeChild(child, MinDate, pixelsPerTick, finalSize.Height);
                ArrangeGanttItem(child, rect);
            }

            return new Size(500, 500);
        }

        private Rect ArrangeChild(GanttItem child, DateTime minDate, double pixelsPerTick, double elementHeight)
        {
            DateTime childStartDate = child.StartDate;
            DateTime childEndDate = child.EndDate;
            TimeSpan childDuration = childEndDate - childStartDate;

            double offset = (childStartDate - minDate).Ticks * pixelsPerTick;
            double width = childDuration.Ticks * pixelsPerTick;

            double y = child.DesiredSize.Height * child.Order;
            var finalRect = new Rect(offset, y, width, elementHeight);
            return finalRect;
        }

        void ArrangeGanttItem(GanttItem ganttItem, Rect rect)
        {
            ganttItem.Arrange(rect);
        }
    }
}