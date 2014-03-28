using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GanttSample
{
    [TemplatePart(Name = "PART_Background", Type = typeof(Canvas))]
    public class GanttControl : ListBox
    {
        readonly DoubleCollection strokeCollection = new DoubleCollection(new List<double> { 2 });

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(GanttControl), new FrameworkPropertyMetadata(DateTime.Now.AddHours(-8),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty MaxDateProperty =
       DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(GanttControl), new FrameworkPropertyMetadata(DateTime.Now.AddHours(8),
           FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

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

        public static readonly DependencyProperty ShowDateTimeLinesProperty = DependencyProperty.Register(
            "ShowDateTimeLines", typeof(bool), typeof(GanttControl), new PropertyMetadata(true));

        public bool ShowDateTimeLines
        {
            get { return (bool)GetValue(ShowDateTimeLinesProperty); }
            set { SetValue(ShowDateTimeLinesProperty, value); }
        }

        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.Register(
            "DateFormat", typeof(string), typeof(GanttControl), new FrameworkPropertyMetadata("H tt", FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty ChildrenPropertyNameProperty = DependencyProperty.Register(
            "ChildrenPropertyName", typeof(string), typeof(GanttControl), new PropertyMetadata("Children"));

        public string ChildrenPropertyName
        {
            get { return (string)GetValue(ChildrenPropertyNameProperty); }
            set { SetValue(ChildrenPropertyNameProperty, value); }
        }

        public string DateFormat
        {
            get { return (string)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        private Canvas canvas;

        public GanttControl()
        {
            Loaded += GanttControl_Loaded;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (element == null) throw new ArgumentNullException("element");
            if (item == null) throw new ArgumentNullException("item");

            base.PrepareContainerForItemOverride(element, item);
            var ganttGroupItem = (GanttGroupItem)element;
            ganttGroupItem.OnNewItemAdded += () => RecalculateOrder(ganttGroupItem);
            ganttGroupItem.ItemsSource = (IEnumerable)item.GetType().GetProperty(ChildrenPropertyName).GetValue(item, null);
            RecalculateOrder(ganttGroupItem);
        }

        void GanttControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillBackground(new Size(ActualWidth, ActualHeight));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var containerForItemOverride = new GanttGroupItem();
            return containerForItemOverride;
        }

        void RecalculateOrder(GanttGroupItem item)
        {
            var ganttItems = Items.OfType<object>().Select(x => ItemContainerGenerator.ContainerFromItem(x)).OfType<GanttGroupItem>().OrderBy(x => x.StartDate).ToList();
            var intersectedItems = item.IntersectsWith(ganttItems).ToArray();
            ReorderingItemsHelper.ReorderItemsBasedOnDate(item, intersectedItems);
        }

        public override void OnApplyTemplate()
        {
            canvas = (Canvas)GetTemplateChild("PART_Background");
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            FillBackground(arrangeBounds);
            return base.ArrangeOverride(arrangeBounds);
        }

        void FillBackground(Size size)
        {
            if (ShowDateTimeLines == false)
            {
                canvas.Visibility = Visibility.Collapsed;
                return;
            }

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
