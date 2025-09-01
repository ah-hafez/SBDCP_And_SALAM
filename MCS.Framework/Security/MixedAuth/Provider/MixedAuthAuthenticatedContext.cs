using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public class MixedAuthAuthenticatedContext : BaseContext
    {
        public MixedAuthAuthenticatedContext(IOwinContext context, ClaimsIdentity identity,
            AuthenticationProperties properties, string accessToken)
            : base(context)
        {
            this.Identity = identity;
            this.Properties = properties;
            this.AccessToken = accessToken;
        }

        public string AccessToken { get; private set; }
        public ClaimsIdentity Identity { get; set; }
        public AuthenticationProperties Properties { get; set; }
    }
}
