using MCS.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.TenantsAdmin
{
    public static class UrlHelper
    {
        public static Uri GetBaseUri()
        {
            return new Uri(string.Concat(HttpContext.Current.Request.Url.Scheme, "://", 
                HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath));
        }
    }
}