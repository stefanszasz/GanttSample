using System.Linq;

namespace GanttSample
{
    public static class ReorderingItemsHelper
    {
        public static void ReorderItemsBasedOnDate(IOrderedDateItem item, IOrderedDateItem[] intersectedItems)
        {
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