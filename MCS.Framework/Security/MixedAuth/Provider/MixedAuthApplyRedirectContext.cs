using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public class MixedAuthApplyRedirectContext : BaseContext<MixedAuthOptions>
    {
        public MixedAuthApplyRedirectContext(IOwinContext context, MixedAuthOptions options, 
            AuthenticationProperties properties, string redirectUri) : base(context, options)
        {
            RedirectUri = redirectUri;
            Properties = properties;
        }

        public string RedirectUri { get; private set; }

        public AuthenticationProperties Properties { get; private set; }
    }
}
