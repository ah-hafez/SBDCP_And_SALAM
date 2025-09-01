using System;
using System.Web.Http.Controllers;
using MCS.Tenants.Service.Controllers.API;

namespace MCS.Tenants.Service.Service.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizationAttribute : System.Web.Http.AuthorizeAttribute
    {
        bool _isAnonymous;
        public AuthorizationAttribute(bool isAnonymous = false)
        {
            _isAnonymous = isAnonymous;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            var controller = actionContext.ControllerContext.Controller as BaseApiController;
            if (!(controller == null || _isAnonymous))
            {
                base.OnAuthorization(actionContext);

                if (controller.CurrentUser == null)
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }
            }
        }
    }
}