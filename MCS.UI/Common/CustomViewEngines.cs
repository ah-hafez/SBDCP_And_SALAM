using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MCS.UI.Common
{
    public class CustomViewEngines
    {
        public class AlternateLocationViewEngine : RazorViewEngine
        {
            public override ViewEngineResult FindPartialView(
                ControllerContext controllerContext,
                string partialViewName,
                bool useCache)
            {
                var altContext = GetAlternateControllerContext(controllerContext);

                if (altContext != null)
                {
                    //see if we can get the view with the alternate controller 
                    //specified, if its found return the result, if its not found
                    //then return the normal results which will try to find 
                    //the view based on the 'real' controllers name.
                    var result = base.FindPartialView(altContext, partialViewName, useCache);

                    if (result.View != null)
                    {
                        return result;
                    }
                }

                return base.FindPartialView(controllerContext, partialViewName, useCache);
            }

            public override ViewEngineResult FindView(
                ControllerContext controllerContext,
                string viewName,
                string masterName,
                bool useCache)
            {
                var altContext = GetAlternateControllerContext(controllerContext);

                if (altContext != null)
                {
                    var result = base.FindView(altContext, viewName, masterName, useCache);
                    if (result.View != null)
                    {
                        return result;
                    }
                }

                return base.FindView(controllerContext, viewName, masterName, useCache);
            }

            /// <summary>
            /// Returns a new controller context with the alternate controller name in the route values
            /// if the current controller is found to contain an AlternateViewEnginePathAttribute.
            /// </summary>
            /// <param name="currentContext"></param>
            /// <returns></returns>
            private static ControllerContext GetAlternateControllerContext(
                ControllerContext currentContext)
            {
                var controller = currentContext.Controller;
                var altControllerAttribute = controller.GetType()
                    .GetCustomAttributes(typeof(AlternateViewEnginePathAttribute), false)
                    .OfType<AlternateViewEnginePathAttribute>()
                    .ToList();
                if (altControllerAttribute.Any())
                {
                    var altController = altControllerAttribute.Single().AlternateControllerName;
                    //we're basically cloning the original route data here...
                    var newRouteData = new RouteData
                    {
                        Route = currentContext.RouteData.Route,
                        RouteHandler = currentContext.RouteData.RouteHandler
                    };
                    currentContext.RouteData.DataTokens.ToList()
                        .ForEach(x => newRouteData.DataTokens.Add(x.Key, x.Value));
                    currentContext.RouteData.Values.ToList()
                        .ForEach(x => newRouteData.Values.Add(x.Key, x.Value));

                    //now, update the new route data with the new alternate controller name
                    newRouteData.Values["controller"] = altController;

                    //now create a new controller context to pass to the view engine
                    var newContext = new ControllerContext(
                        currentContext.HttpContext,
                        newRouteData,
                        currentContext.Controller);
                    return newContext;
                }

                return null;
            }
        }

        /// <summary>
        /// An attribute for a controller that specifies that the ViewEngine 
        /// should look for views for this controller using a different controllers name.
        /// This is useful if you want to share views between specific controllers 
        /// but don't want to have to put all of the views into the Shared folder.
        /// </summary>
        public class AlternateViewEnginePathAttribute : Attribute
        {
            public string AlternateControllerName { get; set; }

            public AlternateViewEnginePathAttribute(string altControllerName)
            {
                AlternateControllerName = altControllerName;
            }
        }
    }
}