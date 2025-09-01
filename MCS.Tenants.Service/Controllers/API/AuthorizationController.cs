using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Business;
using MCS.Business.ASPNETIdentity;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Common.Utility;
using MCS.DTO;
using MCS.Tenants.Service.Models;
using MCS.Tenants.Service.Service.Filters;

namespace MCS.Tenants.Service.Controllers.API
{
    [RoutePrefix("api/authorization")]
    public class AuthorizationController : BaseApiController
    {
        [Authorization]
        [HttpGet]
        [Route("validateUser")]
        public HttpResponseMessage ValidateUser()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        //Seconde Solution
        [HttpPost]
        [Route("existingUser")]
        public async Task<HttpResponseMessage> ExistingApplicationUser(LoginInfoDTO model)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<ApplicationUserDTO> postResult = null;
            ApplicationUserDTO applicationUserDTO = new ApplicationUserDTO();
            ICustomSignInManager _signInManager = null;
            IMemeberShipProvider memeberShipProvider = new MultiTenantAspNetIdentityProvider();
            _signInManager = memeberShipProvider.GetMemeberShipInstance();
            //check if user name already exist
            try
            {
                IApplicationUser existingApplicationUser = _signInManager.Find(model.UserName, model.Password);

                if (existingApplicationUser != null)
                {
                    TokenData tokenData = await GetTokenData(new List<KeyValuePair<string, string>>
                                    {
                                        new KeyValuePair<string, string>("grant_type", "password"),
                                        new KeyValuePair<string, string>("username", model.UserName),
                                        new KeyValuePair<string, string>("password", model.Password)
                                    });


                    applicationUserDTO.Id = existingApplicationUser.Id;
                    applicationUserDTO.PhoneNumber = existingApplicationUser.PhoneNumber;
                    applicationUserDTO.UserName = existingApplicationUser.UserName;
                    applicationUserDTO.Email = existingApplicationUser.Email;
                    applicationUserDTO.AccessToken = tokenData.AccessToken;
                }

                postResult = PostObjectResult<ApplicationUserDTO>.Create(statusCode, applicationUserDTO);
                return Request.CreateResponse(HttpStatusCode.OK, postResult.Result);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                postResult = PostObjectResult<ApplicationUserDTO>.Create(statusCode, null);
                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                postResult = PostObjectResult<ApplicationUserDTO>.Create(statusCode, null);
                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }
        private async Task<TokenData> GetTokenData(IList<KeyValuePair<string, string>> requestParams)
        {
            var request = HttpContext.Current.Request;
            var tokenServiceUrl = AppSettings.Get("tokenUrl");
            using (var client = new HttpClient())
            {
                try
                {
                    //This line to be removed when having a valid SSL certificate
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(requestParams);
                    formUrlEncodedContent.Headers.Add(Constants.HostName, HttpContextHelper.GetHeaderValue(Constants.HostName));
                    var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, formUrlEncodedContent);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TokenData>(responseString);
                }
                catch (Exception ex)
                {
                    Debug.WriteToFile("TenantService", ex.Message);
                    return JsonConvert.DeserializeObject<TokenData>("");
                }

            }
        }
    }
}