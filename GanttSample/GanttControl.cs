using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GanttSample
{
    public class GanttControl : ListBox
    {
        readonly DoubleCollection strokeCollection = new DoubleCollection(new List<double> { 2 });

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(GanttControl), new FrameworkPropertyMetadata(DateTime.Now.AddHours(-8), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty MaxDateProperty =
       DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(GanttControl), new FrameworkPropertyMetadata(DateTime.Now.AddHours(8), FrameworkPropertyMetadataOptions.AffectsMeasure));

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

        private Canvas canvas;

        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.Register(
            "DateFormat", typeof(string), typeof(GanttControl), new FrameworkPropertyMetadata("H tt", FrameworkPropertyMetadataOptions.AffectsArrange));

        public string DateFormat
        {
            get { return (string)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        public GanttControl()
        {
            Loaded += GanttControl_Loaded;
            ((INotifyCollectionChanged)Items).CollectionChanged += ListBox_CollectionChanged;
        }

        private void ListBox_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            RecalculateOrder(element as GanttItem);
        }

        void GanttControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillBackground(new Size(ActualWidth, ActualHeight));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var containerForItemOverride = new GanttItem();
            return containerForItemOverride;
        }

        void RecalculateOrder(GanttItem item)
        {
            var ganttItems = Items.OfType<object>().Select(x => ItemContainerGenerator.ContainerFromItem(x)).OfType<GanttItem>().OrderBy(x => x.StartDate).ToList();
            var intersectedItems = item.IntersectsWith(ganttItems);
            foreach (var intersectedItem in intersectedItems)
            {
                if (intersectedItem.Order == item.Order)
                    item.Order++;
                else
                {
                    if (intersectedItem.Order - item.Order < 2)
                    {
                        
                    }
                }
                //int intersectedItemOrder = intersectedItem.Order;
                //item.Order++;
                //ganttItems.Where(x => x.Order < intersectedItemOrder);
            }
        }

        public override void OnApplyTemplate()
        {
            canvas = (Canvas)GetTemplateChild("BackgroundCanvas");
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            FillBackground(arrangeBounds);
            return base.ArrangeOverride(arrangeBounds);
        }

        void FillBackground(Size size)
        {
            double range = (MaxDate - MinDate).Ticks;
            double pixelsPerTick = size.Width / range;
            const int timeSliceMinutes = 15;

            canvas.Children.Clear();

            var start = new DateTime(MinDate.Year, MinDate.Month, MinDate.Day, MinDate.Hour, 0, 0);
            DateTime maxDate = MaxDate.AddHours(1);
            var addedDays = new List<int>();

            bool skippedFirstHour = false;
            for (DateTime currentTime = start; currentTime <= maxDate; currentTime = currentTime.AddMinutes(timeSliceMinutes))
            {
                DateTime time = currentTime;
                double offset = (time - MinDate).Ticks * pixelsPerTick;
                if (offset < 0)
                {
                    skippedFirstHour = true;
                    continue;
                }

                bool isWholeHour = time.Minute == 0;

                double y = size.Height;
                if (time.Minute == 15 || time.Minute == 45)
                    y = 10;
                else if (time.Minute == 30)
                    y = 20;

                bool isFirstDate;
                var dateTextBlock = new TextBlock { FontSize = 10, ToolTip = time.ToString("F") };
                if (addedDays.Contains(time.DayOfYear))
                {
                    dateTextBlock.Text = time.ToString(DateFormat);
                    isFirstDate = false;
                }
                else
                {
                    dateTextBlock.Text = time.ToString("MMMM dd hh:mm");
                    addedDays.Add(time.DayOfYear);
                    isFirstDate = true;
                }

                var line = new Line
                {
                    X1 = offset,
                    Y1 = 0,
                    X2 = offset,
                    Y2 = y,
                    Stroke = Brushes.LightSteelBlue,
                    StrokeThickness = 1,
                    ToolTip = time.ToString("F")
                };

                double dateBlockY = isFirstDate ? 25 : 35;

                if (isWholeHour)
                {
                    line.StrokeDashArray = strokeCollection;
                    Canvas.SetLeft(dateTextBlock, offset);
                    Canvas.SetTop(dateTextBlock, dateBlockY);
                    canvas.Children.Add(dateTextBlock);
                }
                else if (skippedFirstHour)
                {
                    dateTextBlock.Text = time.ToString("M");
                    Canvas.SetLeft(dateTextBlock, 0);
                    Canvas.SetTop(dateTextBlock, dateBlockY);
                    canvas.Children.Add(dateTextBlock);
                    skippedFirstHour = false;
                }

                canvas.Children.Add(line);
            }
        }

        public void ZoomIn()
        {
            MinDate = MinDate.AddHours(-1);
            MaxDate = MaxDate.AddHours(1);
        }

        public void ZoomOut()
        {
            MinDate = MinDate.AddHours(1);
            MaxDate = MaxDate.AddHours(-1);
        }

        public void MoveLeft()
        {
            MinDate = MinDate.AddHours(-1);
            MaxDate = MaxDate.AddHours(-1);
        }

        public void MoveRight()
        {
            MinDate = MinDate.AddHours(1);
            MaxDate = MaxDate.AddHours(1);
        }
    }
}
