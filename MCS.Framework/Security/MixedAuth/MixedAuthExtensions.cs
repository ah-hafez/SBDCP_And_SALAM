using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MCS.Framework.Web;

namespace MCS.Framework.Security
{
    public static class MixedAuthExtensions
    {
        public static void RegisterMixedAuth(this HttpApplicationBase app)
        {
            app.EndRequest += (object sender, EventArgs e) =>
            {
                if (app.Context.Response.StatusCode == MixedAuthConstants.FakeStatusCode)
                {
                    app.Context.Response.StatusCode = 401;
                    app.Context.Response.SubStatusCode = 2;
                }
            };
        }

        public static CookieOptions ToCookieOptions(this CookieAuthenticationOptions cookieOptions, DateTime expires)
        {
            CookieOptions options = new CookieOptions();

            options.Domain = cookieOptions.CookieDomain;
            options.Expires = expires;
            options.HttpOnly = cookieOptions.CookieHttpOnly;
            options.Path = cookieOptions.CookiePath;
            options.Secure = !options.HttpOnly;

            return options;
        }
    }
}
