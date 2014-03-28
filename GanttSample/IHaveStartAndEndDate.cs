using System;

namespace GanttSample
{
    public interface IHaveStartAndEndDate
    {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}