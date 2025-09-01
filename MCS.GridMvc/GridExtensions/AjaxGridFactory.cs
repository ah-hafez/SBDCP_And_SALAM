using System.Collections.Generic;
using System.Linq;

namespace MCS.GridMvc.Ajax.GridExtensions
{
    public class AjaxGridFactory : IAjaxGridFactory
    {
        public IAjaxGrid CreateAjaxGrid<T>(IList<T> gridItems, int page, int itemsCount, bool renderOnlyRows, int pageSize = 10)
            where T : class
        {
            return CreateAjaxGrid(gridItems, page, itemsCount, renderOnlyRows, pageSize,0);
        }

        public IAjaxGrid CreateAjaxGrid<T>(IList<T> gridItems, int page, int itemsCount, bool renderOnlyRows, int pageSize, int pagePartitionSize)
           where T : class
        {
            var grid = new AjaxGrid<T>(gridItems, page, itemsCount, renderOnlyRows, pageSize, pagePartitionSize);
            return grid;
        }
    }
}