using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class GanttItemPanel : Panel
    {
        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(GanttItemPanel), new FrameworkPropertyMetadata(DateTime.Now.AddDays(0), 
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(GanttItemPanel), new FrameworkPropertyMetadata(DateTime.Now.AddHours(0),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {

        }

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
            double desiredHeight = 0;
            var ganttItems = Children.OfType<GanttItem>().Where(x => x.IsItemVisible).ToArray();
            foreach (var ganttItem in ganttItems)
            {
                ganttItem.Measure(availableSize);
                desiredHeight = ganttItem.DesiredSize.Height;
            }

            int max = ganttItems.Max(x => x.Order) + 1;

            return new Size(0, desiredHeight * max);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double range = (MaxDate - MinDate).Ticks;
            double pixelsPerTick = finalSize.Width / range;

            var ganttItems = Children.OfType<GanttItem>().Where(x => x.IsItemVisible);
            foreach (var ganttItem in ganttItems)
            {
                Rect rect = ArrangeChild(ganttItem, MinDate, pixelsPerTick, finalSize.Height);
                ganttItem.Arrange(rect);
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

            var finalRect = new Rect(offset, y, width, elementHeight);
            return finalRect;
        }
    }
}