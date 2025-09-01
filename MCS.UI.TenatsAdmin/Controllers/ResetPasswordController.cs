using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Notifications;
using MCS.Framework.Security;
using MCS.Business;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using MCS.UI.TenantsAdmin.Models.Account;

namespace MCS.UI.TenantsAdmin.Controllers
{
    public class ResetPasswordController : BaseController
    {
        [HttpGet]
        public ActionResult ResetPasswordStepOne()
        {
            try
            {
          
                ResetPasswordVM resetPasswordVM = new ResetPasswordVM();

                return View("~/Views/Shared/ResetPasswordStepOne.cshtml", resetPasswordVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            string message = string.Empty;

            try
            {
                resetPasswordVM.ConfirmPassword = "";
                resetPasswordVM.NewPassword = "";
                resetPasswordVM.Code = "";

                ICustomSignInManager _signInManager = null;
                IApplicationUser user = null;
                IMemeberShipProvider memeberShipProvider = new MCS.Business.ASPNETIdentity.AspNetIdentityProvider();

                _signInManager = memeberShipProvider.GetMemeberShipInstance();

                user = _signInManager.FindByName(resetPasswordVM.UserName);

                if (user == null)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Global.ResetPassword.UserNameNotValid");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                user = _signInManager.FindByEmail(resetPasswordVM.Email);

                if (user == null)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Global.ResetPassword.UserEmailNotValid");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string token = HttpUtility.UrlEncode(_signInManager.GenerateResetPasswordToken(user.Id));

                string varificationCode = _signInManager.GenerateVarificationCode(user.Id, user.PhoneNumber);

                SendResetPasswordNotification(user, token, "ar", SystemConfigurations.ResetPasswordUrl, varificationCode, user.Id);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Global.ResetPasswordStepTwo.EmailSent");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
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
                ResetPasswordVM resetPasswordVM = new ResetPasswordVM();

                resetPasswordVM.IdentityId = identityId;

                resetPasswordVM.Token = token;

                resetPasswordVM.UserName = username;

                resetPasswordVM.PhoneNumber = phoneNumber;

                return View(resetPasswordVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ResetPasswordTwo(ResetPasswordVM resetPasswordVM)
        {
            string message = string.Empty;

            try
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Global.ResetPasswordStepTwo.Succeeded");

               
                ResetPasswordStepTwo(resetPasswordVM);

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SendResetPasswordNotification(IApplicationUser user, string token, string cultureName, string resetPasswordUrl, string varificationCode, string identityId)
        {
            string emailBody = GetResetPasswordTemplete();

            emailBody = emailBody.Replace("{UserName}", user.UserName)
                                 .Replace("{Url}", string.Concat(resetPasswordUrl, string.Format("?identityId={0}&token={1}&username={2}&phoneNumber={3}", identityId, token, user.UserName, user.PhoneNumber)))
                                 .Replace("{Code}", varificationCode);

            SendEmail("Reset Password", emailBody, user.Email, new List<NotificationAttachment>());
        }

        private void SendEmail(string subject, string body, string email, IList<NotificationAttachment> notificationAttachments)
        {
            IEmailNotificationService emailNotificationService = new EmailNotificationService();
            EmailMessage emailMessage = new EmailMessage();

            emailMessage.Subject = subject;
            emailMessage.Body = body;
            emailMessage.To = email;

            IList<System.Net.Mail.Attachment> mailAttachments = null;

            if (notificationAttachments != null && notificationAttachments.Count > 0)
            {
                mailAttachments = new List<System.Net.Mail.Attachment>();

                foreach (NotificationAttachment notificationAttachment in notificationAttachments)
                {
                    System.Net.Mail.Attachment mailAttachment =
                        new System.Net.Mail.Attachment(new MemoryStream(notificationAttachment.Binary), notificationAttachment.FileName);

                    mailAttachments.Add(mailAttachment);
                }
            }

            emailNotificationService.Send(emailMessage);
        }

        private string GetResetPasswordTemplete()
        {
            string passwordTemplete = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                                    "<head> " +
                                    "<title></title>" +
                                    "</head>" +
                                    "<body style='direction:ltr;'>" +
                                    "<div style='text-align:left;'></div>" +
                                    "<br /><br />" +
                                    "<div style='border-top:3px solid #22BCE5'>&nbsp;</div>" +
                                    "<div style=' text-align:left; font-family:Tahoma;font-size:14px'>" +
                                    "Dear Mr <span>{UserName}</span> you have a request to reset the password, to proceed,  <a href='{Url}' >Click Here</a> <br><br> Varification Code : <b>{Code}</b>" +
                                    "</div>" +
                                    "</body>" +
                                    "</html>";

            return passwordTemplete;
        }
        private void ResetPasswordStepTwo(ResetPasswordVM resetPasswordVM)
        {
            ICustomSignInManager _signInManager = null;

            IMemeberShipProvider memeberShipProvider = new MCS.Business.ASPNETIdentity.AspNetIdentityProvider();

            _signInManager = memeberShipProvider.GetMemeberShipInstance();

            IApplicationUser user = _signInManager.GetUser(resetPasswordVM.IdentityId);

            _signInManager.ResetPassword(resetPasswordVM.IdentityId, resetPasswordVM.Token, resetPasswordVM.NewPassword, resetPasswordVM.Code, resetPasswordVM.PhoneNumber);
        }
    }
}
