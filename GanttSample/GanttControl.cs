using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GanttSample
{
    class GanttControl : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GanttItem();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);            
        }
    }
}
