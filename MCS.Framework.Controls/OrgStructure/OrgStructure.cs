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

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_01.png", "img/png")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_02.png", "img/png")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_03.png", "img/png")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_04.png", "img/png")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_05.png", "img/png")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_06.png", "img/png")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_07.png", "img/png")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.language.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.JS.jquery.ui.touch-punch.min.js", "text/javascript")]
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
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Styles.OrgStructure_ar.css", "text/css")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.OrgStructure.Styles.OrgStructure_en.css", "text/css")]

#endregion [ Resources ]

namespace MCS.Framework.Controls
{
    public static class OrgStructure
    {
        public static string RenderOrgStructure(string hdnIdToSaveDepartmentsData, string hdnIdToSaveSettings, string hdnToSaveSelectedDepartment, string languageShortName, string listOfActions, int limitation, string endPoint = "", bool showToolBox = true)
        {
            StringBuilder controlStream = new StringBuilder();

            controlStream.Append("<div class='OrgStructure'></div>");
            controlStream.AppendFormat("<script type='text/javascript'> OrgStructure('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}'); </script>", hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnToSaveSelectedDepartment, languageShortName, listOfActions, limitation, endPoint, showToolBox);

            return controlStream.ToString();
        }

        public static string RenderTaskWorkflow(string hdnIdToSaveDepartmentsData, string hdnIdToSaveSettings, string hdnIdToSaveTasksData, string hdnIdToSaveTaskIndex, string hdnToSaveSelectedDepartment, string languageShortName, string listOfActions, int limitation)
        {
            StringBuilder controlStream = new StringBuilder();

            controlStream.Append("<div class='OrgStructure'></div>");
            controlStream.AppendFormat("<script type='text/javascript'> TaskWorkflow('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}'); </script>", hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveTasksData, hdnIdToSaveTaskIndex, hdnToSaveSelectedDepartment, languageShortName, listOfActions, limitation);

            return controlStream.ToString();
        }

        public static string RenderOrgStructureResources(string shortCultureName = "ar")
        {
            StringBuilder controlStream = new StringBuilder();

            string _imgZoomIn = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_01.png", typeof(OrgStructure));
            string _imgZoomOut = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_02.png", typeof(OrgStructure));
            string _imgDelete = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_03.png", typeof(OrgStructure));
            string _imgDepartment = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_04.png", typeof(OrgStructure));
            string _imgLink = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_05.png", typeof(OrgStructure));
            
            string _imgSave = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_06.png", typeof(OrgStructure));
            string _imgCangeOrgUnitType = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.Images.toolbar_icon_07.png", typeof(OrgStructure));

            controlStream.AppendFormat("<script>var _imgZoomIn = '{0}'; var _imgZoomOut = '{1}'; var _imgDelete = '{2}'; var _imgDepartment = '{3}'; var _imgLink = '{4}'; var _imgSave = '{5}'; var _imgCangeOrgUnitType = '{6}'; </script>", _imgZoomIn, _imgZoomOut, _imgDelete, _imgDepartment, _imgLink, _imgSave, _imgCangeOrgUnitType);

            //string a29 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.language.js", typeof(OrgStructure));

            //string a = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jquery.ui.touch-punch.min.js", typeof(OrgStructure));
            //string a1 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jquery.contextmenu.js", typeof(OrgStructure));
            //string a2 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.OrgStructureScript.js", typeof(OrgStructure));

            //string a3 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.lib.biltong-0.2.js", typeof(OrgStructure));
            //string a4 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.lib.jsBezier-0.7.js", typeof(OrgStructure));
            //string a5 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.lib.katavorio-0.6.js", typeof(OrgStructure));
            //string a6 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.lib.mottle-0.6.js", typeof(OrgStructure));

            //string a7 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.anchors.js", typeof(OrgStructure));
            //string a8 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.base-library-adapter.js", typeof(OrgStructure));
            //string a9 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.browser-util.js", typeof(OrgStructure));
            //string a10 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connection.js", typeof(OrgStructure));
            //string a11 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connector-editors.js", typeof(OrgStructure));
            //string a12 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connectors-bezier.js", typeof(OrgStructure));
            //string a13 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connectors-flowchart.js", typeof(OrgStructure));
            //string a14 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.connectors-statemachine.js", typeof(OrgStructure));
            //string a15 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.defaults.js", typeof(OrgStructure));
            //string a16 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.dom-adapter.js", typeof(OrgStructure));
            //string a17 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.dom.jsPlumb.js", typeof(OrgStructure));
            //string a18 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.endpoint.js", typeof(OrgStructure));
            //string a19 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.jquery.jsPlumb.js", typeof(OrgStructure));
            //string a20 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.jsPlumb.js", typeof(OrgStructure));
            //string a22 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.overlay-component.js", typeof(OrgStructure));

            //string a23 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.renderers-svg.js", typeof(OrgStructure));
            //string a24 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.renderers-vml.js", typeof(OrgStructure));
            //string a25 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.src.util.js", typeof(OrgStructure));
            //string a26 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.JS.jsPlumb.jsPlumb-list.js", typeof(OrgStructure));
            //string a27 = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.OrgStructure.Styles.jquery.contextmenu.css", typeof(OrgStructure));
            //string a28 = EmbeddedResourcesHelper.WebResourceUrl(string.Format("MCS.Framework.Controls.OrgStructure.Styles.OrgStructure_{0}.css",shortCultureName), typeof(OrgStructure));

            //controlStream.AppendFormat("<link rel='stylesheet' href='{0}' />", a28);
            //controlStream.AppendFormat("<link rel='stylesheet' href='{0}' />", a27);



            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a29);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a1);

            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a4);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a6);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a3);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a5);

            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a25);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a9);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a20);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a16);

            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a22);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a18);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a10);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a7);

            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a15);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a12);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a14);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a13);

            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a11);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a23);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a24);

            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a8);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a17);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a2);
            //controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", a26);

            //return controlStream.ToString();

            string js = Scripts.Render("~/MCS.Framework.Controls/JSOrgStructure").ToString();
            string css = Styles.Render("~/MCS.Framework.Controls/StylesOrgStructure").ToString();

            return controlStream.ToString() + js + css;
        }
    }
}
