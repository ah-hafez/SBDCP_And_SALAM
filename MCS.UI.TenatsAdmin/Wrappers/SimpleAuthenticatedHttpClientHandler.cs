using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
namespace MCS.UI.TenantsAdmin.Wrappers
{
    public class SimpleAuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly string _token;

        public SimpleAuthenticatedHttpClientHandler(string token)
        {
            _token = token ?? throw new ArgumentNullException("token");
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, _token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}