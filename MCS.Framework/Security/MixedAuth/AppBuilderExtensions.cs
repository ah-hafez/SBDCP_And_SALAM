using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Extensions;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseMixedAuth(this IAppBuilder app, MixedAuthOptions options, 
            CookieAuthenticationOptions cookieOptions)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (options == null)
                throw new ArgumentNullException("options");
            if (cookieOptions == null)
                throw new ArgumentNullException("cookieOptions");

            options.CookieOptions = cookieOptions;

            app.Use(typeof(MixedAuthMiddleware), app, options);

            app.UseStageMarker(PipelineStage.PostAuthenticate);

            return app;
        }

        public static IAppBuilder UseMixedAuth(this IAppBuilder app, CookieAuthenticationOptions cookieOptions)
        {
            return app.UseMixedAuth(new MixedAuthOptions(), cookieOptions);
        }
    }
}
