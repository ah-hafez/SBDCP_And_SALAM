using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common;

namespace MCS.UI.Common
{
    public static class UIConfigurations
    {
        public static string GetFullURl()
        {
            var httpValue = SystemConfigurations.EnableSSL ? "https://" : "http://";
            var url = string.Format("{0}{1}{2}", httpValue, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.ApplicationPath);
            return url;
        }
        public static string GetFullLocalURl()
        {
            var httpValue = SystemConfigurations.EnableSSL ? "https://" : "http://";
            var url = string.Format("{0}{1}{2}", httpValue, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.ApplicationPath);
            var appPath = string.Format("{0}", HttpContext.Current.Request.ApplicationPath);

            var urlLocal = "http://localhost" + appPath;
            return urlLocal;
             
        }
    }
}