using System.Configuration;

namespace MCS.Tenants.Service
{
    public static class AppSettings
    {
        public static bool IsProduction
        {
            get
            {
                bool result = false;
                bool.TryParse(ConfigurationManager.AppSettings["IsProduction"], out result);
                return result;
            }
        }
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
    public static class ConnectionStrings
    {
        public static string Get(string key)
        {
            return ConfigurationManager.ConnectionStrings[key]?.ConnectionString;
        }
    }
}