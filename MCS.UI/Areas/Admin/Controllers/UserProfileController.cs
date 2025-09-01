using System;
using System.Web.Mvc;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using UserProfile = MCS.UI.Areas.User.Models.UserProfile;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class UserProfileController : AdminControllerBase
    {
        // GET: User/UserProfile
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RenderChangePasswordPartial()
        {
            try
            {
                UserProfile.ChangePasswordVM changePasswordVM = new UserProfile.ChangePasswordVM();

                return Json(new { View = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ChangePasswordPartial", changePasswordVM) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ChangePassword(UserProfile.ChangePasswordVM changePasswordVM)
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
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserProfile.Succeeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}