using System.Linq;

namespace MCS.Framework.Controls.Mvc
{
    public interface IAjaxGridFactory
    {
        IAjaxGrid CreateAjaxGrid<T>(IQueryable<T> gridItems, int page, bool renderOnlyRows, int AllDataCounts = 0, bool pagingByGrid = false, int pagePartitionSize =0)
            where T : class;
    }
}