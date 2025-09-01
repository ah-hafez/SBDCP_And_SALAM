using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models.Login;
using MCS.UI.Areas.User.Models.Shared;
using System.Globalization;
using MCS.UI.Helpers;
using MCS.UI.Areas.User.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Configuration;
using MCS.UI.Helpers.Extensions;

namespace MCS.UI.Areas.User.Controllers
{
    public class LoginController : BaseController
    {
        #region Inner Classes

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (UserId != null)
                {
                    //TODO: generate key
                    properties.Dictionary["__eMorasalat__UserId__"] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion Inner Classes

        #region Properties

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion Properties

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                LoginInfoVM loginInfoVM = new LoginInfoVM();

                //if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
                //{
                //    loginInfoVM.UserName = Request.Cookies["UserName"].Value;
                //    loginInfoVM.Password = Request.Cookies["Password"].Value;
                //    loginInfoVM.RememberMe = true;
                //}
                ViewBag.ReturnUrl = returnUrl;
                return View(loginInfoVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginInfoVM loginInfoVMs, string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                if (!ModelState.IsValid)
                {
                    return View(loginInfoVMs);
                }
                string message = string.Empty;
                GetResult<TenantInfo> tenantResult = null;
                #region Multi Tenant Enabled
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    //TODO:MH - login wit tenant using username
                    //Get tenant info from seperate API      
                    tenantResult = HttpClientWrapper<GetResult<TenantInfo>>.GetItemRequest(string.Format("api/MultiTenant/GetTenantInfo?username={0}&cultureName=ar", loginInfoVMs.UserName)).Result;
                    if (tenantResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TenantNotFound.ToString());
                        ModelState.AddModelError("Password", message);
                        return View(loginInfoVMs);
                    }
                    else
                    {
                        if (tenantResult.Result == null)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TenantNotFound.ToString());
                            ModelState.AddModelError("Password", message);
                            return View(loginInfoVMs);
                        }
                        else
                        {
                            //set tenant id in sesstion
                            SessionInfo.SetObjectInSession(tenantResult.Result, Constants.TenantKey);
                        }
                    }
                }
                #endregion

                //if (loginInfoVMs.RememberMe)
                //{
                //    Response.Cookies["UserName"].Value = loginInfoVMs.UserName;
                //    Response.Cookies["Password"].Value = loginInfoVMs.Password;

                //    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                //    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                //}
                //else
                //{
                //    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                //    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                //}


                PostObjectResult<UserDTO> postResult =
                    HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest("api/Login/Login?cultureName=" + SessionInfo.CultureShortName, LoginInfoMapper.Map(loginInfoVMs)).Result;

                loginInfoVMs.Password = "";
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    ModelState.AddModelError("Password", message);
                    return View(loginInfoVMs);
                }

                if (postResult.Result == null)
                {
                    message = DbRes.TValidation("User.Login.InvalidCredentials");
                    ModelState.AddModelError("Password", message);
                    return View(loginInfoVMs);
                }
                if (postResult.Result.PendingRegestration == true)
                {
                    message = DbRes.TValidation("الحساب المدخل غير فعال يرجى مراجعة مدير النظام");
                    ModelState.AddModelError("Password", message);
                    return View(loginInfoVMs);
                }


                if (postResult.Result.UserOrgUnits.Count() == 0)
                {
                    message = DbRes.TValidation("User.Login.UserWithNoUnit");
                    ModelState.AddModelError("Password", message);
                    return View(loginInfoVMs);
                }
                if (postResult.StatusCode == StatusCode.Ok)
                {
                    PostObjectResult<UserDTO> Result =
                    HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest("api/Login/LoginUerAction?cultureName=" + SessionInfo.CultureShortName, LoginInfoMapper.Map(loginInfoVMs)).Result;
                                 }
                if (SystemConfigurations.MultiTenantEnabled && tenantResult != null)
                {
                    postResult.Result.TenantLogo = tenantResult.Result.Logo;
                    postResult.Result.LocalName = tenantResult.Result.LocalName;
                }
                GetResult<SettingDTO> Setting = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.DateAndNumbersSettings.DateType)).Result;

                if (Setting.Result != null)
                {
                    if (Setting.Result.Value != null)
                    {
                        SessionInfo.SetObjectInSession(Setting.Result.Value, "DateType");
                    }

                }



                var DateType = SettingMapper.Map(Setting.Result);
                Session["DateType"] = DateType;

                UserVM userVM = UserMapper.Map(postResult.Result);

                string mobile = ADHelper.GetUserPhoneNo(loginInfoVMs.UserName);

                if (!string.IsNullOrEmpty(mobile))
                {
                    userVM.PhoneNumber = mobile;
                }

                return ContinueLogin(userVM, returnUrl);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CheckUserProfile(LoginInfoVM loginInfoVMs)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        Message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.ModelNotValid.ToString()),
                        MessageType = MessageType.Error,
                    });
                }
                string message = string.Empty;
                GetResult<TenantInfo> tenantResult = null;
                #region Multi Tenant Enabled
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    //TODO:MH - login wit tenant using username
                    //Get tenant info from seperate API      
                    tenantResult = HttpClientWrapper<GetResult<TenantInfo>>.GetItemRequest(string.Format("api/MultiTenant/GetTenantInfo?username={0}&cultureName=ar", loginInfoVMs.UserName)).Result;
                    if (tenantResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TenantNotFound.ToString());
                        ModelState.AddModelError("Password", message);
                        return Json(new
                        {
                            Message = message,
                            MessageType = MessageType.Error,
                        });
                    }
                    else
                    {
                        if (tenantResult.Result == null)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TenantNotFound.ToString());
                            ModelState.AddModelError("Password", message);
                            return Json(new
                            {
                                Message = message,
                                MessageType = MessageType.Error,
                            });
                        }
                        else
                        {
                            //set tenant id in sesstion
                            SessionInfo.SetObjectInSession(tenantResult.Result, Constants.TenantKey);
                        }
                    }
                }
                #endregion




                PostObjectResult<UserDTO> postResult =
                    HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest("api/Login/Login?cultureName=" + SessionInfo.CultureShortName, LoginInfoMapper.Map(loginInfoVMs)).Result;
                
                loginInfoVMs.Password = "";
                if (postResult.StatusCode != StatusCode.Ok)
                {
                   
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    ModelState.AddModelError("Password", message);
                    return Json(new
                    {
                        Message = message,
                        MessageType = MessageType.Error,
                    });
                }

                if (postResult.Result == null)
                {
                    message = DbRes.TValidation("User.Login.InvalidCredentials");
                    ModelState.AddModelError("Password", message);
                    return Json(new
                    {
                        Message = message,
                        MessageType = MessageType.Error,
                    });
                }
                if (postResult.Result.PendingRegestration == true)
                {
                    message = DbRes.TValidation("الحساب المدخل غير فعال يرجى مراجعة مدير النظام");
                    ModelState.AddModelError("Password", message);
                    return Json(new
                    {
                        Message = message,
                        MessageType = MessageType.Error,
                    });
                }


                if (postResult.Result.UserOrgUnits.Count() == 0)
                {
                    message = DbRes.TValidation("User.Login.UserWithNoUnit");
                    ModelState.AddModelError("Password", message);
                    return Json(new
                    {
                        Message = message,
                        MessageType = MessageType.Error,
                    });
                }
                if (SystemConfigurations.MultiTenantEnabled && tenantResult != null)
                {
                    postResult.Result.TenantLogo = tenantResult.Result.Logo;
                    postResult.Result.LocalName = tenantResult.Result.LocalName;
                }
                GetResult<SettingDTO> Setting = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.DateAndNumbersSettings.DateType)).Result;

                if (Setting.Result != null)
                {
                    if (Setting.Result.Value != null)
                    {
                        SessionInfo.SetObjectInSession(Setting.Result.Value, "DateType");
                    }

                }
                var DateType = SettingMapper.Map(Setting.Result);
                Session["DateType"] = DateType;

                UserVM userVM = UserMapper.Map(postResult.Result);
               

                string mobile = ADHelper.GetUserPhoneNo(loginInfoVMs.UserName);
                ContinueFillSession(userVM);
                if (!string.IsNullOrEmpty(mobile))
                {
                    userVM.PhoneNumber = mobile;
                }
                if (string.IsNullOrWhiteSpace(userVM.PhoneNumber) || string.IsNullOrWhiteSpace(userVM.InternalNumber))
                {
                    return Json(new
                    {

                        MessageType = MessageType.Warning,
                    });
                }
                else
                {
                    return Json(new
                    {

                        MessageType = MessageType.Information,
                    });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult WindowsLogin(string returnUrl)
        {
            try
            {
                // Request a redirect to the windows login provider
                return new ChallengeResult("Windows", Url.Action("WindowsLoginCallback", "Login", new { ReturnUrl = returnUrl }));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Login/Logout", null).Result;

                if (postResult.StatusCode == StatusCode.Ok)
                {
                    Session.RemoveAll();
                    Session.Abandon();

                    return RedirectToAction("Login");
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult WindowsLoginCallback(string returnUrl)
        {
            try
            {
                string message = string.Empty;

                ExternalLoginInfo externalLoginInfo = AuthenticationManager.GetExternalLoginInfo();

                PostObjectResult<UserDTO> postResult =
                    HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest("api/Login/ExternalLogin?cultureName=" + SessionInfo.CultureShortName, externalLoginInfo).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (postResult.Result == null)
                {
                    message = DbRes.TValidation("User.Login.InvalidCredentials");
                    return Json(new { MessageType = MessageType.Warning, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                return ContinueLogin(UserMapper.Map(postResult.Result), returnUrl);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController(ControllerContext);

            return resetPasswordController.ResetPasswordStepOne();
        }

        [HttpGet]
        public ActionResult ResetPasswordStepTwo(string identityId, string token, string username, string phoneNumber)
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController(ControllerContext);

            return resetPasswordController.ResetPasswordStepTwo(identityId, token, username, phoneNumber);
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordVM resetPasswordVMs)
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController(ControllerContext);

            return resetPasswordController.ResetPassword(resetPasswordVMs);
        }

        [HttpPost]
        public ActionResult ResetPasswordTwo(ResetPasswordVM resetPasswordVMs)
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController(ControllerContext);

            return resetPasswordController.ResetPasswordTwo(resetPasswordVMs);
        }

        [HttpPost]
        public ActionResult TestConsumer_SendOutbound(int transactionNumber)
        {
            try
            {
                PostResult putResult =
                    HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Login/TestConsumer_SendOutbound?transactionNumber={0}&OrgUnitId={1}&cultureName={2}", transactionNumber, SessionInfo.OrgUnitId, SessionInfo.CultureShortName), null).Result;

                return Json(new { MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ActionResult ContinueLogin(UserVM postResult, string returnUrl)
        {
            try
            {
                string message = string.Empty;

                SessionInfo.SetObjectInSession(postResult, Constants.LoggedInUserKey);

                if (postResult.Marking != null)
                {
                    SessionInfo.SetObjectInSession(postResult.Marking, "StampImage");
                }
                if (postResult.SignatureCommand != null)
                {
                    SessionInfo.SetObjectInSession(postResult.SignatureCommand, "SignatureCommandImage");
                    SessionInfo.SetObjectInSession(postResult.SignatureCommand, "BarcodeCommandImage");
                }
                if (postResult.SignatureBehalf != null)
                {
                    SessionInfo.SetObjectInSession(postResult.SignatureBehalf, "SignatureBehalfImage");
                    SessionInfo.SetObjectInSession(postResult.SignatureBehalf, "BarcodeBehalfImage");
                }
                if (postResult.SealSignatureDoc != null)
                {
                    SessionInfo.SetObjectInSession(postResult.SealSignatureDoc, "SealSignatureDocImage");
                    SessionInfo.SetObjectInSession(postResult.SealSignatureDoc, "SealSignatureDocImage");
                }
                if (postResult.MessageSignature != null)
                {
                    SessionInfo.SetObjectInSession(postResult.MessageSignature, "MessageSignatureImage");
                    SessionInfo.SetObjectInSession(postResult.MessageSignature, "MessageSignatureImage");
                }
                if (postResult.Signature != null)
                {
                    SessionInfo.SetObjectInSession(postResult.Signature, "SignatureImage");
                    SessionInfo.SetObjectInSession(postResult.Signature, "BarcodeImage");
                }
                if (postResult.Email != null)
                {
                    SessionInfo.SetObjectInSession(postResult.Email, "UserEmailAddress");
                }

                List<string> popUpWindowData = typeof(UserClaims.PopUpWindowData).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => x.GetValue(x).ToString()).ToList();
                if (SessionInfo.CurrentUser.Claims.Any(c => popUpWindowData.Any(p => p == c)))
                {
                    SessionInfo.CurrentUser.Claims.Add(UserClaims.PopUpWindowData.Prefix);
                }

                SessionInfo.SetObjectInSession(postResult.HasSignaturePasswordText, "HasSignaturePasswordText");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.PrintArchiving"), "PrintDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.DownLoadArchiving"), "DownLoadDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.DeleteArchiving"), "DeleteDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.ColorScanning"), "ColorDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.Annotations"), "AnnotactionDocumentPermission");

                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.WaterMark), "WatermarkPermissions");
                SessionInfo.SetObjectInSession(postResult.Name, "WatermarkText");
                SessionInfo.SetObjectInSession(postResult.Name, "UserName");
                SessionInfo.SetObjectInSession(SystemConfigurations.EnableSSL ? "https" : "http", "UrlSchema");
                SessionInfo.SetObjectInSession(postResult.IsVIPUser, Constants.IsVIPUser);
                SessionInfo.SetObjectInSession(postResult.IsManager, Constants.IsManager);
                if (postResult.TenantLogo != null)
                {
                    SessionInfo.SetObjectInSession(Convert.ToBase64String(postResult.TenantLogo), "TenantLogo");
                }
                var arCulture = ConfigurationManager.AppSettings["DefaultArabicCulture"].ToString();
                var enCulture = ConfigurationManager.AppSettings["DefaultEnglishCulture"].ToString();
                CultureInfo cultureInfo = new CultureInfo(arCulture);
                HttpCookie cookieTemp;

                if (postResult.CultureId == 1)
                    cookieTemp = cultureInfo.SetCookieCulture(arCulture);
                else
                    cookieTemp = cultureInfo.SetCookieCulture(enCulture);
                Response.Cookies.Add(cookieTemp);

                if (postResult.ThemePath != null)
                {
                    SessionInfo.SetObjectInSession(postResult.ThemePath, "ThemePath");
                }

                SessionInfo.SetObjectInSession(postResult.DefaultDisplay, Constants.DefaultDisplay);
                SessionInfo.SetObjectInSession(postResult.DefaultAssignmentPaper, Constants.DefaultAssignmentPaper);

                if (postResult.SMSNotifications != null)
                {
                    SessionInfo.SetObjectInSession(postResult.SMSNotifications, "SMSNotifications");
                    SessionInfo.SetObjectInSession(true, "IsOTPValidated");
                }

                if (Session["SMSNotifications"].ToString().ToLower() == "true")
                {
                    return RedirectToAction("ValidationNumber");
                }
                else if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("MyTransactions", "File");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ContinueFillSession(UserVM postResult)
        {
            try
            {
                string message = string.Empty;

                SessionInfo.SetObjectInSession(postResult, Constants.LoggedInUserKey);

                if (postResult.Marking != null)
                {
                    SessionInfo.SetObjectInSession(postResult.Marking, "StampImage");
                }
                if (postResult.SignatureCommand != null)
                {
                    SessionInfo.SetObjectInSession(postResult.SignatureCommand, "SignatureCommandImage");
                    SessionInfo.SetObjectInSession(postResult.SignatureCommand, "BarcodeCommandImage");
                }
                if (postResult.SignatureBehalf != null)
                {
                    SessionInfo.SetObjectInSession(postResult.SignatureBehalf, "SignatureBehalfImage");
                    SessionInfo.SetObjectInSession(postResult.SignatureBehalf, "BarcodeBehalfImage");
                }
                if (postResult.SealSignatureDoc != null)
                {
                    SessionInfo.SetObjectInSession(postResult.SealSignatureDoc, "SealSignatureDocImage");
                    SessionInfo.SetObjectInSession(postResult.SealSignatureDoc, "SealSignatureDocImage");
                }
                if (postResult.MessageSignature != null)
                {
                    SessionInfo.SetObjectInSession(postResult.MessageSignature, "MessageSignatureImage");
                    SessionInfo.SetObjectInSession(postResult.MessageSignature, "MessageSignatureImage");
                }
                if (postResult.Signature != null)
                {
                    SessionInfo.SetObjectInSession(postResult.Signature, "SignatureImage");
                    SessionInfo.SetObjectInSession(postResult.Signature, "BarcodeImage");
                }
                if (postResult.Email != null)
                {
                    SessionInfo.SetObjectInSession(postResult.Email, "UserEmailAddress");
                }

                List<string> popUpWindowData = typeof(UserClaims.PopUpWindowData).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => x.GetValue(x).ToString()).ToList();
                if (SessionInfo.CurrentUser.Claims.Any(c => popUpWindowData.Any(p => p == c)))
                {
                    SessionInfo.CurrentUser.Claims.Add(UserClaims.PopUpWindowData.Prefix);
                }

                SessionInfo.SetObjectInSession(postResult.HasSignaturePasswordText, "HasSignaturePasswordText");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.PrintArchiving"), "PrintDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.DownLoadArchiving"), "DownLoadDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.DeleteArchiving"), "DeleteDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.ColorScanning"), "ColorDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.Annotations"), "AnnotactionDocumentPermission");

                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.WaterMark), "WatermarkPermissions");
                SessionInfo.SetObjectInSession(postResult.Name, "WatermarkText");
                SessionInfo.SetObjectInSession(postResult.Name, "UserName");
                SessionInfo.SetObjectInSession(SystemConfigurations.EnableSSL ? "https" : "http", "UrlSchema");
                SessionInfo.SetObjectInSession(postResult.IsVIPUser, Constants.IsVIPUser);
                SessionInfo.SetObjectInSession(postResult.IsManager, Constants.IsManager);
                if (postResult.TenantLogo != null)
                {
                    SessionInfo.SetObjectInSession(Convert.ToBase64String(postResult.TenantLogo), "TenantLogo");
                }
                CultureInfo cultureInfo;
                if (postResult.CultureId == 1)
                {
                    cultureInfo = new CultureInfo("ar-JO");
                    Session["Culture"] = "ar-JO";
                }
                else
                {
                    cultureInfo = new CultureInfo("en-JO");
                    Session["Culture"] = "en-JO";
                }
                System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

                if (postResult.ThemePath != null)
                {
                    SessionInfo.SetObjectInSession(postResult.ThemePath, "ThemePath");
                }

                SessionInfo.SetObjectInSession(postResult.DefaultDisplay, Constants.DefaultDisplay);

                if (postResult.SMSNotifications != null)
                {
                    SessionInfo.SetObjectInSession(postResult.SMSNotifications, "SMSNotifications");
                    SessionInfo.SetObjectInSession(true, "IsOTPValidated");
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ValidationNumber(string returnUrl)
        {
            SessionInfo.SetObjectInSession(false, "IsOTPValidated");

            try
            {
                if (!string.IsNullOrEmpty(SessionInfo.CurrentUser.PhoneNumber) && (DateTime.Now > Convert.ToDateTime(Session["OTPTimeout"]) || Session["OTPTimeout"] == null))
                {
                    Session["OTP"] = Request.Url.Host.IndexOf("localhost") > -1 ? SmsHelper.GenerateRandomNo() : SmsHelper.SendOTP(SessionInfo.CurrentUser.PhoneNumber).Result;
                    Session["OTPTimeout"] = DateTime.Now.AddSeconds(25);
                }

                ViewData["OTP"] = Request.Url.Host.IndexOf("localhost") > -1 && Session["OTP"] != null ? Session["OTP"] : "";
            }
            catch (Exception ex)
            {
            }

            return View();
        }

        [HttpPost]
        public ActionResult ValidationNumber(SMSNotificationsModel model, string returnUrl)
        {
            int otp = Convert.ToInt32(Session["OTP"]);

            if (otp.Equals(int.Parse(model.ValidationNumber)))
            {
                SessionInfo.SetObjectInSession(true, "IsOTPValidated");

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("DashboardHome", "Shared");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            else
            {
                string message = DbRes.TValidation("User.Login.VerificationCodeWorng");
                ModelState.AddModelError("ValidationNumber", message);
                return View();
            }
        }
    }
}