using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MCS.UI.TenantsAdmin.Wrappers
{
    public class BasicAuthenticationHttpClientHandler : HttpClientHandler
    {
        private readonly string _username;
        private readonly string _password;

        public BasicAuthenticationHttpClientHandler(string username, string password)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            _username = username;
            _password = password;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(_username + ":" + _password));

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}