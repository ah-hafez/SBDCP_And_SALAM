using System;
using System.Linq;

namespace MCS.Framework.Controls.Mvc
{
    public class AjaxGridPager : IGridPager, IAjaxGridPager
    {
        private readonly IGrid _grid;
        public int PagePartitionSize { get; set; }
        public int AllDataCounts { get; set; }
        public bool PagingByGrid { get; set; }

        public AjaxGridPager(IGrid grid)
        {
            _grid = grid;
        }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public string TemplateName
        {
            get
            {
                //Custom view name to render this pager
                return "_AjaxGridPager";
            }
        }

        /// <summary>
        ///     Returns true if the pager has pages
        /// </summary>
        public bool HasPages
        {
            get
            {
                return _grid.ItemsToDisplay.Count() > PageSize;
            }
        }

        public int Pages { get; protected set; }

        public void Initialize<T>(IQueryable<T> items)
        {
            //double pagesCount = Math.Ceiling((double)PagePartitionSize / PageSize);

            //Pages = Convert.ToInt32(pagesCount);

            if (PagingByGrid)
            {
                Pages = items.Count() / PageSize;
                if (items.Count() % PageSize > 0)
                    Pages++;
            }
            else
            {
                Pages = AllDataCounts / PageSize;
                if (AllDataCounts % PageSize > 0)
                    Pages++;
            }
        }
    }
}