using MCS.GridMvc.Ajax.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MCS.GridMvc.Ajax.GridExtensions
{
    public class AjaxGrid<T> : Grid<T>, IAjaxGrid where T: class
    {
        public IAjaxGridPager AjaxGridSettings { get { return Pager as IAjaxGridPager; } }
        public bool HasItems { get { return Pager.CurrentPage <= AjaxGridSettings.Pages; } }

        public AjaxGrid(IList<T> items, int page,int itemsCount, bool renderOnlyRows, int pageSize, int pagePartitionSize=0)
            : base(items)
        {
            AfterItems = items;
            ItemsCount = itemsCount;           
            RenderOptions.RenderRowsOnly = renderOnlyRows;
            Pager = new AjaxGridPager(this) { CurrentPage = page, PageSize = pageSize};
            Pager.Initialize(itemsCount);
            AjaxGridSettings.PagePartitionSize = pagePartitionSize;
        }

        public string ToJson(string gridPartialViewName, Controller controller)
        {
            var htmlHelper = new KlaHtmlHelpers();
            return htmlHelper.RenderPartialViewToString(gridPartialViewName, this, controller);
        }

        public string ToJson(string gridPartialViewName, object model, Controller controller)
        {
            var htmlHelper = new KlaHtmlHelpers();
            return htmlHelper.RenderPartialViewToString(gridPartialViewName, model, controller);
        }
    }
}