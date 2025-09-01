using System.Web.Mvc;
using System.Web.Routing;

namespace MCS.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {area = "User", controller = "File", action = "MyTransactions", id = UrlParameter.Optional },
                namespaces: new[] { "MCS.UI.Controllers" }
            ).DataTokens.Add("area", "User");
        }
    }
}
