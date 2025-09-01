using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Http.Routing;
using MCS.Framework.Logging;
using MCS.IntegrationServices.Models;
using MCS.Common.ApiControllerResults;
using MCS.IntegrationServices.Mappers;

namespace MCS.IntegrationServices.Handler
{
    public class ApiLogHandler : DelegatingHandler
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None
        };

        #region Protected Methods

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            var allowedList = GetAuditController();

            if (request.Content != null && allowedList.Any(x => x.ToLower() == request.RequestUri.Segments[3].ToLower()))
            {
                request.Content.ReadAsByteArrayAsync()
                    .ContinueWith(task =>
                    {
                        apiLogEntry.RequestContentBody = System.Text.UTF8Encoding.UTF8.GetString(task.Result);
                        apiLogEntry.Signature = request.Headers.GetValues("Signature") != null ? ((string[])request.Headers.GetValues("Signature"))[0].ToString() : null;
                    }, cancellationToken);
            }

            // Execute the request, this does not block
            var response = base.SendAsync(request, cancellationToken);

            response.ContinueWith(async (responseMsg) =>
            {
                var responseResult = responseMsg.Result;

                try
                {
                    if (apiLogEntry != null && allowedList.Any(x => x.ToLower() == request.RequestUri.Segments[3].ToLower()))
                    {
                        apiLogEntry.ResponseStatusCode = (int)responseResult.StatusCode;
                        apiLogEntry.ResponseTimestamp = DateTime.Now;

                        if (responseResult.Content != null)
                        {
                            apiLogEntry.ResponseContentBody = await responseResult.Content.ReadAsStringAsync();
                            apiLogEntry.ResponseContentType = responseResult.Content.Headers.ContentType.MediaType;
                            apiLogEntry.ResponseHeaders = SerializeHeaders(responseResult.Content.Headers);
                        }


                        var requestResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/AuditLog/AddApiLog"), AuditLogMapper.Map(apiLogEntry));
                        PostResult postResult = requestResult.Result;

                    }
                }
                catch (Exception ex)
                {
                    // TODO: Log exception here
                }
            });

            return response;
        }

        #endregion

        #region Private Methods

        private ApiAuditLogVM CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();

            var entry = new ApiAuditLogVM
            {
                UserId = SessionInfo.CurrentUser?.Id ?? 0,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };

            return entry;
        }


        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, settings);
        }


        private List<string> GetAuditController()
        {
            var list = new List<string>();
            list.Add("Account/");
            list.Add("Transaction/");
            list.Add("User/");
            return list;
        }
        #endregion
    }



}