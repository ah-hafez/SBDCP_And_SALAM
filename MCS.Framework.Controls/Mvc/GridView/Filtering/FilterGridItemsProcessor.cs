using System;
using System.Collections.Generic;
using System.Linq;

namespace MCS.Framework.Controls.Mvc
{
    /// <summary>
    ///     Grid items filter proprocessor
    /// </summary>
    internal class FilterGridItemsProcessor<T> : IGridItemsProcessor<T> where T : class
    {
        private readonly IGrid _grid;
        private IGridFilterSettings _settings;

        public FilterGridItemsProcessor(IGrid grid, IGridFilterSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _grid = grid;
            _settings = settings;
        }

        public void UpdateSettings(IGridFilterSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _settings = settings;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items, bool? useGridFunctionality)
        {
            foreach (IGridColumn column in _grid.Columns)
            {
                var gridColumn = column as IGridColumn<T>;
                if (gridColumn == null) continue;
                if (gridColumn.Filter == null) continue;

                IEnumerable<ColumnFilterValue> options = _settings.IsInitState
                                                             ? new List<ColumnFilterValue>
                                                                 {
                                                                     column.InitialFilterSettings
                                                                 }
                                                             : _settings.FilteredColumns.GetByColumn(column);

                if (useGridFunctionality.HasValue && useGridFunctionality.Value)
                {
                    foreach (ColumnFilterValue filterOptions in options)
                    {
                        if (filterOptions == ColumnFilterValue.Null)
                            continue;
                        items = gridColumn.Filter.ApplyFilter(items, filterOptions);
                    }
                }

            }
            return items;
        }

        #endregion
    }
}