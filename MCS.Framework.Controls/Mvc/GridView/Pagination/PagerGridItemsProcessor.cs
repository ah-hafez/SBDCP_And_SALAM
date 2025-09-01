using System.Linq;

namespace MCS.Framework.Controls.Mvc
{
    /// <summary>
    ///     Cut's the current page from items collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagerGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
    {
        private readonly IGridPager _pager;

        public PagerGridItemsProcessor(IGridPager pager)
        {
            _pager = pager;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items, bool? useGridFunctionality)
        {
            _pager.Initialize(items); //init pager

            if (_pager.CurrentPage <= 0) return items; //incorrect page

            if (_pager.PagingByGrid)
            {
                int skip = (_pager.CurrentPage - 1) * _pager.PageSize;
                return items.Skip(skip).Take(_pager.PageSize);
            }

            return items.Take(_pager.PageSize);
        }

        #endregion
    }
}