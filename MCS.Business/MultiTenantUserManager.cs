using MCS.Framework.Security;
using MCS.Business.ASPNETIdentity;

namespace MCS.Business
{
    public class MultiTenantUserManager
    {
        public static ICustomSignInManager UserManagerProvider
        {
            get
            {
                MultiTenantAspNetIdentityProvider memeberShipProvider = new MultiTenantAspNetIdentityProvider();

                return memeberShipProvider.GetMemeberShipInstance();
            }
        }
    }
}
