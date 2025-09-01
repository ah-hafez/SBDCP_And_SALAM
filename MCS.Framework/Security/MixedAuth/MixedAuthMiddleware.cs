using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public class MixedAuthMiddleware : AuthenticationMiddleware<MixedAuthOptions>
    {
        public MixedAuthMiddleware(OwinMiddleware next, IAppBuilder app, MixedAuthOptions options) : base(next, options)
        {
            if (String.IsNullOrEmpty(Options.SignInAsAuthenticationType))
            {
                Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
            }
          
            if (Options.Provider == null)
            {
                Options.Provider = new MixedAuthProvider();
            }

            if (Options.StateDataFormat == null)
            {
                IDataProtector dataProtecter = app.CreateDataProtector(
                    typeof(MixedAuthMiddleware).Namespace,
                    Options.AuthenticationType,
                    "v1"
                );

                Options.StateDataFormat = new PropertiesDataFormat(dataProtecter);
            }

            if (Options.AccessTokenFormat == null)
            {
                IDataProtector dataProtecter = app.CreateDataProtector(
                    typeof(OAuthAuthorizationServerMiddleware).Namespace,
                    "Access_Token",
                    "v1"
                );
                Options.AccessTokenFormat = new TicketDataFormat(dataProtecter);
            }
        }

        protected override AuthenticationHandler<MixedAuthOptions> CreateHandler()
        {
            return new MixedAuthHandler();
        }
    }
}
