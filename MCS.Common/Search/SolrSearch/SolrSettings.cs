using System.Configuration;
using MCS.Framework.Web;

namespace MCS.Common
{
    public class SolrSettings
    {
        public static string SolrUrl 
        { 
            get 
            { 
                string url = ConfigurationManager.AppSettings["SolrUrl"];

                string hostName = HttpContextHelper.GetHeaderValue(Constants.HostName);

                if (string.IsNullOrEmpty(hostName))
                {
                    hostName = HttpContextHelper.HostName;
                }

                return url.Replace("{CoreName}", hostName); 
            } 
        }
    }
}
