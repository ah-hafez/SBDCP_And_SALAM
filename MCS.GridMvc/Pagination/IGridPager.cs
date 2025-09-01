using System.Linq;

namespace MCS.GridMvc.Pagination
{
    public interface IGridPager
    {
        /// <summary>
        ///     Max grid items, displaying on the page
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        ///     Current page index
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        ///     Partial view name to render the pager
        /// </summary>
        string TemplateName { get; }

        /// <summary>
        /// Grid Name
        /// </summary>
        string GridName { get; }

        /// <summary>
        ///     Method invokes before pager render
        /// </summary>
        void Initialize(int itemsCount);

        ///// <summary>
        /////     Total pages count
        ///// </summary>
        //int PageCount { get; }

        ///// <summary>
        /////     Starting displaying page
        ///// </summary>
        //int StartDisplayedPage { get; }

        ///// <summary>
        /////     Last displaying page
        ///// </summary>
        //int EndDisplayedPage { get; }

        //int MaxDisplayedPages { get; set; }

        //string ParameterName { get; }

        //int ItemsCount { get; set; }

        ///// <summary>
        /////     Получить адрес для конкретной страницы
        ///// </summary>
        ///// <param name="pageIndex">Номер страницы</param>
        ///// <returns>Адрес страницы</returns>
        //string GetLinkForPage(int pageIndex);
    }
}