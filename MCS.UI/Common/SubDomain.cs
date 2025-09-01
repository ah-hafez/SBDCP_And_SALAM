using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Common
{
    public static class SubDomain
    {
        public static string GetSubDomain()
        {
            var url = HttpContext.Current.Request.Url;
            string result = string.Empty;
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                string[] subDomains = host.Split('.');
                if (subDomains.Length > 1)
                {
                    result = subDomains[0] + "." + subDomains[1];
                }
                else
                {
                    result = subDomains[0];
                }
            }
            return result;
        }
    }
}