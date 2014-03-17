using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    class GanttControl2 : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GanttItem();
        }

        //protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        //{
        //    var ganttItem = (GanttItem)element;
        //    ganttItem.DataContext = item;
        //}
    }
}
