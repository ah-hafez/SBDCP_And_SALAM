using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using MCS.Framework.Security;
using MCS.Business.ASPNETIdentity;

namespace MCS.Tenants.Service.Controllers.API
{

    public class BaseApiController : ApiController
    {
        public ICustomSignInManager _signInManager = null;
        public IApplicationUser user = null;
        public IMemeberShipProvider memeberShipProvider = new MultiTenantAspNetIdentityProvider();
        public string CurrentUserIdentity => User.Identity.GetUserId();
        public IApplicationUser CurrentUser
        {
            get
            {
                //ToDo: this data need to be in cache
                _signInManager = memeberShipProvider.GetMemeberShipInstance();
                var user = _signInManager.GetUser(CurrentUserIdentity);
                return user;
            }
        }
        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(CurrentUserIdentity))
            {
                var user = CurrentUser; 
            }
            return base.ExecuteAsync(controllerContext, cancellationToken);
        }
        #region Helpers
        protected T GetFirstHeaderValueOrDefault<T>(string headerKey, Func<HttpRequestMessage, string> defaultValue, Func<string, T> valueTransform)
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            if (!message.Headers.TryGetValues(headerKey, out headerValues))
                return valueTransform(defaultValue(message));
            string firstHeaderValue = headerValues.FirstOrDefault() ?? defaultValue(message);
            return valueTransform(firstHeaderValue);
        }
        #endregion
    }
}
