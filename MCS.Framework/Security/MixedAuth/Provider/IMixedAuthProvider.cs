using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public interface IMixedAuthProvider
    {
        WindowsIdentity GetLogonUserIdentity(IOwinContext context);
        void ApplyRedirect(MixedAuthApplyRedirectContext context);
        Task Authenticated(MixedAuthAuthenticatedContext context);
    }
}
