using System;
using System.Web;
using System.Web.Mvc;
using MCS.Common;

namespace MCS.UI
{
    public static class UrlHelper
    {
        public static Uri GetBaseUri()
        {
            string urlScheme = SystemConfigurations.EnableSSL ? "https" : "http";
            return new Uri(string.Concat(urlScheme, "://",
                HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath));
        }

        public static string GetResetPasswordUrl(ControllerContext context, string area = "")
        {
            if (string.IsNullOrEmpty(area))
            {
                area = context.RouteData.DataTokens["area"] != null ? context.RouteData.DataTokens["area"].ToString() : null;
            }

            string url = SystemConfigurations.ResetPasswordUrl;

            url = url.Replace("{HostName}", HttpContext.Current.Request.ApplicationPath);
            url = url.Replace("{Area}", area);

            return url;
        }
    }
}