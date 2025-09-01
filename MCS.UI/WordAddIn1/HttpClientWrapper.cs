using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WordAddIn1
{
    public static class HttpClientWrapper<T> where T : class
    {

        public   static string urlLink = ConfigurationManager.AppSettings["WebApiUrl"];

        private static void SetupClient(HttpClient client, string token )
        {
            client.BaseAddress = new Uri(urlLink);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
           
            client.DefaultRequestHeaders.Add("Language", "ar");
        }

        public static async Task<T> GetItemRequest(string apiUrl, string token)
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client, token);

                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                result = response.Content.ReadAsAsync<T>().Result;
            }

            return result;
        }

        public static async Task<T> PostRequest(string apiUrl, object postObject, bool isMultiTenant = false, int tenantId = -1, string token = "", string tenantDatabaseName = "")
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client, token);

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
