using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Handlers;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using MCS.Framework.Controls;

namespace MCS.Framework.Controls.Mvc
{
    public static class ChartExtensions
    {
        public static MvcHtmlString Chart(this HtmlHelper html, string chartControlId, string title, ChartType chartType, string dataSource, string depth, string dataSourceServiceUrl, string breadCrumbClassName = "", string fontFamily = "", string emptyDataMsg = "")
        {
            return MvcHtmlString.Create(Framework.Controls.Chart.RenderChart(chartControlId, title, chartType, dataSource, depth, dataSourceServiceUrl, breadCrumbClassName, fontFamily, emptyDataMsg));
        }

        public static MvcHtmlString Chart(this HtmlHelper html, string chartControlId, string title, ChartType chartType, string dataSource, string depth, string dataSourceServiceUrl, string[] arrayOfChartColors, string breadCrumbClassName = "", string fontFamily = "", string emptyDataMsg = "")
        {
            return MvcHtmlString.Create(Framework.Controls.Chart.RenderChart(chartControlId, title, chartType, dataSource, depth, dataSourceServiceUrl, arrayOfChartColors, breadCrumbClassName, fontFamily, emptyDataMsg));
        }

        public static MvcHtmlString ChartOnClick(this HtmlHelper html, string divChartId, string title, ChartType chartType, string dataSource, string depth, string dataSourceServiceUrl, string breadCrumbClassName = "", string fontFamily = "", string emptyDataMsg = "")
        {
            return MvcHtmlString.Create(Framework.Controls.Chart.RenderChartOnClick(divChartId, title, chartType, dataSource, depth, dataSourceServiceUrl, breadCrumbClassName, fontFamily, emptyDataMsg));
        }

        public static MvcHtmlString ChartOnClick(this HtmlHelper html, string divChartId, string title, ChartType chartType, string dataSource, string depth, string dataSourceServiceUrl, string[] arrayOfChartColors, string breadCrumbClassName = "", string fontFamily = "", string emptyDataMsg = "")
        {
            return MvcHtmlString.Create(Framework.Controls.Chart.RenderChartOnClick(divChartId, title, chartType, dataSource, depth, dataSourceServiceUrl, arrayOfChartColors, breadCrumbClassName, fontFamily, emptyDataMsg));
        }

        public static MvcHtmlString RenderChartResources(this HtmlHelper html)
        {
            return MvcHtmlString.Create(Framework.Controls.Chart.RenderChartResources());
        }
    }
}
