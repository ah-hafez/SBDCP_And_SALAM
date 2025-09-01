using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Routing;
using MCS.Domain.NonMappedTypes;
using MCS.Tenants.Service.Helpers;

namespace YESSER.MCS.Tenants.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //Api default response enveloping
            config.MessageHandlers.Add(new ApiEnvelopeWrappingHandler());

            //Reference: http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
            //Disable xml serialization
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new PropertyTypeConverter());

            config.MapHttpAttributeRoutes();

            IHttpRoute defaultRoute = config.Routes.CreateRoute("api/{controller}/{id}",
                                     new { id = RouteParameter.Optional }, null);

            // Add route
            config.Routes.Add("DefaultApi", defaultRoute);

        }
    }
}
