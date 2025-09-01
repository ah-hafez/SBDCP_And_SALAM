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

#region [ Resources ]

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.AutoComplete.JS.AutoCompleteScript.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.AutoComplete.Styles.AutoComplete.css", "text/css")]

#endregion [ Resources ]

namespace MCS.Framework.Controls.Mvc
{
    public static class AutoCompleteExtensions
    {
        public static MvcHtmlString AutoComplete(this HtmlHelper html, string autoCompleteControlid, string hdnIdToSaveValue, string items, bool matchAnywhere, string content = "", string inputClassName = "", string ulClassName = "", string buttonId = "", string hdnExtraParametersId = "", bool selectFirstIndex = false, string validationClass = "", string waterMarkText = "", int maxLengthText = 40)
        {
            return MvcHtmlString.Create(Framework.Controls.AutoComplete.RenderAutoComplete(autoCompleteControlid, hdnIdToSaveValue, items, matchAnywhere, content, inputClassName, ulClassName, buttonId, hdnExtraParametersId, selectFirstIndex, validationClass, waterMarkText, maxLengthText));
        }

        public static MvcHtmlString ResetAutoComplete(string autoCompleteControlid)
        {
            return MvcHtmlString.Create(Framework.Controls.AutoComplete.RenderResetAutoComplete(autoCompleteControlid));
        }

        public static MvcHtmlString AutoCompleteChangeList(string autoCompleteControlid, string newList)
        {
            return MvcHtmlString.Create(Framework.Controls.AutoComplete.AutoCompleteChangeList(autoCompleteControlid, newList));
        }

        public static MvcHtmlString RenderAutoCompleteResources(this HtmlHelper html)
        {
            return MvcHtmlString.Create(Framework.Controls.AutoComplete.RenderAutoCompleteResources());
        }
    }
}
