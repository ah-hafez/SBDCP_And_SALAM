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
using System.Web.UI;

#region [ Resources ]

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.jSignature.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.SignatureScript.js", "text/javascript")]
//[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.flashcanvas.js", "text/javascript")]
//[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.flashcanvas.min.js", "text/javascript")]
//[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.jSignature.CompressorBase30.js", "text/javascript")]
//[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.jSignature.CompressorSVG.js", "text/javascript")]
//[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.jSignature.SignHere.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.jSignature.UndoButton.js", "text/javascript")]
//[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.eSignature.JS.modernizr.js", "text/javascript")]

#endregion [ Resources ]

namespace MCS.Framework.Controls.Mvc
{
    public static class SignatureExtensions
    {
        public static MvcHtmlString Signature(this HtmlHelper html, string signatureControlid, string hdnIdToSaveBase64Image)
        {
            return MvcHtmlString.Create(Framework.Controls.Signature.RenderSignature(signatureControlid, hdnIdToSaveBase64Image));
        }

        //public static MvcHtmlString GetSignature(this HtmlHelper html, string hiddenFieldIdToStoreSignatureData)
        //{
        //    return MvcHtmlString.Create(Framework.Controls.Signature.GetSignature(hiddenFieldIdToStoreSignatureData));
        //}

        public static MvcHtmlString RenderSignatureResources(this HtmlHelper html)
        {
             return MvcHtmlString.Create(Framework.Controls.Signature.RenderSignatureResources());
        }
    }
}
