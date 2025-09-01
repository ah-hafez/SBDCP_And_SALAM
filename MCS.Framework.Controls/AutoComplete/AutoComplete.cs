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

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.AutoComplete.JS.AutoCompleteScript.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.AutoComplete.Styles.AutoComplete.css", "text/css")]

#endregion [ Resources ]

namespace MCS.Framework.Controls
{
    public static class AutoComplete
    {
        public static string RenderAutoComplete(string autoCompleteControlid, string hdnIdToSaveValue, string items, bool matchAnywhere, string content = "", string inputClassName = "", string ulClassName = "", string buttonId = "", string hdnExtraParametersId = "", bool selectFirstIndex = false, string validationClass = "", string waterMarkText = "",int maxlengthText = 40)
        {
            StringBuilder controlStream = new StringBuilder();

            if (inputClassName != string.Empty)
            {
                controlStream.AppendFormat("<input id='{0}' type='text' maxlength='{3}' class='{1}'  placeholder='{2}' />", autoCompleteControlid, inputClassName, waterMarkText, maxlengthText);
            }
            else
            {
                controlStream.AppendFormat("<input id='{0}' maxlength='{2}' type='text' placeholder='{1}' />", autoCompleteControlid, waterMarkText, maxlengthText);
            }

            controlStream.AppendFormat("<script type='text/javascript'> AutoComplete('{0}','{1}','{2}','{3}','{4}','{5}','{6}', '{7}'); </script>", autoCompleteControlid, hdnIdToSaveValue, items, content, matchAnywhere.ToString().ToLower(), hdnExtraParametersId, selectFirstIndex.ToString().ToLower(), validationClass);

            return controlStream.ToString();
        }

        public static string RenderResetAutoComplete(string autoCompleteControlid)
        {
            StringBuilder controlStream = new StringBuilder();

            return "";
        }

        public static string AutoCompleteChangeList(string autoCompleteControlid, string newList)
        {
            StringBuilder controlStream = new StringBuilder();

            controlStream.AppendFormat("<script type='text/javascript'> AutoCompleteChangeList('{0}','{1}'); </script>", autoCompleteControlid, newList);

            return controlStream.ToString();
        }
        
        public static string RenderAutoCompleteResources()
        {
            StringBuilder controlStream = new StringBuilder();

            string jsAutoCompleteScriptUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.AutoComplete.JS.AutoCompleteScript.js", typeof(AutoComplete));
            string cssAutoCompleteUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.AutoComplete.Styles.AutoComplete.css", typeof(AutoComplete));

            controlStream.AppendFormat("<link rel='stylesheet' href='{0}' />", cssAutoCompleteUrl);
            controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", jsAutoCompleteScriptUrl);

            return controlStream.ToString();

            //StringBuilder controlStream = new StringBuilder();

            ////string jsAutoCompleteScriptUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.AutoComplete.JS.AutoCompleteScript.js", typeof(AutoComplete));
            //string cssAutoCompleteUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.AutoComplete.Styles.AutoComplete.css", typeof(AutoComplete));

            //controlStream.AppendFormat("<link rel='stylesheet' href='{0}' />", cssAutoCompleteUrl);
            ////controlStream.AppendFormat("<script type='text/javascript' src='{0}'></script>", jsAutoCompleteScriptUrl);
            //string js = Scripts.Render("~/MCS.Framework.Controls/JSAutoComplete").ToString();

            ////return controlStream.ToString();
            //return controlStream.ToString() + js;
        }
    }
}