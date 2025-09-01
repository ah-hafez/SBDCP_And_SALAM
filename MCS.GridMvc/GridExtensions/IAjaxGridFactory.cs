using System.Collections.Generic;
using System.Linq;

namespace MCS.GridMvc.Ajax.GridExtensions
{
    public interface IAjaxGridFactory
    {
        IAjaxGrid CreateAjaxGrid<T>(IList<T> gridItems, int page, int itemsCount, bool renderOnlyRows, int pagePartitionSize = 0)
            where T : class;
    }
}