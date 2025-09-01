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
using MCS.Common;
using MCS.UI.TenantsAdmin.Helpers;
using MCS.UI.TenantsAdmin.Models;

namespace MCS.UI.TenantsAdmin.Wrappers
{
    public static class HttpClientWrapper<T> where T : class
    {
        private static void SetupClient(HttpClient client)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            client.BaseAddress = new Uri(SystemConfigurations.WebApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (SystemConfigurations.MultiTenantEnabled)
            {
                //client.DefaultRequestHeaders.Add(Constants.SubDomainName, SubDomain.GetSubDomain());
                if (SessionInfoHelper.GetObjectFromSession(Constants.TenantKey) != null)
                {
                    client.DefaultRequestHeaders.Add(Constants.TenantId, ((TenantInfo)SessionInfoHelper.GetObjectFromSession(Constants.TenantKey)).Id.ToString());
                    client.DefaultRequestHeaders.Add(Constants.TenantDatabaseName, ((TenantInfo)SessionInfoHelper.GetObjectFromSession(Constants.TenantKey)).DatabaseName);
                    client.DefaultRequestHeaders.Add(Constants.ECMCategoryId, ((TenantInfo)SessionInfoHelper.GetObjectFromSession(Constants.TenantKey)).ECMCategoryId);
                    client.DefaultRequestHeaders.Add(Constants.ECMProfileId, ((TenantInfo)SessionInfoHelper.GetObjectFromSession(Constants.TenantKey)).ECMProfileId);
                }
            }
            if (!string.IsNullOrEmpty(SessionInfoHelper.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionInfoHelper.AccessToken);
            }

            client.DefaultRequestHeaders.Add("Language", SessionInfoHelper.CultureShortName);
        }

        public static async Task<T> GetItemRequest(string apiUrl)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                result = response.Content.ReadAsAsync<T>().Result;
            }

            return result;
        }

        public static async Task<List<T>> GetItemsRequest(string apiUrl)
        {
            List<T> result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                result = response.Content.ReadAsAsync<List<T>>().Result;
            }

            return result;
        }

        public static async Task<List<T>> GetItemsRequest(string apiUrl, T objectParam)
        {
            List<T> result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                result = response.Content.ReadAsAsync<List<T>>().Result;
            }

            return result;
        }

        public static async Task<TokenResponseModel> PostToken(HttpContent content)
        {
            TokenResponseModel result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.PostAsync("Token", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string jsonMessage;

                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        jsonMessage = new StreamReader(responseStream).ReadToEnd();
                    }

                    result = JsonConvert.DeserializeObject<TokenResponseModel>(jsonMessage);
                }
            }

            return result;
        }

        public static async Task<T> PostRequest(string apiUrl, object postObject)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.PostAsync(apiUrl, postObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;

                    result = JsonConvert.DeserializeObject<T>(t.Result);

                });
            }

            return result;
        }

        public static async Task<T> PostAsyncRequest(string apiUrl, object postObject)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.PostAsync(apiUrl, postObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;

                    result = JsonConvert.DeserializeObject<T>(t.Result);

                });
            }

            return result;
        }

        public static async Task<T> PutAsyncRequest(string apiUrl, object putObject)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.PutAsync(apiUrl, putObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;

                    result = JsonConvert.DeserializeObject<T>(t.Result);

                });
            }

            return result;
        }

        public static async Task<T> PutRequest(string apiUrl, object putObject)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.PutAsync(apiUrl, putObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;

                    result = JsonConvert.DeserializeObject<T>(t.Result);

                });
            }

            return result;
        }

        public static async Task<T> DeleteAsyncRequest(string apiUrl)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client);

                var response = await client.DeleteAsync(apiUrl).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;

                    result = JsonConvert.DeserializeObject<T>(t.Result);

                });
            }

            return result;
        }

        public static async Task<T> DeleteRequest(string apiUrl)
        {
            T result = null;

            using (var client = new HttpClient((GetHttpClientHandler())))
            {
                SetupClient(client);

                var response = await client.DeleteAsync(apiUrl).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;

                    result = JsonConvert.DeserializeObject<T>(t.Result);

                });
            }

            return result;
        }

        private static HttpClientHandler GetHttpClientHandler()
        {
            var httpClientHandler = new HttpClientHandler()
            {
                UseCookies = true,
                UseDefaultCredentials = true,
                CookieContainer = new System.Net.CookieContainer()
            };

            Uri target = new Uri(SystemConfigurations.WebApiUrl);

            System.Net.Cookie cookie = new System.Net.Cookie();

            cookie.Name = "ASP.NET_SessionId";
            cookie.Value = SessionInfoHelper.SessionId;
            cookie.Domain = target.Host;

            httpClientHandler.CookieContainer.Add(cookie);

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request != null)
                {
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    cookie = new System.Net.Cookie();

                    cookie.Name = Constants.UserHostIPAddressKey;
                    cookie.Value = ipAddress;
                    cookie.Domain = target.Host;

                    httpClientHandler.CookieContainer.Add(cookie);
                }
            }

            return httpClientHandler;
        }
    }
}