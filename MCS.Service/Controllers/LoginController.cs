using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Encryption;
using MCS.Framework.Exceptions;
using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Business;
using MCS.Business.ASPNETIdentity;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Service.Mappers;
using System.Linq;
using System.Linq.Expressions;


namespace MCS.Service.Controllers
{
    public class LoginController : ApiBaseController
    {
        private CustomSignInManager _signInManager;
        private IAuthenticationManager AuthenticationManager
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public CustomSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<CustomSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        private class TokenData
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("userName")]
            public string Username { get; set; }

            [JsonProperty("userIdentity")]
            public string UserIdentity { get; set; }

            [JsonProperty(".issued")]
            public string IssuedAt { get; set; }

            [JsonProperty(".expires")]
            public string ExpiresAt { get; set; }
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) ||
                    string.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private Tenant HandleMultiTenant()
        {
            string hostName = HttpContextHelper.GetHeaderValue(Common.Constants.HostName);

            Tenant tenant = null;

            //if (!string.IsNullOrEmpty(hostName) && MultiTenantsContext.LoggedInTenant == null)
            //{
            //    ITenantBL tenantBL = new TenantBL();

            //    tenant = tenantBL.GetTenantByHostName(hostName);

            //    TenantInfo tenantInfo = new TenantInfo
            //    {
            //        Id = tenant.Id,
            //        HostName = tenant.HostName,
            //        DatabaseName = tenant.DatabaseName
            //    };

            //    MultiTenantsContext.SetLoggedInTenantInWebSession(tenantInfo);
            //}

            return tenant;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Login(LoginInfoDTO loginInfoDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<UserDTO> postObjectResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IList<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>
            {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", loginInfoDTO.UserName),
                    new KeyValuePair<string, string>("password", loginInfoDTO.Password),
                    new KeyValuePair<string, string>("isWindowsLogin", loginInfoDTO.IsWindowsLogin.ToString())
            };

                    var signInStatus = SignInStatus.Success;
                    TokenData tokenData = null;
                    string userIdentityId = "";
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    UserProfile userProfile = null;
                    //This calculates the logon failure towards account lockout
                    // To enable password failures to trigger account lockout, change to ShouldLockout: true
                    if (SystemConfigurations.ShouldLockout)
                    {
                        if (!loginInfoDTO.IsWindowsLogin)
                        {
                            signInStatus = await SignInManager.PasswordSignInAsync(loginInfoDTO.UserName, loginInfoDTO.Password, loginInfoDTO.RememberMe, SystemConfigurations.ShouldLockout);
                        }
                        else
                        {
                            tokenData = await GetTokenData(requestParams);
                            userIdentityId = tokenData.UserIdentity;

                            if (!string.IsNullOrEmpty(tokenData.UserIdentity))
                                userProfile = userManagementBL.GetUserByIdentity(tokenData.UserIdentity);

                            if (userProfile != null)
                            {
                                signInStatus = SignInStatus.Success;//SignInManager.GetSignInStatus(loginInfoDTO.UserName, loginInfoDTO.Password);
                            }
                            else
                            {
                                signInStatus = await SignInManager.PasswordSignInAsync(loginInfoDTO.UserName, loginInfoDTO.Password, loginInfoDTO.RememberMe, SystemConfigurations.ShouldLockout);
                            }
                        }

                        switch (signInStatus)
                        {
                            case SignInStatus.Success:
                                statusCode = Common.StatusCode.Ok;
                                break;
                            case SignInStatus.LockedOut:
                                statusCode = Common.StatusCode.Lockout;
                                break;
                            case SignInStatus.Failure:
                            default:
                                statusCode = Common.StatusCode.UserNameOrPasswordNotCorrect;
                                break;
                        }

                        if (statusCode != Common.StatusCode.Ok)
                        {
                            postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);
                            return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                        }
                    }

                    tokenData = await GetTokenData(requestParams);

                    userProfile = userManagementBL.GetUserByIdentity(tokenData.UserIdentity, cultureName);

                    if (userProfile == null)
                    {
                        statusCode = Common.StatusCode.UserNameOrPasswordNotCorrect;

                        postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                        return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                    }
                    else if (userProfile.IsActive == false)
                    {
                        statusCode = Common.StatusCode.InActiveUser;

                        postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                        return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                    }

                    UserDTO userDTO = GetUserDTO(userProfile, loginInfoDTO.UserName, tokenData.AccessToken, cultureName);

                    postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, userDTO);

                    return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> LoginUerAction(LoginInfoDTO loginInfoDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<UserDTO> postObjectResult = null;



            try
            {
                using (var transactionContextScope = context.Create())
                {
                    TokenData tokenData = null;
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>
            {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", loginInfoDTO.UserName),
                    new KeyValuePair<string, string>("password", loginInfoDTO.Password),
                    new KeyValuePair<string, string>("isWindowsLogin", loginInfoDTO.IsWindowsLogin.ToString())
            };
                    UserProfile userProfile = null;
                    tokenData = await GetTokenData(requestParams);
                    userProfile = userManagementBL.GetUserByIdentity(tokenData.UserIdentity, cultureName);
                    userManagementBL.UserLoginAction(userProfile.Id);
                    return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                }



            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);



                postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);



                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);



                statusCode = Common.StatusCode.GeneralError;



                postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);



                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> LoginByMobile(LoginInfoDTO loginInfoDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<string> postObjectResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    string passwordSignInMessage = string.Empty;
                    IList<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>
                                    {
                                        new KeyValuePair<string, string>("grant_type", "password"),
                                        new KeyValuePair<string, string>("username", loginInfoDTO.UserName),
                                        new KeyValuePair<string, string>("password", string.Empty),
                                        new KeyValuePair<string, string>("isWindowsLogin", loginInfoDTO.IsWindowsLogin.ToString())
                                    };


                    TokenData tokenData = await GetTokenData(requestParams);

                    postObjectResult = PostObjectResult<string>.Create(statusCode, tokenData.AccessToken);

                    return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postObjectResult = PostObjectResult<string>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postObjectResult = PostObjectResult<string>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> ExternalLogin(ExternalLoginInfo externalLoginInfo, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<UserDTO> postObjectResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (SystemConfigurations.MultiTenantEnabled)
                    {
                        Tenant tenant = HandleMultiTenant();

                        if (tenant == null)
                        {
                            statusCode = Common.StatusCode.TenantNotFound;

                            postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                            return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                        }
                    }

                    if (externalLoginInfo == null)
                    {
                        statusCode = Common.StatusCode.UserNotAuthorised;

                        postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                        return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                    }

                    int startIndex = externalLoginInfo.DefaultUserName.LastIndexOf("\\") + 1;
                    int length = externalLoginInfo.DefaultUserName.Length - startIndex;

                    string userName = externalLoginInfo.DefaultUserName.Substring(startIndex, length);

                    IApplicationUser applicationUser = await UserManager.UserManagerProvider.FindByNameAsync(userName);

                    if (applicationUser == null)
                    {
                        statusCode = Common.StatusCode.UserNotAuthorised;

                        postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                        return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                    }

                    IList<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", externalLoginInfo.DefaultUserName)
                };


                    TokenData tokenData = await GetTokenData(requestParams);

                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    UserProfile userProfile = userManagementBL.GetUserByIdentity(applicationUser.Id, cultureName);

                    if (userProfile == null)
                    {
                        statusCode = Common.StatusCode.UserNotAuthorised;

                        postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                        return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                    }

                    UserDTO userDTO =
                        GetUserDTO(userProfile, externalLoginInfo.DefaultUserName, tokenData.AccessToken, cultureName);

                    postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, userDTO);

                    return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postObjectResult = PostObjectResult<UserDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage Logout()
       {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    userManagementBL.UserLogoutAction(User.Identity.GetUserId());
                }
                if (HttpContext.Current.Session[UserContext.LoggedInUserSessionVariable] != null)
                {
                    HttpContext.Current.Session.RemoveAll();
                    HttpContext.Current.Session.Abandon();
                    HttpContext.Current.Session[UserContext.LoggedInUserSessionVariable] = null;
                }

                AuthenticationManager.SignOut();

                postResult = PostResult.Create(statusCode, null);

                var resp = Request.CreateResponse(HttpStatusCode.OK, postResult);

                return resp;
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage ResetPasswordStepOne(ResetPasswordDTO resetPasswordDTO, string cultureName, string resetPasswordUrl)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        if (SystemConfigurations.MultiTenantEnabled)
                        {
                            Tenant tenant = HandleMultiTenant();

                            if (tenant == null)
                            {
                                statusCode = Common.StatusCode.TenantNotFound;

                                postResult = PostResult.Create(statusCode, "");

                                return Request.CreateResponse(HttpStatusCode.OK, postResult);
                            }
                        }

                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.ResetPasswordStepOne(resetPasswordDTO.UserName, resetPasswordDTO.Email, cultureName, resetPasswordUrl);

                        postResult = PostResult.Create(statusCode, "");

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage ResetPasswordStepTwo(ResetPasswordDTO resetPasswordDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        if (SystemConfigurations.MultiTenantEnabled)
                        {
                            Tenant tenant = HandleMultiTenant();

                            if (tenant == null)
                            {
                                statusCode = Common.StatusCode.TenantNotFound;

                                postResult = PostResult.Create(statusCode, "");

                                return Request.CreateResponse(HttpStatusCode.OK, postResult);
                            }
                        }

                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.ResetPasswordStepTwo(resetPasswordDTO);

                        postResult = PostResult.Create(statusCode, "");

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);

                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        private string GetUserIP()
        {
            string ipAddress = string.Empty;

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request != null)
                {
                    HttpCookie userHostIPCookie = HttpContext.Current.Request.Cookies[Common.Constants.UserHostIPAddressKey];

                    ipAddress = (userHostIPCookie != null) ? userHostIPCookie.Value : string.Empty;
                }
            }

            return ipAddress;
        }

        private UserDTO GetUserDTO(UserProfile userProfile, string userName, string accessToken, string cultureName)
        {
            IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

            string baseOrgUnitName = orgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName);

            var vipUserId = Title.VIPUser.LookupIdentity(LookupCategory.Title, cultureName);
            List<Permission> userPermissions = new List<Permission>();
            var userGroupPermissions = userProfile.UserGroups.Select(g => g.Group.Permissions);

            foreach (var item in userGroupPermissions)
            {
                foreach (var itemPermission in item)
                {
                    userPermissions.Add(itemPermission);
                }
            }

            userPermissions.Distinct();

            User user = new User()
            {
                Id = userProfile.Id,
                IPAddress = GetUserIP(),
                UserName = userName,
                Email = userProfile.Email,
                //Claims = LoginUserMapper.MapClaimsGroup(userProfile.Group.Permissions)
                Claims = LoginUserMapper.MapClaimsGroup(userPermissions)
            };

            UserContext.SetLoggedInUserInWebSession(user);

            UserDTO userDTO = LoginUserMapper.Map(userProfile);

            userDTO.SessionId = HttpContext.Current.Session.SessionID;
            userDTO.AccessToken = accessToken;
            userDTO.BaseOrgUnitName = baseOrgUnitName;
            userDTO.UserName = userName;
            userDTO.IsVIPUser = userProfile.TitleId == vipUserId;

            UserPreferenceInfo userPreferenceInfo = userManagementBL.GetUserPreferenceForLogin(userDTO.Id, cultureName);

            if (userPreferenceInfo != null)
            {
                userDTO.Signature = userPreferenceInfo.Signature;
                userDTO.Marking = userPreferenceInfo.Marking;
                userDTO.Email = userProfile.Email;
                userDTO.PhoneNumber = userProfile.PhoneNumber;
                userDTO.CultureId = userPreferenceInfo.CultureId;
                userDTO.ThemeId = userPreferenceInfo.ThemeId;
                userDTO.ThemePath = userManagementBL.GetThemeByIdForLogin(userPreferenceInfo.ThemeId);
                userDTO.SMSNotifications = userPreferenceInfo.SMSNotifications;
                userDTO.HasSignaturePasswordText = userPreferenceInfo.HasSignaturePasswordText;
                userDTO.DefaultDisplay = userPreferenceInfo.DefaultDisplay;
                userDTO.InternalNumber = userProfile.InternalNumber;
                userDTO.DefaultAssignmentPaper = userPreferenceInfo.DefaultAssignmentPaper;
            }

            return userDTO;
        }

        private async Task<TokenData> GetTokenData(IList<KeyValuePair<string, string>> requestParams)
        {
            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/Token";

            using (var client = new HttpClient())
            {
                //This line to be removed when having a valid SSL certificate
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(requestParams);
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    //formUrlEncodedContent.Headers.Add(Common.Constants.SubDomainName, HttpContextHelper.GetHeaderValue(Common.Constants.SubDomainName));
                    formUrlEncodedContent.Headers.Add(Common.Constants.TenantDatabaseName, TenantHelper.GetTenantDatabaseNameFromHeader());
                    formUrlEncodedContent.Headers.Add(Common.Constants.TenantId, HttpContextHelper.GetHeaderValue(Common.Constants.TenantId));
                }
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, formUrlEncodedContent);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                //var responseCode = tokenServiceResponse.StatusCode;

                return JsonConvert.DeserializeObject<TokenData>(responseString);
            }
        }
    }
}
