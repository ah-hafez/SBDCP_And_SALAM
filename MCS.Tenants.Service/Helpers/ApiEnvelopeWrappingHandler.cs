using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MCS.Tenants.Service.Results;

namespace MCS.Tenants.Service.Helpers
{
    public class ApiEnvelopeWrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            return await BuildApiResponseAsync(request, response);

        }

        private static async Task<HttpResponseMessage> BuildApiResponseAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            object resonseContent = null;
            Dictionary<string, object> errorMessages = null;
            Dictionary<string, object> modelErrorMessages = null;
            string errorMessage = null;
            if (response.TryGetContentValue(out resonseContent) && !response.IsSuccessStatusCode)
            {
                HttpError error = resonseContent as HttpError;
                if (error != null)
                {
                    //clear original content since we will insert it into a new variable now
                    resonseContent = null;
                    errorMessage = error.Message;
                    if (error.ModelState != null && error.ModelState.Any())
                    {
                        modelErrorMessages = error.ModelState;
                    }
                    else if (!AppSettings.IsProduction)
                    {
                        //Show further detailed Exception and StackTrace
                        errorMessage += error.InnerException != null ? $"\n\r {error.GetDeepestInnerException()}" : null;
                        if (error.Count > 1) errorMessages = error;
                    }
                }
                else if ((400 <= (int)response.StatusCode && (int)response.StatusCode <= 499) || (500 <= (int)response.StatusCode && (int)response.StatusCode <= 599))
                {
                    errorMessage = await response.Content.ReadAsStringAsync();
                    resonseContent = null;
                }
            }
            var newResponse = request.CreateResponse(response.StatusCode, new JsonResponseResult<object>(resonseContent, errorMessage, errorMessages, modelErrorMessages, (int)response.StatusCode));
            response.Content = newResponse.Content;
            return response;
        }
    }
}