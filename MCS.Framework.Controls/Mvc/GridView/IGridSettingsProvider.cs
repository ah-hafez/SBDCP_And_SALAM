
namespace MCS.Framework.Controls.Mvc
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