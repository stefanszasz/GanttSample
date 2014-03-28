namespace GanttSample
{
    public interface IOrderedDateItem : IHaveStartAndEndDate
    {
        int Order { get; set; }
    }
}