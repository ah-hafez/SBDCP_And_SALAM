using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using MCS.Framework.Security;

namespace MCS.Business.ASPNETIdentity
{
    public class ASPNetIdentityUser : IdentityUser, IApplicationUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ICustomSignInManager userManager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await userManager.GenerateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}
