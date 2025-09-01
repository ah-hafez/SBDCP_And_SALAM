using MCS.Business.ASPNETIdentity;
using MCS.Common;
using MCS.Domain;
using MCS.Framework.Exceptions;
using MCS.Framework.Notifications;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Business
{
    public static class TransactionEncryptionBL
    {
        public static void SendHashCodeByEmail(int TransactionNumber, string HashedCode, int UserId, string cultureName)
        {

            
                var notificationUsers = new List<NotificationUser>
                {
                    new NotificationUser { UserId = UserId}
                };

                Dictionary<string, string> keyValues = new Dictionary<string, string>();


                keyValues["{TransactionNumber}"] = TransactionNumber.ToString();
                keyValues["{Code}"] = HashedCode;

                var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().UserId) };
                NotificationsManager.EmailNotification(NotificationSource.VerificationTransactionCodeEmail, NotificationTemplateType.VerificationTransactionCodeEmail,
                    NotificationEmailSubject.VerificationTransactionCodeEmail, notificationUsersEmail, cultureName, null, keyValues);

         
        }

        public static void SendHashCodeBySMS(int TransactionNumber, string mobileNumber, string HashedCode, int UserId, string cultureName)
        {
            try
            {
                ISMSNotificationService smsNotificationService = new SMSNotificationService();
                SMSMessage smsMessage = new SMSMessage();

                smsMessage.ToNumber = mobileNumber;
                smsMessage.Body = HashedCode;

                smsNotificationService.Send(smsMessage);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }
    }
}
