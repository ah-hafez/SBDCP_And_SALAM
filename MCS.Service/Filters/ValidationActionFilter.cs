using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace MCS.Service.Filters
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;

            if (!modelState.IsValid)
            {

                var errorList = (from item in modelState
                                 where item.Value.Errors.Any() && item.Value.Errors.Any(a => a.Exception != null)
                                 select item.Value.Errors[0].Exception).ToList();

                if (errorList.Any(error => error.Message == "Current error context error is different to requested error."))
                {
                    actionContext.Response = actionContext.Request
                                     .CreateErrorResponse(HttpStatusCode.InternalServerError, "Forbidden input. The following characters are not allowed: &, <, >, \", '");
                }
                else
                {
                    modelState.ForEach(pair =>
                    {
                        var errors = pair.Value.Errors.Select(error =>
                        error.Exception == null
                        ? new ModelError(error.ErrorMessage)
                        : error.ErrorMessage == null
                            ? new ModelError(error.Exception)
                            : new ModelError(error.Exception, error.ErrorMessage)).ToList();

                        pair.Value.Errors.Clear();
                        foreach (var error in errors)
                        {
                            pair.Value.Errors.Add(error);
                        }
                    });
                    actionContext.Response = actionContext.Request
                                         .CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
                }
            }

        }
    }
}