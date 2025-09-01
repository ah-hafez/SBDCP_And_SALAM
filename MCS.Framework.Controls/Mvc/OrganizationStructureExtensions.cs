using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Handlers;
using System.Web.Mvc;
using System.Web.Optimization;

#region [ Resources ]

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jquery.jquery.ui.touch-punch.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jquery.contextmenu.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.OrgStructureScript.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.lib.biltong-0.2.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.lib.jsBezier-0.7.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.lib.katavorio-0.6.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.lib.mottle-0.6.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.anchors.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.base-library-adapter.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.browser-util.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connection.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connector-editors.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connectors-bezier.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connectors-flowchart.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connectors-statemachine.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.defaults.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.dom-adapter.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.dom.jsPlumb.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.endpoint.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.jquery.jsPlumb.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.jsPlumb.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.overlays-guidelines.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.overlay-component.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.renderers-svg.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.renderers-vml.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.util.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.jsPlumb-list.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Styles.jquery.contextmenu.css", "text/css")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Styles.OrgStructure.css", "text/css")]

#endregion [ Resources ]
namespace MCS.Framework.Controls.Mvc
{
    public static class OrgStructureExtensions
    {
        public static MvcHtmlString OrgStructure(this HtmlHelper html, string hdnIdToSaveDepartmentsData, string hdnIdToSaveSettings, string hdnIdToSaveSelectedDepartment, string languageShortName, string listOfActions, int limitation , string endPoint="",bool showToolBox=true)
        {
            return MvcHtmlString.Create(Framework.Controls.OrgStructure.RenderOrgStructure(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, limitation, endPoint, showToolBox));
        }

        public static MvcHtmlString RenderOrgStructureResources(this HtmlHelper html)
        {
            return MvcHtmlString.Create(Framework.Controls.OrgStructure.RenderOrgStructureResources());
        }
        public static MvcHtmlString RenderOrgStructureResources(this HtmlHelper html, string shortCultureName)
        {
            if (!string.IsNullOrEmpty(shortCultureName))
            {
                return MvcHtmlString.Create(Framework.Controls.OrgStructure.RenderOrgStructureResources(shortCultureName));
            }
            return MvcHtmlString.Create(Framework.Controls.OrgStructure.RenderOrgStructureResources());
        }
    }
}
