using System.Web;

namespace MCS.Framework.Web
{
    public static class HttpContextHelper
    {
        private static readonly object _transaction = new object();
        public static string HostName
        {
            get
            {
                return HttpContext.Current.Request.ApplicationPath.Substring(1);
            }
        }

        public static string GetHeaderValue(string key)
        {
            lock (_transaction)
            {
                return HttpContext.Current.Request.Headers.Get(key);
            }
        }
    }
}
