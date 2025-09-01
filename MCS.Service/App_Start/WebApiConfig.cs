using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using MCS.DTO;

namespace MCS.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));


            // first clear all formatters
            config.Formatters.Clear();
            // Since JSON is the first added, 
            // it will be the "default" if no Accept header present
            config.Formatters.Add(new JsonMediaTypeFormatter());
            // You can choose not to add these back if you never want to use them, 
            // up to you.     
            config.Formatters.Add(new XmlMediaTypeFormatter());
            config.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());

            //Register custom TransactionDTOBinder binder
            var provider = new SimpleModelBinderProvider(typeof(TransactionDTOBinder), new TransactionDTOBinder());
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);
            
            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{Action}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
