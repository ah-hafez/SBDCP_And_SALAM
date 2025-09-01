using System.Linq;

namespace MCS.Framework.Controls.Mvc
{
    public class AjaxGridFactory : IAjaxGridFactory
    {
        public IAjaxGrid CreateAjaxGrid<T>(IQueryable<T> gridItems, int page, bool renderOnlyRows)
            where T : class
        {
            return CreateAjaxGrid(gridItems, page, renderOnlyRows, 0, false,0 );
        }

        public IAjaxGrid CreateAjaxGrid<T>(IQueryable<T> gridItems, int page, bool renderOnlyRows, int AllDataCounts, bool pagingByGrid = false, int pagePartitionSize = 0)
           where T : class
        {
            var grid = new AjaxGrid<T>(gridItems, page, renderOnlyRows, AllDataCounts, pagingByGrid, pagePartitionSize);
            return grid;
        }
    }
}