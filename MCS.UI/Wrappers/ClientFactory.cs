using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MCS.UI.Wrappers
{
    public static class ClientFactory
    {
        public static TApiInterface GetClient<TApiInterface>(string baseUrl)
        {
            return RestService.For<TApiInterface>(baseUrl);
        }

        public static TApiInterface GetClient<TApiInterface, TClientHandler>(string baseUrl, Func<TClientHandler> initializr) where TClientHandler : HttpClientHandler
        {
            return RestService.For<TApiInterface>(new HttpClient(initializr()) { BaseAddress = new Uri(baseUrl) });
        }

        public static TApiInterface GetClient<TApiInterface, TClientHandler>(string baseUrl, Dictionary<string, object> data, Func<Dictionary<string, object>, TClientHandler> initializr) where TClientHandler : HttpClientHandlerWithData
        {
            return RestService.For<TApiInterface>(new HttpClient(initializr(data)) { BaseAddress = new Uri(baseUrl) });
        }

        public static TApiInterface GetClient<TApiInterface>(string baseUrl, string username, string password)
        {
            return RestService.For<TApiInterface>(new HttpClient(new BasicAuthenticationHttpClientHandler(username, password)) { BaseAddress = new Uri(baseUrl) });
        }

        public static TApiInterface GetClient<TApiInterface>(string baseUrl, string authorizationToken)
        {
            return RestService.For<TApiInterface>(new HttpClient(new SimpleAuthenticatedHttpClientHandler(authorizationToken)) { BaseAddress = new Uri(baseUrl) });
        }

        public static TApiInterface GetClientWithAuthentication<TApiInterface>(string baseUrl, Func<Task<string>> getToken)
        {
            return RestService.For<TApiInterface>(new HttpClient(new AuthenticatedHttpClientHandler(getToken)) { BaseAddress = new Uri(baseUrl) });
        }
    }
}