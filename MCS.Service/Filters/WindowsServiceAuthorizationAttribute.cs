using System;
using System.Web.Http.Controllers;
using MCS.Common;
using MCS.Service.Controllers;

namespace MCS.Service.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class WindowsServiceAuthorizationAttribute : System.Web.Http.AuthorizeAttribute
    {
        public WindowsServiceAuthorizationAttribute()
        {

        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            var controller = actionContext.ControllerContext.Controller as ApiBaseController;
            if (!(controller == null || SystemConfigurations.MultiTenantEnabled))
            {
                base.OnAuthorization(actionContext);

                if (controller.CurrentUserIdentity == null)
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }
            }
        }
    }
}