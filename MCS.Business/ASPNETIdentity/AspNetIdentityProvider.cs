using Microsoft.AspNet.Identity.Owin;
using System.Web;
using MCS.Framework.Security;

namespace MCS.Business.ASPNETIdentity
{
    public class AspNetIdentityProvider : IMemeberShipProvider
    {
        public ICustomSignInManager GetMemeberShipInstance()
        {
            return HttpContext.Current.GetOwinContext().Get<CustomSignInManager>();
        }

        public IApplicationUser GetMemeberShipApplicationUser()
        {
            return new ASPNetIdentityUser();
        }
    }
}
