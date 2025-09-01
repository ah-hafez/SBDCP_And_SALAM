using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Exceptions;
using MCS.Framework.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MCS.Framework.Logging;

namespace  MCS.UI.TenantsAdmin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GridHelper.ResetPageSize();

            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;

            TempData["Exception"] = exception;

            Logger.WriteException(exception);

            filterContext.ExceptionHandled = true;

            HandleErrorInfo handleErrorInfo =
                new HandleErrorInfo(exception, filterContext.RouteData.Values["controller"].ToString(),
                    filterContext.RouteData.Values["action"].ToString());

            string actionName = "Error";
            string controllerName = "Error";

            if (!Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    Action = actionName,
                    Controller = controllerName,
                    Area = ""
                }));
            }
            else
            {
                bool errorOccurred = true;
                string url =  MCS.UI.TenantsAdmin.UrlHelper.GetBaseUri() + "/Error/Error";

                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    Data = new
                    {
                        errorOccurred,
                        url
                    }
                };
            }

            ExceptionHelper.HandleException(exception);

            base.OnException(filterContext);
        }
    }
}