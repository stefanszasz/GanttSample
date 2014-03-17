using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GanttSample
{
    public class GanttItem : Control
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Brush), typeof(GanttItem), new PropertyMetadata(default(Brush)));

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register(
            "StartDate", typeof(DateTime), typeof(GanttItem), new PropertyMetadata(default(DateTime), OnStartDateChanged));

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        private static void OnStartDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GanttItem item = d as GanttItem;
            GanttRowPanel.SetStartDate(item, item.StartDate);
        }

        public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register(
            "EndDate", typeof(DateTime), typeof(GanttItem), new PropertyMetadata(default(DateTime), OnEndDateChanged));

        private static void OnEndDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GanttItem item = d as GanttItem;
            GanttRowPanel.SetEndDate(item, item.EndDate);
        }

        public GanttItem()
        {
            DefaultStyleKey = typeof (GanttItem);
        }

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    drawingContext.DrawRectangle(Color, null,
        //        new Rect(0, 0, RenderSize.Width, RenderSize.Height));
        //}
    }
}