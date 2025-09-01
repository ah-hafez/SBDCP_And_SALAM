using System;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.UserProfile;

namespace MCS.UI.Areas.User.Controllers
{
    public class UserProfileController : BaseController
    {
        // GET: User/UserProfile
        [CustomAuthorize()]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RenderChangePasswordPartial()
        {
            try
            {
                ChangePasswordVM changePasswordVM = new ChangePasswordVM();

                return Json(new { View = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ChangePasswordPartial", changePasswordVM) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            try
            {
                string message = string.Empty;

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/UserProfile/ChangePassword?oldPassword={0}&newPassword={1}", changePasswordVM.OldPassword, changePasswordVM.NewPassword), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserProfile.Succeeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public ActionResult UpdateUserProfile()
        {
            try
            {
                UpdateUserInformationVM updateUserInformationVM = new UpdateUserInformationVM();
                return View("~/Areas/User/Views/UserProfile/_UpdateUserProfile.cshtml");

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateUserInternalNumber(UpdateUserInformationVM updateUserInformationVM)
        {
            try
            {
                string message = string.Empty;

                var userprofileDto = UserProfileMapper.Map(updateUserInformationVM);
                userprofileDto.UserProfileId = SessionInfo.CurrentUser.Id;
                PutResult putResult = HttpClientWrapper<PutResult>.PostRequest("api/UserProfile/UpdateUserInternalNumber", userprofileDto).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserProfile.Succeeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}