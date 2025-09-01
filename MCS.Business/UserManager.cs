using MCS.Framework.Security;

namespace MCS.Business
{
    public class UserManager
    {
        public static ICustomSignInManager UserManagerProvider
        {
            get
            {
                IMemeberShipProvider memeberShipProvider = new MCS.Business.ASPNETIdentity.AspNetIdentityProvider();

                return memeberShipProvider.GetMemeberShipInstance();
            }
        }
    }
}
