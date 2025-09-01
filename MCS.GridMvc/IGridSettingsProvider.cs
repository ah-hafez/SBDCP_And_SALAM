using MCS.GridMvc.Filtering;
using MCS.GridMvc.Sorting;

namespace MCS.GridMvc
{
    /// <summary>
    ///     Setting for grid
    /// </summary>
    public interface IGridSettingsProvider
    {
        IGridSortSettings SortSettings { get; }
        IGridFilterSettings FilterSettings { get; }
        IGridColumnHeaderRenderer GetHeaderRenderer();
    }
}