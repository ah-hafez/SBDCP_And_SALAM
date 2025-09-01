using System.Linq;
using System.Web.Routing;

namespace MCS.UI
{
    public static class UrlHelperExt
    {
        public static string EncryptActionRoutes(this System.Web.Mvc.UrlHelper urlHelper, string actionName, string controllerName, object routeValues)
        {
            if (routeValues != null)
            {
                string queryString = "?ER=" + Framework.Encryption.AESEncrytDecry.Base64Encode("true") + "&";
                RouteValueDictionary keyValuePairs = new RouteValueDictionary(routeValues);
                for (int i = 0; i < keyValuePairs.Keys.Count; i++)
                {
                    queryString += keyValuePairs.Keys.ElementAt(i) + "=" + Framework.Encryption.AESEncrytDecry.Base64Encode(keyValuePairs.Values.ElementAt(i).ToString()) + "&";
                }
                return string.Concat("/", urlHelper.Action(actionName, controllerName).Split('/')[1], "/" + urlHelper.Action(actionName, controllerName).Split('/')[2], "/", controllerName, "/", actionName, queryString.TrimEnd('&'));
            }

            return urlHelper.Action(actionName, controllerName);
        }
    }
}