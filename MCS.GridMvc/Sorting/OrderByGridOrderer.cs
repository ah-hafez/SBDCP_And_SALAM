using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MCS.GridMvc.Sorting
{
    /// <summary>
    ///     Object applies order (OrderBy, OrderByDescending) for items collection
    /// </summary>
    internal class OrderByGridOrderer<T, TKey> : IColumnOrderer<T>
    {
        private readonly Expression<Func<T, TKey>> _expression;

        public OrderByGridOrderer(Expression<Func<T, TKey>> expression)
        {
            _expression = expression;
        }

        #region IColumnOrderer<T> Members

        public IList<T> ApplyOrder(IList<T> items)
        {
            return items;
           // return ApplyOrder(items, GridSortDirection.Ascending);
        }

        public IList<T> ApplyOrder(IList<T> items, GridSortDirection direction)
        {
            return items;
            //switch (direction)
            //{
            //    case GridSortDirection.Ascending:
            //        return items.OrderBy(_expression);
            //    case GridSortDirection.Descending:
            //        return items.OrderByDescending(_expression);
            //    default:
            //        throw new ArgumentOutOfRangeException("direction");
            //}
        }

        #endregion
    }
}