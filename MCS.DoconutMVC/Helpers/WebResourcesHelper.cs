using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MCS.DoconutMVC.Helpers
{
    public static class WebResourcesHelper
    {

        public static string GlobalResource(this HtmlHelper html, string resourceText)
        {
            var localResourceObject = HttpContext.GetGlobalResourceObject("ResourceView", resourceText);
            return localResourceObject != null ? localResourceObject.ToString() : string.Empty;
        }
        private static Page Page
        {
            get
            {
                if (_page == null)
                    _page = new Page();
                return _page;
            }
        }
        [ThreadStatic]
        private static Page _page;

        public static string GetWebResourceUrl(this HtmlHelper htmlHelper, string resource)
        {
            return GetWebResourceUrl(resource);
        }

        public static string GetWebResourceUrl(string resource)
        {
            return Page.ClientScript.GetWebResourceUrl(typeof(DoconutViewer.Controllers.DocoNutController), resource);
        }

        
    }
}