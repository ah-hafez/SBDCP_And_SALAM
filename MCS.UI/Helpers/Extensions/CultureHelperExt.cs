using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MCS.UI.Helpers.Extensions
{
    public static class CultureHelperExt
    {
        public static HttpCookie SetCookieCulture(this CultureInfo cultureInfo, string cultureName)
        {
            cultureInfo = new CultureInfo(cultureName);
            HttpCookie cookieTemp = new HttpCookie("Culture");
            cookieTemp.Value = cultureName;
            cookieTemp.Expires = DateTime.Now.AddMonths(1);
            cookieTemp.Secure = false;
            cookieTemp.Shareable = false;
            cookieTemp.Domain = ConfigurationManager.AppSettings["ServerName"].ToString();
            return cookieTemp;

        }
    }
}