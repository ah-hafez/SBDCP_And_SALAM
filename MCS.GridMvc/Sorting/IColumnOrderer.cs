using System.Collections.Generic;
using System.Linq;

namespace MCS.GridMvc.Sorting
{
    /// <summary>
    ///     Custom user column orderer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IColumnOrderer<T>
    {
        IList<T> ApplyOrder(IList<T> items);
        IList<T> ApplyOrder(IList<T> items, GridSortDirection direction);
    }
}