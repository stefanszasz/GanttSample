using System.Collections.Generic;

namespace GanttSample
{
    public static class OrderedDateItemExtensions
    {
        public static IEnumerable<IOrderedDateItem> IntersectsWith(this IOrderedDateItem processItem, IEnumerable<IOrderedDateItem> items)
        {
            foreach (var item in items)
            {
                if (item.Equals(processItem)) continue;
                bool intersects = processItem.StartDate < item.EndDate && item.StartDate < processItem.EndDate;
                if (intersects)
                {
                    yield return item;
                }
            }
        }
    }
}