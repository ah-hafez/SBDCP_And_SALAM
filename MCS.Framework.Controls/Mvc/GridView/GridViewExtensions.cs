using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

#region [ Resources ]

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.GridView.JS.URI.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.GridView.JS.gridmvc.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.GridView.JS.gridmvc-ext.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.GridView.JS.gridmvc.customwidgets.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.GridView.JS.gridmvc.customwidgetsText.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.GridView.JS.gridmvcAR.js", "text/javascript")]

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.GridView.Styles.Gridmvc.css", "text/css")]

#endregion [ Resources ]

namespace MCS.Framework.Controls.Mvc
{
    public static class GridViewExtensions
    {
        public static MvcHtmlString RenderGridViewResources(this HtmlHelper html)
        {

            //string scripts = System.Web.Optimization.Scripts.Render("~/MCS.Framework.Controls/Mvc/JSGridView").ToHtmlString();

            //string replaced = Regex.Replace(scripts, "src=\"/", "src=\"" + HttpContext.Current.Request.Url.Host + "/", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            //string js = replaced;

            //string css = Styles.Render("~/MCS.Framework.Controls/Mvc/StylesGridView").ToString();

            //string replaced1 = Regex.Replace(css, "href=\"/", "href=\"" + HttpContext.Current.Request.Url.Host + "/", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            string js = Scripts.Render("~/MCS.Framework.Controls/Mvc/JSGridView").ToString();
            string css = Styles.Render("~/MCS.Framework.Controls/Mvc/StylesGridView").ToString();

            return MvcHtmlString.Create(js + css);
        }
    }
}
