using System.Collections.Generic;
using System.Linq;

namespace MCS.GridMvc
{
    /// <summary>
    ///     Preprocess items to display
    ///     This objects process initial collection of items in the Grid.Mvc (sorting, filtering, paging etc.)
    /// </summary>
    public interface IGridItemsProcessor<T> where T : class
    {
        IList<T> Process(IList<T> items);
    }

}