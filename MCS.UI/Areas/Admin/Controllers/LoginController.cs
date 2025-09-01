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
using MCS.UI.Areas.User.Models.Login;
using MCS.UI.Areas.User.Models.Shared;
using UserMapper = MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Helpers;
using MCS.UI.Areas.User.Models;

namespace MCS.UI.Areas.Admin.Controllers
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

                if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
                {
                    loginInfoVM.UserName = Request.Cookies["UserName"].Value;
                    loginInfoVM.Password = Request.Cookies["Password"].Value;
                    loginInfoVM.RememberMe = true;
                }
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
        [ValidateAntiForgeryToken()]
        public ActionResult Login(LoginInfoVM loginInfoVM, string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                if (!ModelState.IsValid)
                {
                    return View(loginInfoVM);
                }
                string message = string.Empty;
                GetResult<TenantInfo> AdminTenant = null;

                if (SystemConfigurations.MultiTenantEnabled)
                {
                    AdminTenant = HttpClientWrapper<GetResult<TenantInfo>>.GetItemRequest(string.Format("api/MultiTenant/GetTenantInfo?username={0}&cultureName=ar", loginInfoVM.UserName)).Result;
                    if (AdminTenant.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TenantNotFound.ToString());
                        ModelState.AddModelError("Password", message);
                        return View(loginInfoVM);
                    }
                    else
                    {
                        if (AdminTenant.Result == null)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TenantNotFound.ToString());
                            ModelState.AddModelError("Password", message);
                            return View(loginInfoVM);
                        }
                        else
                        {
                            SessionInfo.SetObjectInSession(AdminTenant.Result, Constants.TenantKey);
                        }
                    }
                }
                PostObjectResult<UserDTO> postResult = HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest("api/Login/Login?cultureName=" + SessionInfo.CultureShortName, UserMapper.LoginInfoMapper.Map(loginInfoVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    ModelState.AddModelError("Password", message);
                    return View(loginInfoVM);
                }

                if (postResult.Result == null)
                {
                    message = DbRes.TValidation("User.Login.InvalidCredentials");
                    ModelState.AddModelError("Password", message);
                    return View(loginInfoVM);
                }

                if (SystemConfigurations.MultiTenantEnabled && AdminTenant != null)
                {
                    postResult.Result.TenantLogo = AdminTenant.Result.Logo;
                    postResult.Result.LocalName = AdminTenant.Result.LocalName;
                }

                return ContinueLogin(UserMapper.UserMapper.Map(postResult.Result), returnUrl);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken()]
        public ActionResult WindowsLogin(string returnUrl)
        {
            try
            {
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
        public ActionResult ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController(ControllerContext);

            return resetPasswordController.ResetPassword(resetPasswordVM);
        }

        [HttpPost]
        public ActionResult ResetPasswordTwo(ResetPasswordVM resetPasswordVM)
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController(ControllerContext);

            return resetPasswordController.ResetPasswordTwo(resetPasswordVM);
        }

        [HttpGet]
        public ActionResult WindowsLoginCallback(string returnUrl)
        {
            try
            {
                string message = string.Empty;

                ExternalLoginInfo externalLoginInfo = AuthenticationManager.GetExternalLoginInfo();

                PostObjectResult<UserDTO> postResult = HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest("api/Login/ExternalLogin?cultureName=" + SessionInfo.CultureShortName, externalLoginInfo).Result;

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

                return ContinueLogin(UserMapper.UserMapper.Map(postResult.Result), returnUrl);
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
                if (postResult.MessageSignature != null) 
                {
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

                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.PrintArchiving"), "PrintDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.DownLoadArchiving"), "DownLoadDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.DeleteArchiving"), "DeleteDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.ColorScanning"), "ColorDocumentPermission");
                SessionInfo.SetObjectInSession(SessionInfo.CurrentUser.Claims.Contains("Archiving.Annotations"), "AnnotactionDocumentPermission");

                SessionInfo.SetObjectInSession(postResult.Name, "WatermarkText");
                SessionInfo.SetObjectInSession(postResult.Name, "UserName");
                SessionInfo.SetObjectInSession(SystemConfigurations.EnableSSL ? "https" : "http", "UrlSchema");

                if (postResult.TenantLogo != null)
                {
                    SessionInfo.SetObjectInSession(Convert.ToBase64String(postResult.TenantLogo), "TenantLogo");
                }

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
                    return RedirectToAction("Index", "OrgUnitStructure");
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
                    return RedirectToAction("Index", "OrgUnitStructure");
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