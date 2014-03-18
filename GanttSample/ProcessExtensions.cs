using System.Collections.Generic;
using System.Linq;

namespace GanttSample
{
    public static class ProcessExtensions
    {
        public static IEnumerable<Process> IntersectsWith(this Process processItem, IEnumerable<Process> items)
        {
            foreach (Process item in items.Where(x => x.StartDate > processItem.StartDate))
            {
                if (item.Equals(processItem)) continue;

                bool intersects = processItem.EndDate > item.StartDate;
                if (intersects)
                {
                    yield return item;
                }
            }
        }
    }
}