using System;
using System.Collections.Generic;
using System.Linq;

namespace GanttSample
{
    public static class ProcessExtensions
    {
        public static IEnumerable<GanttItem> IntersectsWith(this GanttItem processItem, IEnumerable<GanttItem> items)
        {
            foreach (GanttItem item in items)
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