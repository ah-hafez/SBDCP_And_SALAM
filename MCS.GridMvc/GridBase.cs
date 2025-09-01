using System;
using System.Collections.Generic;
using System.Linq;

namespace MCS.GridMvc
{
    /// <summary>
    ///     Base implementation of the Grid.Mvc
    /// </summary>
    public abstract class GridBase<T> : List<T> where T : class
    {
        //pre-processors process items before adds to main collection (like filtering)
        private readonly List<IGridItemsProcessor<T>> _preprocessors = new List<IGridItemsProcessor<T>>();
        private readonly List<IGridItemsProcessor<T>> _processors = new List<IGridItemsProcessor<T>>();
        protected IList<T> AfterItems; //items after processors
        protected IList<T> BeforeItems; //items before processors


        private int _itemsCount = -1; // total items count on collection
        private bool _itemsPreProcessed; //is preprocessors launched?
        private bool _itemsProcessed; //is processors launched?

        private Func<T, string> _rowCssClassesContraint;
                
        protected GridBase(IList<T> items)
        {
            BeforeItems = items;
        }

        public abstract IGridSettingsProvider Settings { get; set; }

        public IList<T> GridItems
        {
            get
            {
                //call preprocessors before:
                if (!_itemsPreProcessed)
                {
                    _itemsPreProcessed = true;
                    foreach (var gridItemsProcessor in _preprocessors)
                    {
                        BeforeItems = gridItemsProcessor.Process(BeforeItems);
                    }
                }
                return BeforeItems;
            }
        }


        /// <summary>
        ///     Text in empty grid (no items for display)
        /// </summary>
        public string EmptyGridText { get; set; }

        /// <summary>
        /// Total count of items in the grid
        /// </summary>
        public int ItemsCount
        {
            get
            {
                return _itemsCount;
            }
            set
            {
                _itemsCount = value; //value can be set by pager (for minimizing db calls)
            }
        }

        #region Custom row css classes
        public void SetRowCssClassesContraint(Func<T, string> contraint)
        {
            _rowCssClassesContraint = contraint;
        }

        public string GetRowCssClasses(object item)
        {
            if (_rowCssClassesContraint == null)
                return string.Empty;
            var typed = item as T;
            if (typed == null)
                throw new InvalidCastException(string.Format("The item must be of type '{0}'", typeof(T).FullName));
            return _rowCssClassesContraint(typed);
        }

        #endregion

        protected void PrepareItemsToDisplay()
        {
            if (!_itemsProcessed)
            {
                _itemsProcessed = true;
                IList<T> itemsToProcess = GridItems;
                foreach (var processor in _processors.Where(p => p != null))
                {
                    itemsToProcess = processor.Process(itemsToProcess);
                }
                AfterItems = itemsToProcess.ToList(); //select from db (in EF case)
            }
        }

        #region Processors methods

        protected void AddItemsProcessor(IGridItemsProcessor<T> processor)
        {
            if (!_processors.Contains(processor))
                _processors.Add(processor);
        }

        protected void RemoveItemsProcessor(IGridItemsProcessor<T> processor)
        {
            if (_processors.Contains(processor))
                _processors.Remove(processor);
        }

        protected void AddItemsPreProcessor(IGridItemsProcessor<T> processor)
        {
            if (!_preprocessors.Contains(processor))
                _preprocessors.Add(processor);
        }

        protected void RemoveItemsPreProcessor(IGridItemsProcessor<T> processor)
        {
            if (_preprocessors.Contains(processor))
                _preprocessors.Remove(processor);
        }

        protected void InsertItemsProcessor(int position, IGridItemsProcessor<T> processor)
        {
            if (!_processors.Contains(processor))
                _processors.Insert(position, processor);
        }

        #endregion

    }
}