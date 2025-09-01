using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Security;
using MCS.Business.ASPNETIdentity;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.DTO.Tenants;
using MCS.UI.TenantsAdmin.Helpers;
using MCS.UI.TenantsAdmin.Models;
using MCS.UI.TenantsAdmin.Models.Account;
using MCS.UI.TenantsAdmin.Wrappers;

namespace MCS.UI.TenantsAdmin.Controllers
{
    public class LoginController : BaseController
    {
        private CustomSignInManager _signInManager;

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

        #endregion Properties

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
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
        public async Task<ActionResult> Login(LoginInfoVM loginInfoVM, string returnUrl)
        {
            try
            {
                if (loginInfoVM.RememberMe)
                {
                    Response.Cookies["UserName"].Value = loginInfoVM.UserName;
                    Response.Cookies["Password"].Value = loginInfoVM.Password;

                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                }

                string message = string.Empty;
                PostObjectResult<ApplicationUserDTO> postResult = HttpClientWrapper<PostObjectResult<ApplicationUserDTO>>
                    .PostRequest("api/authorization/existingUser", loginInfoVM).Result;

                if (postResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (postResult.Result.Id == null)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrEmpty(message))
                {
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                var resultUserLogin = postResult.Result;
                var applicationUserVM = new ApplicationUserVM
                {
                    Email = resultUserLogin.Email,
                    Id = resultUserLogin.Id,
                    UserName = resultUserLogin.UserName,
                    AccessToken = resultUserLogin.AccessToken,
                    Claims = resultUserLogin.Claims,
                    SessionId = resultUserLogin.SessionId
                };
                SessionInfoHelper.SetObjectInSession(applicationUserVM, Constants.LoggedInUserKey);
                return Json(new { MessageType = MessageType.Information, ReturnUrl = returnUrl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                Session.RemoveAll();
                Session.Abandon();

                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController();

            return resetPasswordController.ResetPasswordStepOne();
        }

        [HttpGet]
        public ActionResult ResetPasswordStepTwo(string identityId, string token, string username, string phoneNumber)
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController();

            return resetPasswordController.ResetPasswordStepTwo(identityId, token, username, phoneNumber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController();

            return resetPasswordController.ResetPassword(resetPasswordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ResetPasswordTwo(ResetPasswordVM resetPasswordVM)
        {
            ResetPasswordController resetPasswordController = new ResetPasswordController();

            return resetPasswordController.ResetPasswordTwo(resetPasswordVM);
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

    }
}
