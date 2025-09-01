using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.Framework.Encryption;
using MCS.Framework.MultiTenants;
using MCS.IntegrationServices.Common;
using MCS.IntegrationServices.Mappers;
using MCS.IntegrationServices.Models;
using MCS.IntegrationServices.UtilityClasses;
using MobileApi.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web.Http;

namespace MCS.IntegrationServices.Controllers
{
    [BasicAuthentication]
    public class AccountController : ApiController
    {
        private string sToken = string.Empty;
        public string Token
        {
            get
            {
                return sToken != string.Empty ? sToken : Request.Headers.Authorization.ToString();
            }
            set
            {
                sToken = value;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login(LoginRequest request)
        {
            try
            {
                DataResult result = new DataResult();
                int userId = -1; string userFullName = string.Empty;
                string userDefaultEntityName = string.Empty;
                int entityId = 0;
                var isUserValid = CheckUserLogin(request.UserName, request.Password, "1245", result, out userId, out userFullName, out userDefaultEntityName, out entityId, "ar");

                if (!isUserValid)
                {
                    return Unauthorized();
                }

                DateTime lastLoginDate = Utilities.FormatDateTimeNow();

                if (string.IsNullOrWhiteSpace(Token))
                {
                    return Unauthorized();
                }
                AuthenticationModule authentication = new AuthenticationModule();
                string token = authentication.GenerateTokenForUser(request.UserName, userId);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return InternalServerError();
            }
        }

        /// <summary>
        /// This method is to logout the logged in user and end the user session  
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The action result</returns>

        private bool CheckUserLogin(string userName, string password, string userDeviceToken, DataResult result, out int userId, out string userFullName, out string userDefaultEntityName, out int entityId, string languageName)
        {
            userId = -1;
            userFullName = string.Empty;
            userDefaultEntityName = string.Empty;
            entityId = 0;
            if (string.IsNullOrEmpty(userName))
            {
                result.Code = MessageCode.InvalidUserNameOrPassword;
                result.Description = MessageResources.GetResourceText(ResourceText.InvalidUserName, languageName);

                return false;
            }
            if (!Utilities.IsAuthenticated(userName, password))
            {
                result.Code = MessageCode.UnauthenticatedUser;
                result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUser, languageName);

                return false;
            }
            else
            {


                LoginInfoDTO loginInfoDTO = new LoginInfoDTO()
                {
                    UserName = userName,
                    Password = AESEncrytDecry.EncryptStringAES(password),
                    IsWindowsLogin = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWindowsLogin"].ToString())
                };

                PostObjectResult<UserDTO> postResult =
                      HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest("api/Login/Login?cultureName=" + SessionInfo.CultureShortName, loginInfoDTO).Result;
                UserVM userVM = UserMapper.Map(postResult.Result);
                userId = userVM.Id;
                SessionInfo.SetObjectInSession(userVM, Constants.LoggedInUserKey);

                //List<string> popUpWindowData = typeof(UserClaims.PopUpWindowData).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => x.GetValue(x).ToString()).ToList();
                //if (SessionInfo.CurrentUser.Claims.Any(c => popUpWindowData.Any(p => p == c)))
                //{
                //    SessionInfo.CurrentUser.Claims.Add(UserClaims.PopUpWindowData.Prefix);
                //}


                Token = postResult?.Result?.AccessToken;
            }

            return true;
        }
        private bool PreRequest(string languageAbbreviation, out IHttpActionResult iHttpActionResult)
        {
            AuthenticationIdentity authenticationIdentity = Thread.CurrentPrincipal.Identity as AuthenticationIdentity;

            if (authenticationIdentity == null || string.IsNullOrEmpty(authenticationIdentity.UserName))
            {
                iHttpActionResult = BadRequest();

                return false;
            }

            DataResult result = new DataResult();
            DateTime lastLoginDate = DateTime.MinValue;

            LoginInfoDTO loginInfoDTO = new LoginInfoDTO()
            {
                UserName = authenticationIdentity.UserName,
                Password = string.Empty
            };



            iHttpActionResult = Ok();

            return true;
        }

        private bool PostRequest(string languageAbbreviation, out IHttpActionResult iHttpActionResult, bool isLogout = false)
        {
            iHttpActionResult = Ok();

            //No need to check the authenticationIdentity nullability, it is already checked in the PrePost
            AuthenticationIdentity authenticationIdentity = Thread.CurrentPrincipal.Identity as AuthenticationIdentity;

            return true;
        }
    }
}
