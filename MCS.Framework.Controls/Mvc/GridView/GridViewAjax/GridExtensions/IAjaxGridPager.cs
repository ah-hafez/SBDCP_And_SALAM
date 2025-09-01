namespace MCS.Framework.Controls.Mvc
{
    public interface IAjaxGridPager
    {
        int Pages { get; }
        int PagePartitionSize { get; set; }
        int AllDataCounts { get; set; }

    }
}