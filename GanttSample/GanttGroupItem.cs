using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class GanttGroupItem : ListBox
    {
        public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register(
            "StartDate", typeof(DateTime), typeof(GanttGroupItem), new PropertyMetadata(default(DateTime)));

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register(
            "EndDate", typeof(DateTime), typeof(GanttGroupItem), new PropertyMetadata(default(DateTime)));

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(GanttGroupItem), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty OrderProperty = DependencyProperty.Register(
            "Order", typeof(int), typeof(GanttGroupItem), new PropertyMetadata(default(int)));

        public int Order
        {
            get { return (int)GetValue(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        public bool IsItemVisible { get; set; }

        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(GanttGroupItem), new FrameworkPropertyMetadata(DateTime.Now.AddHours(-8), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty MaxDateProperty =
       DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(GanttGroupItem), new FrameworkPropertyMetadata(DateTime.Now.AddHours(8), FrameworkPropertyMetadataOptions.AffectsMeasure));

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

        public GanttGroupItem()
        {
            DefaultStyleKey = typeof(GanttGroupItem);
            IsItemVisible = true;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var containerForItemOverride = new GanttItem();
            return containerForItemOverride;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var ganttGroupItem = (GanttItem)element;
            RecalculateOrder(ganttGroupItem);            
        }

        void RecalculateOrder(GanttItem item)
        {
            var ganttItems = Items.OfType<object>().Select(x => ItemContainerGenerator.ContainerFromItem(x)).OfType<GanttItem>().OrderBy(x => x.StartDate).ToList();
            var intersectedItems = item.IntersectsWith(ganttItems).ToArray();
            foreach (var intersectedItem in intersectedItems)
            {
                if (intersectedItem.Order == item.Order)
                    item.Order++;
                else
                {
                    int minimumOrder = intersectedItems.Min(x => x.Order);
                    if (minimumOrder == 0)
                        item.Order = intersectedItems.Max(x => x.Order) + 1;
                    else
                        item.Order = minimumOrder - 1;

                    break;
                }
            }
        }
    }
}