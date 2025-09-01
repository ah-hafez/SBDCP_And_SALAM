using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace MCS.Framework.Controls.Mvc
{
    public class AjaxGrid<T> : Grid<T>, IAjaxGrid where T : class
    {
        public IAjaxGridPager AjaxGridSettings { get { return Pager as IAjaxGridPager; } }
        public bool HasItems { get { return Pager.CurrentPage <= AjaxGridSettings.Pages; } }

        public AjaxGrid(IQueryable<T> items, int page, bool renderOnlyRows, int AllDataCounts = 0, bool useGridFunctionality = false, int pagePartitionSize = 0)
            : base(items)
        {
            UseGridFunctionality = useGridFunctionality;
            Pager = new AjaxGridPager(this) { CurrentPage = page, PagingByGrid = useGridFunctionality };
            RenderOptions.RenderRowsOnly = renderOnlyRows;
            AjaxGridSettings.AllDataCounts = AllDataCounts;

            if (pagePartitionSize > 0)
            {
                AjaxGridSettings.PagePartitionSize = pagePartitionSize;
            }
            else
            {
                if (ConfigurationManager.AppSettings["GridPagePartitionSize"] != null)
                {
                    AjaxGridSettings.PagePartitionSize = Convert.ToInt32(ConfigurationManager.AppSettings["GridPagePartitionSize"]);
                }
            }
        }

        public string ToJson(string gridPartialViewName, Controller controller)
        {
            var htmlHelper = new KlaHtmlHelpers();
            return htmlHelper.RenderPartialViewToString(gridPartialViewName, this, controller);
        }

        public string ToJson(string gridPartialViewName, Object model, Controller controller)
        {
            var htmlHelper = new KlaHtmlHelpers();
            return htmlHelper.RenderPartialViewToString(gridPartialViewName, model, controller);
        }
    }
}