using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MCS.UI.Wrappers
{
    public class ServiceHttpClientHandler : HttpClientHandler
    {
        public ServiceHttpClientHandler()
        {
            Credentials = CredentialCache.DefaultNetworkCredentials;
            UseDefaultCredentials = true;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}