namespace MCS.GridMvc.Ajax.GridExtensions
{
    public interface IAjaxGridPager
    {
        int Pages { get; }
        int PagePartitionSize { get; set; }
    }
}