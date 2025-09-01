using Microsoft.AspNet.Identity.Owin;
using System.Web;
using MCS.Framework.Security;

namespace MCS.Business.ASPNETIdentity
{
    public class MultiTenantAspNetIdentityProvider: IMemeberShipProvider
    {
        public ICustomSignInManager GetMemeberShipInstance()
        {
            return HttpContext.Current.GetOwinContext().Get<MultiTenantCustomSignInManager>();
        }

        public IApplicationUser GetMemeberShipApplicationUser()
        {
            return new ASPNetIdentityUser();
        }
    }
}
