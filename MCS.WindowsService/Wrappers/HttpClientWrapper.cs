using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MCS.Common;


namespace MCS.WindowsService.Wrappers
{    
    public static class HttpClientWrapper<T> where T : class
    {
        private static void SetupClient(HttpClient client, string token, bool isMultiTenant = false, int tenantId = -1, string tenantDatabaseName = "")
        {
            client.BaseAddress = new Uri(isMultiTenant ? SystemConfigurations.TenantsWebApiUrl : SystemConfigurations.WebApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            if (tenantId != -1)
            {
                client.DefaultRequestHeaders.Add("Tenant_Id", tenantId.ToString());
                client.DefaultRequestHeaders.Add("__TenantDatabaseName", tenantDatabaseName);
            }
            client.DefaultRequestHeaders.Add("Language", "ar");
        }

        public static async Task<T> GetItemRequest(string apiUrl, string token, bool isMultiTenant = false, int TenantId = -1, string tenantDatabaseName = "")
        {
            T result = null;

            using (var client = new HttpClient(GetHttpClientHandler()))
            {
                SetupClient(client, token, isMultiTenant, TenantId, tenantDatabaseName);

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
                SetupClient(client, token, isMultiTenant, tenantId, tenantDatabaseName);

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