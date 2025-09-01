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
using System.Web.UI.WebControls;

namespace MCS.Framework.Controls.Mvc
{
    public static class TextEditorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="textControlId"></param>
        /// <param name="readOnly"></param>
        /// <param name="languageShortName"></param>
        /// <param name="plugins"></param>
        /// <param name="toolbar"></param>
        /// <param name="base64Image">name of action in the current controller that use to get stream image (example:'GetImage')</param>
        /// <returns></returns>

        public static MvcHtmlString CustomTextEditor(this HtmlHelper html, string textControlId, string hdnIdToSaveContent, bool readOnly, string languageShortName, string content = "", string javascriptFunName = "",
             string stampBase64Image = "", string signatureBase64Image = "", bool isContentEncoded = true)
        {
            return MvcHtmlString.Create(Framework.Controls.TextEditor.RenderTextEditor(textControlId, hdnIdToSaveContent, readOnly,
                languageShortName, content, javascriptFunName, stampBase64Image, signatureBase64Image, isContentEncoded));
        }

        public static MvcHtmlString RenderTextEditorResources(this HtmlHelper html)
        {
            return MvcHtmlString.Create(Framework.Controls.TextEditor.RenderTextEditorResources());
        }       
    }
}
