using MCS.GridMvc.Columns;
using MCS.GridMvc.Html;
using MCS.GridMvc.Pagination;
using System.Collections.Generic;

namespace MCS.GridMvc
{
    /// <summary>
    ///     Grid.Mvc interface
    /// </summary>
    public interface IGrid
    {
        /// <summary>
        ///     Grid render options
        /// </summary>
        GridRenderOptions RenderOptions { get; }

        /// <summary>
        ///     Grid columns
        /// </summary>
        IGridColumnCollection Columns { get; }

        /// <summary>
        ///     Grid items
        /// </summary>
        IEnumerable<object> ItemsToDisplay { get; }

        ///// <summary>
        /////     Total grid items count
        ///// </summary>
        //int ItemsCount { get; set; }

        /// <summary>
        ///     Displaying grid items count
        /// </summary>
        int DisplayingItemsCount { get; }

        /// <summary>
        ///     Total items count in the grid (after filtering)
        /// </summary>
        int ItemsCount { get;  }

        /// <summary>
        ///     Pager for the grid
        /// </summary>
        IGridPager Pager { get; }

        /// <summary>
        ///     Enable paging view
        /// </summary>
        bool EnablePaging { get; }

        /// <summary>
        ///     Text in empty grid (no items for display)
        /// </summary>
        string EmptyGridText { get; }

        /// <summary>
        ///     Returns the current Grid language
        /// </summary>
        string Language { get; }

        /// <summary>
        ///     Object that sanitize grid column values from dangerous content
        /// </summary>
        ISanitizer Sanitizer { get; }

        IGridSettingsProvider Settings { get; }

        /// <summary>
        ///     Get all css classes mapped to the item
        /// </summary>
        string GetRowCssClasses(object item);

        /// <summary>
        /// Collection Name
        /// </summary>
        string CollectionName { get; }

        //void OnPreRender(); //TODO backward Compatibility
    }
}