using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public class MixedAuthOptions : AuthenticationOptions
    {
        public MixedAuthOptions()
            : base("Windows")
        {
            Caption = MixedAuthConstants.DefaultAuthenticationType;
            CallbackPath = new PathString("/MixedAuth");
            ClientId = "MixedAuth";
            AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Passive;
        }

        public string ClientId { get; set; }

        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; set; }
        public CookieAuthenticationOptions CookieOptions { get; set; }
        public PathString CallbackPath { get; set; }
        public string SignInAsAuthenticationType { get; set; }
        public IMixedAuthProvider Provider { get; set; }
    }
}
