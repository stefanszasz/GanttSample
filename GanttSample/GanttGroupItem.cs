using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class GanttGroupItem : ListBox
    {
        public event Action OnNewItemAdded = delegate { };

        public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register(
            "StartDate", typeof(DateTime), typeof(GanttGroupItem), new FrameworkPropertyMetadata(DateTime.Now.AddHours(0),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register(
            "EndDate", typeof(DateTime), typeof(GanttGroupItem), new FrameworkPropertyMetadata(DateTime.Now.AddHours(0),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

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
            "Order", typeof(int), typeof(GanttGroupItem), new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public int Order
        {
            get { return (int)GetValue(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        public GanttGroupItem()
        {
            DefaultStyleKey = typeof(GanttGroupItem);
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

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                OnNewItemAdded();
            }
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
                    {
                        var items = intersectedItems.Select(x => x.Order).OrderBy(x => x).ToList();
                        if (items.Count > 1)
                        {
                            bool wasSet = false;
                            for (int i = 0; i < items.Count - 1; i++)
                            {
                                if (items[i + 1] - items[i] > 1)
                                {
                                    item.Order = items[i] + 1;
                                    wasSet = true;
                                    break;
                                }
                            }
                            if (wasSet == false)
                            {
                                item.Order = items.Last() + 1;
                            }
                        }
                        else
                        {
                            item.Order = items.Count;
                        }
                    }
                    else
                    {
                        item.Order = 0;
                    }

                    break;
                }
            }
        }
    }
}