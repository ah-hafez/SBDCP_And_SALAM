using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public interface IApplicationUser
    {
        string Id { get; set; }
        string Email { get; set; }
        string PhoneNumber { get; set; }
        string UserName { get; set; }
        Task<ClaimsIdentity> GenerateUserIdentityAsync(ICustomSignInManager userManager, string authenticationType);
    }
}
