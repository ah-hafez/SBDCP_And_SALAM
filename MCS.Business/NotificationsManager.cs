using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Framework;

namespace MCS.Business
{
    public class NotificationsManager
    {
        public static void SystemNotification(NotificationSource notificationSource, NotificationTemplateType notificationTemplateType, NotificationWebSubject notificationWebSubject,
            IList<NotificationUser> notificationUsers, string cultureName, Dictionary<string, string> labels)
        {
            try
            {
                INotificationBL notificationBL = new NotificationBL();

                IList<NotificationDetail> notificationDetails = new List<NotificationDetail>();
                NotificationDetail notificationDetail = BuildNotificationDetail(NotificationType.Web, notificationTemplateType, notificationWebSubject, null, cultureName);
                notificationDetail.Link = BuildNotificationLink(notificationTemplateType, labels);
                labels["{RedirectTransactionURl}"] = notificationDetail.Link;
                
               notificationDetails.Add(notificationDetail);

                Notification notification = new Notification()
                {
                    SourceId = notificationSource.LookupIdentity(LookupCategory.NotificationSource, cultureName),
                    Date = DateTime.Now,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                    Details = notificationDetails,
                    Users = notificationUsers
                };

                notificationBL.SendNotification(notification, labels);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public static void EmailNotification(NotificationSource notificationSource, NotificationTemplateType notificationTemplateType, NotificationEmailSubject? notificationEmailSubject,
            IList<NotificationUser> notificationUsers, string cultureName, IList<NotificationAttachment> attachments, Dictionary<string, string> labels)
        {
            try
            {
                IList<NotificationDetail> notificationDetails = new List<NotificationDetail>();

                NotificationDetail notificationDetail = BuildNotificationDetail(NotificationType.Email, notificationTemplateType, null, notificationEmailSubject, cultureName);

                notificationDetail.Attachments = attachments;
                notificationDetail.Email = notificationUsers.FirstOrDefault().User.Email;

                notificationDetails.Add(notificationDetail);

                INotificationBL notificationBL = IoC.Resolve<INotificationBL>();

                Notification tenantNotification = new Notification()
                {
                    SourceId = notificationSource.LookupIdentity(LookupCategory.NotificationSource, cultureName),
                    Date = DateTime.Now,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                    Details = notificationDetails,
                    Users = notificationUsers
                };
                notificationBL.SendNotification(tenantNotification, labels);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public static NotificationDetail BuildNotificationDetail(NotificationType notificationType, NotificationTemplateType? notificationTemplateType,
            NotificationWebSubject? notificationWebSubject, NotificationEmailSubject? notificationEmailSubject, string cultureName)
        {
            try
            {
                INotificationBL notificationBL = new NotificationBL();
                ILookupBL lookupBL = new LookupBL();
                Lookup subject = null;
                NotificationDetail notificationDetail = new NotificationDetail()
                {
                    NotificationType = lookupBL.GetLookupItem(notificationType.LookupIdentity(LookupCategory.NotificationType, cultureName))
                };

                if (notificationTemplateType.HasValue)
                {
                    notificationDetail.NotificationTemplateType = lookupBL.GetLookupItem(notificationTemplateType.Value.LookupIdentity(LookupCategory.NotificationTemplateType, cultureName));

                    LookupLocalization templateText = notificationDetail.NotificationTemplateType.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

                    notificationDetail.Body = (templateText != null) ? templateText.Text : string.Empty;
                }

                if (notificationWebSubject.HasValue)
                {
                    subject = lookupBL.GetLookupItem(notificationWebSubject.LookupIdentity(LookupCategory.NotificationWebSubject, cultureName));
                }
                else if (notificationEmailSubject.HasValue)
                {
                    subject = lookupBL.GetLookupItem(notificationEmailSubject.LookupIdentity(LookupCategory.NotificationEmailSubject, cultureName));
                }

                if (subject != null && subject.Localizations != null)
                {
                    LookupLocalization subjectLocalization = subject.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();
                    if (subjectLocalization != null)
                    {
                        notificationDetail.Subject = subjectLocalization.Text;
                    }
                }

                return notificationDetail;
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public static NotificationUser BuildNotificationUser(int userId)
        {
            try
            {
                IUserManagementBL userManagementBL = new UserManagementBL();

                return new NotificationUser()
                {
                    User = userManagementBL.GetUserById(userId)
                };
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private static string BuildNotificationLink(NotificationTemplateType notificationTemplateType, Dictionary<string, string> labels)
        {
            try
            {
                string BaseURl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
                string URL = string.Empty;
                string transactionURL = BaseURl + "/User/File/RedirectToCorrectView?TransactionId={TransactionId}&TransactionTypeId={TransTypeId}";
                //string transactionURL = BaseURl + "/User/";
                string copiesURL = BaseURl+"/User/File/Copies";
                string followUpURL = BaseURl+"/User/File/FollowUp";
                string orgUnitURL = BaseURl+"/User/File/RedirectToOrgUnit?TransactionId={TransactionId}&TransactionTypeId={TransTypeId}";
                string taskURL = BaseURl+"/User/File/RedirectToTask?taskId={TaskId}&transactonId={TransactionId}";
                switch (notificationTemplateType)
                {
                    case NotificationTemplateType.TransactionAssignmentWeb:
                    case NotificationTemplateType.RevertRejectTransactionWeb:
                    case NotificationTemplateType.TransactionAssignmentDraftWeb:
                        URL = transactionURL;
                        break;
                    case NotificationTemplateType.ElectronicCopiesWeb:
                        URL = copiesURL;
                        break;
                    case NotificationTemplateType.FollowupWeb:
                        URL = followUpURL;
                        break;
                    case NotificationTemplateType.OrgUnitWeb:
                        URL = orgUnitURL;
                        break;
                    case NotificationTemplateType.NewTaskWeb:
                    case NotificationTemplateType.AcceptTaskWeb:
                    case NotificationTemplateType.RejectTaskWeb:
                    case NotificationTemplateType.ReplyTaskWeb:
                    case NotificationTemplateType.ResendTaskWeb:
                        URL = taskURL;
                        break;
                }
                if (labels != null)
                {
                    foreach (KeyValuePair<string, string> label in labels)
                    {
                        URL = URL.Replace(label.Key, label.Value);
                    }
                }
                return URL;
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
