using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MCS.Common;

namespace MobileAPIs.Wrappers
{
    public static class HttpClientWrapper<T> where T : class
    {
        private static void SetupClient(HttpClient client, string language, string token, int tenantId = -1)
        {
            client.BaseAddress = new Uri(SystemConfigurations.WebApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            if (SystemConfigurations.MultiTenantEnabled)
            {
                if (tenantId != -1)
                {
                    client.DefaultRequestHeaders.Add(Constants.TenantId, tenantId.ToString());
                }
            }
            client.DefaultRequestHeaders.Add("Language", language);
        }

        public static async Task<T> GetItemRequest(string apiUrl, string language, string token, int TenantId = -1)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client, language, token, TenantId);

                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                result = response.Content.ReadAsAsync<T>().Result;
            }

            return result;
        }

        public static async Task<T> PostRequest(string apiUrl, object postObject, string language, string token = "", int tenantId = -1)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client, language, token, tenantId);

                var response = await client.PostAsync(apiUrl, postObject, new JsonMediaTypeFormatter()).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync()?.ContinueWith((Task<string> t) =>
                {
                    if (t.IsFaulted)
                        throw t.Exception;

                    result = JsonConvert.DeserializeObject<T>(t.Result);

                });
            }

            return result;
        }


        public static async Task<T> PutRequest(string apiUrl,string language, object putObject, string token = "", int tenantId = -1)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client, language, token, tenantId);

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

        private static HttpClientHandler GetHttpClientHandler()
        {
            var httpClientHandler = new HttpClientHandler()
            {
                Credentials = CredentialCache.DefaultNetworkCredentials,
            };
            return httpClientHandler;
        }
    }
}