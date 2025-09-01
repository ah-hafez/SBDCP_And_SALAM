using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public class MixedAuthProvider : IMixedAuthProvider
    {
        public MixedAuthProvider()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);

            OnApplyRedirect = context =>
                context.Response.Redirect(context.RedirectUri);

            OnGetLogonUserIdentity = context =>
            {
                var httpRequest = ((System.Web.HttpContextBase)context.Environment["System.Web.HttpContextBase"]).Request;
                return httpRequest.LogonUserIdentity;
            };
        }

        public Func<MixedAuthAuthenticatedContext, Task> OnAuthenticated { get; set; }
        public Action<MixedAuthApplyRedirectContext> OnApplyRedirect { get; set; }
        public Func<IOwinContext, WindowsIdentity> OnGetLogonUserIdentity { get; set; }

        public virtual Task Authenticated(MixedAuthAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }

        public virtual void ApplyRedirect(MixedAuthApplyRedirectContext context)
        {
            OnApplyRedirect(context);
        }

        public virtual WindowsIdentity GetLogonUserIdentity(IOwinContext context)
        {
            return OnGetLogonUserIdentity(context);
        }
    }
}
