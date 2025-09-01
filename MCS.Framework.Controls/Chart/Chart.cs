using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Handlers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.HtmlControls;

#region [ Resources ]

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.ChartScript.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.Chart.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.src.Chart.Bar.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.src.Chart.Core.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.src.Chart.Doughnut.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.src.Chart.Line.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.src.Chart.PolarArea.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.src.Chart.Radar.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts..gitignore", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts..travis.yml", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.bower.json", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.CONTRIBUTING.md", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.gulpfile.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.Chart.JS.Charts.package.json", "text/javascript")]

#endregion [ Resources ]

namespace MCS.Framework.Controls
{
    public enum ChartType
    {
        Doughnut,
        Pie,
        Polararea,
        Bar,
        Line,
        Radar
    }

    public static class Chart
    {
        public static string RenderChart(string chartControlId, string title, ChartType chartType, string dataSource, string depth, string dataSourceServiceUrl, string breadCrumbClassName = "", string fontFamily = "", string emptyDataMsg = "")
        {
            StringBuilder controlStream = new StringBuilder();

            string divId = "div" + chartControlId;

            string style = string.Empty;

            if (title == null || title == string.Empty)
                style = "display:none;";

            controlStream.AppendFormat("<div id='{0}'><ul style='{1}' class='{2}'></ul><canvas id='{3}' width='400' height='200' /></div>", divId, style, breadCrumbClassName, chartControlId);

            controlStream.AppendFormat("<script type='text/javascript'> RenderChart('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '', '{7}', '{8}'); </script>", chartControlId, divId, title, chartType.ToString().ToLower(), dataSource, depth, dataSourceServiceUrl, fontFamily, emptyDataMsg);

            return controlStream.ToString();
        }

        public static string RenderChart(string chartControlId, string title, ChartType chartType, string dataSource, string depth, string dataSourceServiceUrl, string[] arrayOfChartColors, string breadCrumbClassName = "", string fontFamily = "", string emptyDataMsg = "")
        {
            string arrayOfColors = Newtonsoft.Json.JsonConvert.SerializeObject(arrayOfChartColors).ToString();

            StringBuilder controlStream = new StringBuilder();

            string divId = "div" + chartControlId;

            controlStream.AppendFormat("<div id='{0}'><ul class='{1}'></ul><canvas id='{2}' width='300' height='300' /></div>", divId, breadCrumbClassName, chartControlId);

            controlStream.AppendFormat("<script type='text/javascript'> RenderChart('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}'); </script>", chartControlId, divId, title, chartType.ToString().ToLower(), dataSource, depth, dataSourceServiceUrl, arrayOfColors, fontFamily, emptyDataMsg);

            return controlStream.ToString();
        }

        public static string RenderChartOnClick(string divChartId, string title, ChartType chartType, string dataSource, string depth, string dataSourceServiceUrl, string breadCrumbClassName = "", string fontFamily = "", string emptyDataMsg = "")
        {
            StringBuilder controlStream = new StringBuilder();

            controlStream.AppendFormat("RenderChartOnClick('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '', '{6}', '{7}', '{8}');", divChartId, title, chartType.ToString().ToLower(), dataSource, depth, dataSourceServiceUrl, breadCrumbClassName, fontFamily, emptyDataMsg);

            return controlStream.ToString();
        }

        public static string RenderChartOnClick(string divChartId, string title, ChartType chartType, string dataSource, string depth, string dataSourceServiceUrl, string[] arrayOfChartColors, string breadCrumbClassName = "", string fontFamily = "", string emptyDataMsg = "")
        {
            StringBuilder controlStream = new StringBuilder();

            controlStream.AppendFormat("RenderChartOnClick('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}');", divChartId, title, chartType.ToString().ToLower(), dataSource, depth, dataSourceServiceUrl, arrayOfChartColors, breadCrumbClassName, fontFamily, emptyDataMsg);

            return controlStream.ToString();
        }

        public static string RenderChartResources()
        {
            string js = Scripts.Render("~/MCS.Framework.Controls/JSChart").ToString();

            return js;

            //StringBuilder controlStream = new StringBuilder();

            //string a = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Chart.JS.ChartScript.js", typeof(Chart));
            //string a1 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.Chart.JS.Charts.Chart.js", typeof(Chart));

            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a1);

            //return controlStream.ToString();
        }
    }
}
