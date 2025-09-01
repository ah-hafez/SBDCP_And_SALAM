using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public class MixedAuthHandler : AuthenticationHandler<MixedAuthOptions>
    {
        public MixedAuthHandler()
        {
        }

        protected async override System.Threading.Tasks.Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationProperties properties = UnpackStateParameter(Request.Query);

            if (properties != null)
            {
                var logonUserIdentity = Options.Provider.GetLogonUserIdentity(Context);

                if (logonUserIdentity.AuthenticationType != Options.CookieOptions.AuthenticationType && logonUserIdentity.IsAuthenticated)
                {
                    AddCookieBackIfExists();

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(logonUserIdentity.Claims, Options.SignInAsAuthenticationType);
                    
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, logonUserIdentity.User.Value, null, Options.AuthenticationType));

                    var ticket = new AuthenticationTicket(claimsIdentity, properties);

                    var context = new MixedAuthAuthenticatedContext(Context, claimsIdentity, 
                        properties, Options.AccessTokenFormat.Protect(ticket));

                    await Options.Provider.Authenticated(context);

                    return ticket;
                }
            }

            return new AuthenticationTicket(null, properties);
        }

        public async override System.Threading.Tasks.Task<bool> InvokeAsync()
        {
            if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path)
            {
                if (!string.IsNullOrEmpty(Request.Query["access_token"]) &&
                    Request.QueryString.Value.IndexOf("token_info") >= 0)
                {
                    try
                    {
                        AuthenticationTicket ticket = UnpackAccessTokenParameter(Request.Query);

                        Newtonsoft.Json.Linq.JObject token = new Newtonsoft.Json.Linq.JObject();
                        var claim = ticket.Identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                        token["user_id"] = claim != null ? claim.Value : "";
                        token["app_id"] = Options.ClientId;
                        Response.StatusCode = 200;
                        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(token));
                    }
                    catch
                    {
                        Newtonsoft.Json.Linq.JObject result = new Newtonsoft.Json.Linq.JObject();
                        result["reason"] = "Invalid access token";
                        Response.StatusCode = 200;
                        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                    }

                    return true;
                }

                var logonUserIdentity = Options.Provider.GetLogonUserIdentity(Context);

                if (logonUserIdentity.AuthenticationType == Options.CookieOptions.AuthenticationType || !logonUserIdentity.IsAuthenticated)
                {
                    Response.StatusCode = MixedAuthConstants.FakeStatusCode;

                    return true;
                }
                else
                {
                    var ticket = await AuthenticateAsync();

                    if (ticket != null)
                    {
                        Context.Authentication.SignIn(ticket.Properties, ticket.Identity);

                        Response.Redirect(ticket.Properties.RedirectUri);

                        return true;
                    }
                }
            }
            else
            {
                AddCookieBackIfExists();
            }

            return false;
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode == MixedAuthConstants.FakeStatusCode)
            {
                return Task.FromResult<object>(null);
            }

            if (Response.StatusCode != 401)
            {
                return Task.FromResult<object>(null);
            }

            AuthenticationResponseChallenge challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

            if (challenge != null)
            {
                var state = challenge.Properties;

                if (String.IsNullOrEmpty(state.RedirectUri))
                    state.RedirectUri = Request.Scheme + Uri.SchemeDelimiter + Request.Host + Request.PathBase + Request.Path + Request.QueryString;

                var logonUserIdentity = Options.Provider.GetLogonUserIdentity(Context);

                if (logonUserIdentity.AuthenticationType == Options.CookieOptions.AuthenticationType || !logonUserIdentity.IsAuthenticated)
                {
                    ReplaceCookie();
                }

                string redirectUri = Request.Scheme +
                    Uri.SchemeDelimiter +
                    Request.Host +
                    RequestPathBase +
                    Options.CallbackPath + "?state=" + Uri.EscapeDataString(Options.StateDataFormat.Protect(state));

                var redirectContext = new MixedAuthApplyRedirectContext(Context, Options, state, redirectUri);

                Options.Provider.ApplyRedirect(redirectContext);
            }

            return Task.FromResult<object>(null);
        }

        private static string GetParameter(IReadableStringCollection query, string key)
        {
            IList<string> values = query.GetValues(key);

            if (values != null && values.Count == 1)
            {
                return values[0];
            }

            return null;
        }

        private AuthenticationProperties UnpackStateParameter(IReadableStringCollection query)
        {
            string state = GetParameter(query, "state");

            if (state != null)
            {
                return Options.StateDataFormat.Unprotect(state);
            }

            return null;
        }

        private AuthenticationTicket UnpackAccessTokenParameter(IReadableStringCollection query)
        {
            string access_token = GetParameter(query, "access_token");

            if (access_token != null)
            {
                return Options.AccessTokenFormat.Unprotect(access_token);
            }

            return null;
        }

        private void AddCookieBackIfExists()
        {
            if (!string.IsNullOrEmpty(Context.Request.Cookies[MixedAuthConstants.TempCookieName]))
            {
                AuthenticationTicket ticket =
                    Options.CookieOptions.TicketDataFormat.Unprotect(Context.Request.Cookies[MixedAuthConstants.TempCookieName]);

                if (ticket != null)
                {
                    Options.CookieOptions.CookieManager.DeleteCookie(Context,
                        MixedAuthConstants.TempCookieName,
                        Options.CookieOptions.ToCookieOptions(DateTime.UtcNow.AddDays(-1)));

                    Options.CookieOptions.CookieManager.AppendResponseCookie(Context,
                        Options.CookieOptions.CookieName,
                        Options.CookieOptions.TicketDataFormat.Protect(ticket),
                        Options.CookieOptions.ToCookieOptions(ticket.Properties.ExpiresUtc.Value.ToUniversalTime().DateTime));
                }
            }
        }

        private void ReplaceCookie()
        {
            string cookieValue = Context.Request.Cookies[Options.CookieOptions.CookieName];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                AuthenticationTicket ticket = Options.CookieOptions.TicketDataFormat.Unprotect(cookieValue);
                if (ticket != null)
                {
                    Options.CookieOptions.CookieManager.DeleteCookie(Context,
                        Options.CookieOptions.CookieName,
                        Options.CookieOptions.ToCookieOptions(DateTime.UtcNow.AddDays(-1)));

                    Options.CookieOptions.CookieManager.AppendResponseCookie(Context,
                        MixedAuthConstants.TempCookieName,
                        Options.CookieOptions.TicketDataFormat.Protect(ticket),
                        Options.CookieOptions.ToCookieOptions(DateTime.UtcNow.AddMinutes(5)));
                }
            }
        }
    }
}
