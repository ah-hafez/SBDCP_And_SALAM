using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Optimization;

namespace MCS.Framework.Controls
{
    public sealed class BundlingHelper
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.VirtualPathProvider = new EmbeddedVirtualPathProvider(HostingEnvironment.VirtualPathProvider);

            //
            //TextEditor
            //
            bundles.Add(new ScriptBundle("~/MCS.Framework.Controls/JSTextEditor")
               .Include("~/MCS.Framework.Controls/tinymce.min.js?TextEditor.JS.tinymce")
               .Include("~/MCS.Framework.Controls/TextEditorScript.js?TextEditor.JS")
               );

            //
            //AutoComplete
            //
            bundles.Add(new ScriptBundle("~/MCS.Framework.Controls/JSAutoComplete")
               .Include("~/MCS.Framework.Controls/AutoCompleteScript.js?AutoComplete.JS")
               );

            //
            //Chart
            //
            bundles.Add(new ScriptBundle("~/MCS.Framework.Controls/JSChart")
               .Include("~/MCS.Framework.Controls/ChartScript.js?Chart.JS")
               .Include("~/MCS.Framework.Controls/Chart.js?Chart.JS.Charts")
               );

            //
            //Signature
            //
            bundles.Add(new StyleBundle("~/MCS.Framework.Controls/StylesSignature")
               .Include("~/MCS.Framework.Controls/jSignature.css?eSignature.Styles")
               );

            bundles.Add(new ScriptBundle("~/MCS.Framework.Controls/JSSignature")
               .Include("~/MCS.Framework.Controls/jSignature.js?eSignature.JS")
               .Include("~/MCS.Framework.Controls/SignatureScript.js?eSignature.JS")
               );

            //
            //Calendar
            //
            bundles.Add(new StyleBundle("~/MCS.Framework.Controls/StylesCalendar")
               .Include("~/MCS.Framework.Controls/jquery.calendars.picker.css?Calendar.Styles")
               );

            bundles.Add(new ScriptBundle("~/MCS.Framework.Controls/JSCalendar")
               .Include("~/MCS.Framework.Controls/jquery.calendars.js?Calendar.JS")
               .Include("~/MCS.Framework.Controls/jquery.calendars.plus.js?Calendar.JS")
               .Include("~/MCS.Framework.Controls/jquery.calendars.picker.js?Calendar.JS")
               .Include("~/MCS.Framework.Controls/jquery.calendars.picker-ar.js?Calendar.JS")
               .Include("~/MCS.Framework.Controls/jquery.calendars.ummalqura.js?Calendar.JS")
               .Include("~/MCS.Framework.Controls/jquery.calendars.ummalqura-ar.js?Calendar.JS")
               .Include("~/MCS.Framework.Controls/jquery.calendars-ar.js?Calendar.JS")
               .Include("~/MCS.Framework.Controls/CalendarScript.js?Calendar.JS")
               );

            //
            //OrgStructure
            //
            bundles.Add(new StyleBundle("~/MCS.Framework.Controls/StylesOrgStructure")
                .Include("~/MCS.Framework.Controls/OrgStructure.css?OrgStructure.Styles")
                .Include("~/MCS.Framework.Controls/jquery.contextmenu.css?OrgStructure.Styles")
               );

            bundles.Add(new ScriptBundle("~/MCS.Framework.Controls/JSOrgStructure")
               .Include("~/MCS.Framework.Controls/language.js?OrgStructure.JS")
               .Include("~/MCS.Framework.Controls/jquery.ui.touch-punch.min.js?OrgStructure.JS")
               .Include("~/MCS.Framework.Controls/jquery.contextmenu.js?OrgStructure.JS")
               .Include("~/MCS.Framework.Controls/jsBezier-0.7.js?OrgStructure.JS.jsPlumb.lib")
               .Include("~/MCS.Framework.Controls/mottle-0.6.js?OrgStructure.JS.jsPlumb.lib")
               .Include("~/MCS.Framework.Controls/biltong-0.2.js?OrgStructure.JS.jsPlumb.lib")
               .Include("~/MCS.Framework.Controls/katavorio-0.6.js?OrgStructure.JS.jsPlumb.lib")
               .Include("~/MCS.Framework.Controls/util.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/browser-util.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/jsPlumb.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/dom-adapter.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/overlay-component.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/endpoint.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/connection.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/anchors.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/defaults.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/connectors-bezier.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/connectors-statemachine.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/connectors-flowchart.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/connector-editors.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/renderers-svg.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/renderers-vml.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/base-library-adapter.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/dom.jsPlumb.js?OrgStructure.JS.jsPlumb.src")
               .Include("~/MCS.Framework.Controls/OrgStructureScript.js?OrgStructure.JS")
               .Include("~/MCS.Framework.Controls/jsPlumb-list.js?OrgStructure.JS.jsPlumb")
               );

            //
            //GridView
            //
            bundles.Add(new StyleBundle("~/MCS.Framework.Controls/Mvc/StylesGridView")
               .Include("~/MCS.Framework.Controls/Gridmvc.css?Mvc.GridView.Styles")
               //.Include("~/MCS.Framework.Controls/ResponsiveTable.css?Mvc.GridView.Styles")
               );

            bundles.Add(new ScriptBundle("~/MCS.Framework.Controls/Mvc/JSGridView")
               .Include("~/MCS.Framework.Controls/URI.js?Mvc.GridView.JS")
               .Include("~/MCS.Framework.Controls/gridmvc.js?Mvc.GridView.JS")
               .Include("~/MCS.Framework.Controls/gridmvc-ext.js?Mvc.GridView.JS")
               .Include("~/MCS.Framework.Controls/gridmvc.customwidgets.js?Mvc.GridView.JS")
               .Include("~/MCS.Framework.Controls/gridmvc.customwidgetsText.js?Mvc.GridView.JS")
               .Include("~/MCS.Framework.Controls/gridmvcAR.js?Mvc.GridView.JS")

               );
        }
    }
}
