using System;
using System.Web.Mvc;
using MCS.Framework.Encryption;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using UserLogin = MCS.UI.Areas.User.Models.Shared;
using UserMapper = MCS.UI.Areas.User.Mappers.Shared;
namespace MCS.UI
{
    public class ResetPasswordController : BaseController
    {
        readonly ControllerContext _context = null;
        public ResetPasswordController(ControllerContext context)
        {
            if (context != null)
                _context = context;
            else
                _context = ControllerContext;
        }

        [HttpGet]
        public ActionResult ResetPasswordStepOne()
        {
            try
            {
                ResetPasswordDTO resetPasswordDTO = new ResetPasswordDTO();

                return View("~/Views/Shared/ResetPasswordStepOne.cshtml", resetPasswordDTO);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ResetPassword(UserLogin.ResetPasswordVM resetPasswordVM)
        {
            try
            {
                string message = string.Empty;

                resetPasswordVM.ConfirmPassword = AESEncrytDecry.EncryptData(resetPasswordVM.ConfirmPassword);
                resetPasswordVM.NewPassword = AESEncrytDecry.EncryptData(resetPasswordVM.NewPassword);
                resetPasswordVM.Code = string.Empty;

                PostObjectResult<string> postResult = HttpClientWrapper<PostObjectResult<string>>.PostRequest(string.Format("api/Login/ResetPasswordStepOne?cultureName={0}&resetPasswordUrl={1}", SessionInfo.CultureShortName, UrlHelper.GetResetPasswordUrl(_context)), UserMapper.ResetPasswordMapper.Map(resetPasswordVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Global.ResetPasswordStepTwo.EmailSent");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ResetPasswordStepTwo(string identityId, string token, string username, string phoneNumber)
        {
            try
            {
                ResetPasswordDTO resetPasswordDTO = new ResetPasswordDTO();

                resetPasswordDTO.IdentityId = identityId;

                resetPasswordDTO.Token = token;  

                resetPasswordDTO.UserName = username;

                resetPasswordDTO.PhoneNumber = phoneNumber;

                return View(resetPasswordDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ResetPasswordTwo(UserLogin.ResetPasswordVM resetPasswordVM)
        {
            try
            {
                string message = string.Empty;
                resetPasswordVM.ConfirmPassword = AESEncrytDecry.EncryptData(resetPasswordVM.ConfirmPassword);
                resetPasswordVM.NewPassword = AESEncrytDecry.EncryptData(resetPasswordVM.NewPassword);

                PostObjectResult<string> postResult = HttpClientWrapper<PostObjectResult<string>>.PostRequest("api/Login/ResetPasswordStepTwo", UserMapper.ResetPasswordMapper.Map(resetPasswordVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Global.ResetPasswordStepTwo.Succeeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}