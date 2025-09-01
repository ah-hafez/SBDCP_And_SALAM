using MCS.GridMvc.Columns;
using System.Web;
using System.Web.Mvc;

namespace MCS.GridMvc
{
    public class GridCellRenderer : GridStyledRenderer, IGridCellRenderer
    {
        private const string TdClass = "grid-cell";

        public GridCellRenderer()
        {
            AddCssClass(TdClass);
        }

        public IHtmlString Render(IGridColumn column, IGridCell cell, object instance)
        {
            string cssStyles = GetCssStylesString();
            string cssClass = GetCssClassesString();

            var builder = new TagBuilder("td");
            if (!string.IsNullOrWhiteSpace(cssClass))
                builder.AddCssClass(cssClass);
            if (!string.IsNullOrWhiteSpace(cssStyles))
                builder.MergeAttribute("style", cssStyles);
            builder.MergeAttribute("data-name", column.Name);
            builder.MergeAttribute("data-th", column.Title);

            if (column.RenderEnabled(instance))
            {
                builder.InnerHtml = cell.ToString();
            }

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}