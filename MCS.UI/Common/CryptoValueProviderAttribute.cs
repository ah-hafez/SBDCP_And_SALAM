using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

namespace YESSER.NCS.MCS.UI.Common
{
    public class CryptoValueProviderAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.Controller.ValueProvider = new CryptoValueProvider(filterContext.RouteData);
        }
    }

    public static class Crypto
    {
        public static string Encrypt(Dictionary<string, string> keyValue)
        {
            return "";
        }

        public static Dictionary<string, string> Decrypt(string encryptedText)
        {
            return new Dictionary<string, string>();
        }
    }

    public class CryptoValueProvider : IValueProvider
    {
        RouteData routeData = null;

        Dictionary<string, string> dictionary = null;

        public CryptoValueProvider(RouteData routeData)
        {
            this.routeData = routeData;
        }

        public bool ContainsPrefix(string prefix)
        {
            if (this.routeData.Values["id"] == null)
            {
                return false;
            }

            this.dictionary = Crypto.Decrypt(this.routeData.Values["id"].ToString());
            return this.dictionary.ContainsKey(prefix.ToUpper());
        }

        public ValueProviderResult GetValue(string key)
        {
            ValueProviderResult result;
            result = new ValueProviderResult(this.dictionary[key.ToUpper()], this.dictionary[key.ToUpper()], CultureInfo.CurrentCulture);
            return result;
        }
    }
}