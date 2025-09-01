using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using MCS.Framework.MultiTenants;
using MCS.Framework.Web;
using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Common;
using MCS.UI.Models;

namespace MCS.UI
{
    public static class HttpClientWrapper<T> where T : class
    {
        private static HttpClient _Client
        {
            get
            {
                return SessionInfo.GetObjectFromSession("httpClient") as HttpClient;
            }
            set
            {
                SessionInfo.SetObjectInSession(value, "httpClient");
            }
        }

        public static void FlushClient()
        {
            SessionInfo.SetObjectInSession(null, "httpClient");
        }

        private static HttpClient GetClient()
        {
            HttpClient Client;

            if (_Client == null || _Client.DefaultRequestHeaders.Authorization == null)
            {
                Client = new HttpClient(GetHttpClientHandler());
                SetupClient(Client);

                _Client = Client;
            }
            else
            {
                Client = _Client;
            }

            return Client;
        }

        private static void SetupClient(HttpClient client)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            client.BaseAddress = new Uri(SystemConfigurations.WebApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(0, 2, 0);

            if (SystemConfigurations.MultiTenantEnabled)
            {
                //client.DefaultRequestHeaders.Add(Constants.SubDomainName, SubDomain.GetSubDomain());
                if (SessionInfo.GetObjectFromSession(Constants.TenantKey) != null)
                {
                    client.DefaultRequestHeaders.Add(Constants.TenantId, ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id.ToString());
                    client.DefaultRequestHeaders.Add(Constants.TenantDatabaseName, ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).DatabaseName);
                    client.DefaultRequestHeaders.Add(Constants.ECMCategoryId, ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).ECMCategoryId);
                    client.DefaultRequestHeaders.Add(Constants.ECMProfileId, ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).ECMProfileId);
                }
            }

            if (!string.IsNullOrEmpty(SessionInfo.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionInfo.AccessToken);
            }

            client.DefaultRequestHeaders.Add("Language", SessionInfo.CultureShortName);
            client.DefaultRequestHeaders.Add("OrgUnitId", SessionInfo.OrgUnitId.ToString());
        }

        public static async Task<T> GetItemRequest(string apiUrl)
        {
            T result = null;

            var client = GetClient();
            var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            result = response.Content.ReadAsAsync<T>().Result;

            return result;
        }

        public static async Task<List<T>> GetItemsRequest(string apiUrl)
        {
            List<T> result = null;

            var client = GetClient();
            var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            result = response.Content.ReadAsAsync<List<T>>().Result;

            return result;
        }

        public static async Task<List<T>> GetItemsRequest(string apiUrl, T objectParam)
        {
            List<T> result = null;

            var client = GetClient();
            var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            result = response.Content.ReadAsAsync<List<T>>().Result;

            return result;
        }

        public static async Task<T> PostRequest(string apiUrl, object postObject)
        {
            T result = null;

            var client = GetClient();
            var response = await client.PostAsync(apiUrl, postObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
            {
                if (t.IsFaulted)
                    throw t.Exception;

                result = JsonConvert.DeserializeObject<T>(t.Result);

            });

            return result;
        }

        public static async Task<T> PostAsyncRequest(string apiUrl, object postObject)
        {
            T result = null;

            var client = GetClient();
            var response = await client.PostAsync(apiUrl, postObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
            {
                if (t.IsFaulted)
                    throw t.Exception;

                result = JsonConvert.DeserializeObject<T>(t.Result);

            });

            return result;
        }

        public static async Task<T> PutAsyncRequest(string apiUrl, object putObject)
        {
            T result = null;

            var client = GetClient();
            var response = await client.PutAsync(apiUrl, putObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
            {
                if (t.IsFaulted)
                    throw t.Exception;

                result = JsonConvert.DeserializeObject<T>(t.Result);

            });

            return result;
        }

        public static async Task<T> PutRequest(string apiUrl, object putObject)
        {
            T result = null;

            var client = GetClient();
            var response = await client.PutAsync(apiUrl, putObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
            {
                if (t.IsFaulted)
                    throw t.Exception;

                result = JsonConvert.DeserializeObject<T>(t.Result);

            });

            return result;
        }

        public static async Task<T> DeleteAsyncRequest(string apiUrl)
        {
            T result = null;

            var client = GetClient();
            var response = await client.DeleteAsync(apiUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
            {
                if (t.IsFaulted)
                    throw t.Exception;

                result = JsonConvert.DeserializeObject<T>(t.Result);

            });

            return result;
        }

        public static async Task<T> DeleteRequest(string apiUrl)
        {
            T result = null;

            var client = GetClient();
            var response = await client.DeleteAsync(apiUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
            {
                if (t.IsFaulted)
                    throw t.Exception;

                result = JsonConvert.DeserializeObject<T>(t.Result);

            });

            return result;
        }

        private static HttpClientHandler GetHttpClientHandler()
        {
            var httpClientHandler = new HttpClientHandler()
            {
                UseCookies = true,
                UseDefaultCredentials = true,
                CookieContainer = new CookieContainer()
            };

            Uri target = new Uri(SystemConfigurations.WebApiUrl);

            Cookie cookie = new Cookie
            {
                Name = "ASP.NET_SessionId",
                Value = SessionInfo.SessionId,
                Domain = target.Host
            };

            httpClientHandler.CookieContainer.Add(cookie);

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request != null)
                {
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    cookie = new Cookie
                    {
                        Name = Constants.UserHostIPAddressKey,
                        Value = ipAddress,
                        Domain = target.Host
                    };

                    httpClientHandler.CookieContainer.Add(cookie);
                }
            }

            return httpClientHandler;
        }
    }
}