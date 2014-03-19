using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    public class GanttControl : ListBox
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            RecalculateOrder();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GanttItem();
        }

        void RecalculateOrder()
        {
            var ganttItems = Items.OfType<object>().Select(x => ItemContainerGenerator.ContainerFromItem(x)).OfType<GanttItem>().ToList();
            ganttItems.ForEach(x => x.Order = 0);
            foreach (GanttItem item in ganttItems)
            {
                var intersectedItems = item.IntersectsWith(ganttItems);
                foreach (var intersectedItem in intersectedItems)
                {
                    intersectedItem.Order++;
                }
            }
        }
    }
}
