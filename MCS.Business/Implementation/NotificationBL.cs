using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Notifications;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Framework.Security;


namespace MCS.Business
{
    public class NotificationBL : BaseBL, INotificationBL
    {
        public IList<Notification> GetNotifications(SearchCriteria searchCriteria, bool isRead, string CultureName, out int rowsCount)
        {
            try
            {
                IList<Notification> notifications = new List<Notification>();
                rowsCount = 0;
                DateTime notificationDateRange = DateTime.Now.AddDays(-SystemConfigurations.NotificationDateRange);
                INotificationRepository notificationRepository = IoC.Resolve<NotificationRepository>();
                if (isRead)
                {
                    int Web = NotificationType.Web.LookupIdentity(LookupCategory.NotificationType, CultureName);
                    notifications = notificationRepository.GetNotifications(n => n.Users.Any(u => u.UserId == User.Id) &&
                                    n.Details.Any(d => d.NotificationType.Id == Web) && n.CreatedOn >= notificationDateRange,
                                    searchCriteria, out rowsCount, CultureName).ToList();
                }
                else if (isRead == false)
                {
                    int Web = NotificationType.Web.LookupIdentity(LookupCategory.NotificationType, CultureName);
                    notifications = notificationRepository.GetNotifications(n => n.IsRead == isRead &&
                                    n.Users.Any(u => u.UserId == User.Id) &&
                                    n.Details.Any(d => d.NotificationType.Id == Web) && n.CreatedOn >= notificationDateRange,
                                    searchCriteria, out rowsCount, CultureName).ToList();
                }
                return notifications;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void DeleteNotifications(IList<int> ids)
        {
            try
            {
                INotificationRepository notificationRepository = IoC.Resolve<NotificationRepository>();
                foreach (var id in ids)
                {
                    notificationRepository.DeleteNotification(id, User.Id);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void MarkAsReadNotification(IList<int> ids)
        {
            try
            {
                INotificationRepository notificationRepository = IoC.Resolve<NotificationRepository>();
                foreach (var id in ids)
                {
                    notificationRepository.MarkAsReadNotification(id);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void SendNotification(Notification notification, Dictionary<string, string> labels)
        {
            try
            {
                INotificationRepository notificationRepository = IoC.Resolve<NotificationRepository>();

                foreach (NotificationDetail notificationDetail in notification.Details)
                {
                    if (labels != null)
                    {
                        notificationDetail.Body = FormatNotificationLabels(notificationDetail.Body, labels);
                        if (!string.IsNullOrEmpty(notificationDetail.Subject))
                        {
                            notificationDetail.Subject = FormatNotificationLabels(notificationDetail.Subject, labels);
                        }
                    }
                }
                foreach (NotificationUser notificationUser in notification.Users)
                {
                    foreach (NotificationDetail notificationDetail in notification.Details)
                    {
                        NotificationType notificationType = (NotificationType)notificationDetail.NotificationType.Id.LookupInternalID(LookupCategory.NotificationType, string.Empty);

                        switch (notificationType)
                        {
                            case NotificationType.Email:
                                notificationRepository.AddNotification(notification);
                                break;
                            case NotificationType.SMS:
                                SendSMS(notificationDetail.Body, notificationUser.User.PhoneNumber);
                                break;
                            case NotificationType.Web:
                                notificationRepository.AddNotification(notification);
                                SendSignalR(notificationUser.User.Id, notificationDetail.Body);
                                break;
                        }
                    }
                }
            }
            catch (BusinessException be)
            {
                throw BusinessException.Translate(be);
            }
            catch (DataAccessException de)
            {
                throw BusinessException.Translate(de);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        //public NotificationTemplate GetNotificationTemplate(NotificationTemplateType notificationTemplateType)
        //{
        //    try
        //    {
        //        INotificationRepository notificationRepository = IoC.Resolve<INotificationRepository>();

        //        return notificationRepository.GetNotificationTemplate(notificationTemplateType);
        //    }
        //    catch (BusinessException)
        //    {
        //        throw;
        //    }
        //    catch (DataAccessException)
        //    {
        //        throw new BusinessException(StatusCode.GeneralError);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw BusinessException.Translate(ex);
        //    }
        //}

        private string FormatNotificationLabels(string notificationBody, Dictionary<string, string> labels)
        {
            try
            {
                foreach (KeyValuePair<string, string> label in labels)
                {
                    notificationBody = notificationBody.Replace(label.Key, label.Value);
                }

                return notificationBody;
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
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

        private void SendSMS(string body, string mobileNumber)
        {
            ISMSNotificationService smsNotificationService = new SMSNotificationService();
            SMSMessage smsMessage = new SMSMessage();

            smsMessage.ToNumber = mobileNumber;
            smsMessage.Body = body;

            smsNotificationService.Send(smsMessage);
        }

        private void SendSignalR(int userId, string body)
        {
            ISignalRNotificationService signalRNotificationService = new SignalRNotificationService();
            SignalRMessage signalRMessage = new SignalRMessage();

            signalRMessage.Body = body;

            signalRNotificationService.SendToUser(userId, signalRMessage);
        }
        private bool SendFollowUpNotificationToUser(int userId, string cultureName)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(userId, cultureName);
            if (notificationSubscriptions.HasFlag(NotificationSubscriptions.Followup))
                return true;
            else
                return false;
        }

        public void SendFollowUpNotification(Transaction transaction, NotificationTemplateType notificationTemplateType, NotificationWebSubject notificationWebSubject,
            NotificationUser notificationUser, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled && SendFollowUpNotificationToUser(notificationUser.UserId, cultureName))
            {
                IList<NotificationDetail> notificationDetails = new List<NotificationDetail>();
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();

                string SenderOrgUnitName = string.Empty;
                string SentToOrgUnitName = string.Empty;

                keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                keyValues["{TransactionId}"] = transaction.Id.ToString();
                keyValues["{TransactionTypeId}"] = transaction.TransactionCategoryId.ToString();
                keyValues["{Date}"] = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                keyValues["{SourceType}"] = transaction.TransactionType.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{DeliveryMethod}"] = transaction.DeliveryMethod.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{SubjectClassification}"] = transaction.LetterType.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{ConfidentialityLevel}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{Entity}"] = transaction.Entity.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{User}"] = transaction.User.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{BaseUrl}"] = SystemConfigurations.NotificationBaseUrl;
                keyValues["{Year}"] = DateTimeUtility.GetHijriYear(DateTime.UtcNow).ToString();
                keyValues["{sender}"] = User.UserName;


                ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                keyValues["{Subject}"] = lookupBL.GetLookupItem((int)NotificationEmailSubject.FollowUpRecieveEmail, cultureName).Text;

                NotificationSource notificationSource = NotificationSource.TransactionAssignment; //useless

                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                notificationUsers.Add(notificationUser);

                //Notification Web 
                NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);
            }
        }
        public List<NotificationDetail> GetFailedNotifactions(int failureCount, NotificationType notificationType)
        {
            INotificationRepository notificationRepository = IoC.Resolve<NotificationRepository>();
            return notificationRepository.GetFailedNotifactions(failureCount, notificationType);
        }

        public void UpdateNotifactionDetails(IList<NotificationDetail> tenantNotificationDetail)
        {
            INotificationRepository notificationRepository = IoC.Resolve<NotificationRepository>();
            notificationRepository.UpdateNotifactionDetails(tenantNotificationDetail);
        }
        public void SendAssignmentNotification(Transaction transaction, IList<TransactionAssignment> transactionAssignments, string cultureName = "")
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            foreach (TransactionAssignment transactionAssignment in transactionAssignments)
            {

                TrayType trayType = (TrayType)transactionAssignment.TrayId;
                NotificationSource notificationSource = NotificationSource.None;
                NotificationTemplateType notificationTemplateType = NotificationTemplateType.None;
                NotificationTemplateType notificationEmailTemplateType = NotificationTemplateType.None;
                NotificationEmailSubject notificationEmailSubject = NotificationEmailSubject.None;
                NotificationWebSubject notificationWebSubject = NotificationWebSubject.None;
                switch (trayType)
                {
                    case TrayType.MyTransactions:
                        notificationSource = NotificationSource.AssignTransaction;
                        notificationTemplateType = NotificationTemplateType.TransactionAssignmentWeb;
                        notificationEmailTemplateType = NotificationTemplateType.TransactionAssignmentEmail;
                        notificationEmailSubject = NotificationEmailSubject.TransactionAssignmentEmail;
                        notificationWebSubject = NotificationWebSubject.TransactionAssignment;
                        break;
                    case TrayType.DraftOutbound:
                        notificationSource = NotificationSource.TransactionAssignment;
                        notificationTemplateType = NotificationTemplateType.TransactionAssignmentDraftWeb;
                        notificationEmailTemplateType = NotificationTemplateType.TransactionAssignmentEmail;
                        notificationEmailSubject = NotificationEmailSubject.TransactionAssignmentDraftEmail;
                        notificationWebSubject = NotificationWebSubject.TransactionAssignmentDraft;
                        break;
                    case TrayType.OrgUnit:
                        notificationSource = NotificationSource.OrgUnit;
                        notificationTemplateType = NotificationTemplateType.OrgUnitWeb;
                        notificationEmailTemplateType = NotificationTemplateType.OrgUnitEmail;
                        notificationEmailSubject = NotificationEmailSubject.OrgUnitEmail;
                        notificationWebSubject = NotificationWebSubject.OrgUnit;
                        break;
                }

                if (transactionAssignment.ToUserId != null && transactionAssignment.ToUserId > 0)
                {
                    //get the user preferences and check if willing to have notification on the new assignment

                    NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(transactionAssignment.ToUserId.Value, cultureName);
                    //UserPreferenceInfo userPreferenceInfo = userManagementBL.GetUserPreferenceByUserId(userId, cultureName);

                    if (notificationSubscriptions.HasFlag(NotificationSubscriptions.MyTransactions) ||
                        notificationSubscriptions.HasFlag(NotificationSubscriptions.OutboundDraft))
                    {
                        IList<NotificationUser> notificationUsers = new List<NotificationUser>
                                {
                                    NotificationsManager.BuildNotificationUser(transactionAssignment.ToUserId.Value)
                                };
                        SendTransactionAssignmentNotification(transactionAssignment, notificationSource, notificationTemplateType,
                            notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                            notificationUsers, cultureName);
                    }

                }
                else
                {
                    IOrgUnitRepository OrgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                    var orgunitUsers = OrgUnitRepository.GetUsersByOrgUnitId(transactionAssignment.ToEntityId, cultureName).Where(x => !string.IsNullOrWhiteSpace(x.Email)).ToList();
                    foreach (var user in orgunitUsers)
                    {
                        IList<NotificationUser> notificationUsers = new List<NotificationUser>
                                {
                                    NotificationsManager.BuildNotificationUser(user.Id)
                                };
                        SendTransactionAssignmentNotification(transactionAssignment, notificationSource, notificationTemplateType,
                            notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                            notificationUsers, cultureName);
                    }

                }

                //Notification => Web
                if (transaction.Copies != null && transaction.Copies.Count > 0)
                {
                    List<int> userIds = new List<int>();
                    foreach (var copy in transaction.Copies)
                    {
                        if (copy.UserId.HasValue && copy.UserId.Value > 0)
                        {
                            NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(copy.UserId.Value);
                            if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                            {
                                IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(copy.UserId.Value) };
                                SendTransactionNotification(transaction, NotificationSource.ElectronicCopies, NotificationTemplateType.ElectronicCopiesWeb,
                                    NotificationTemplateType.ElectronicCopiesEmail, NotificationEmailSubject.ElectronicCopiesEmail, NotificationWebSubject.ElectronicCopies,
                                    notificationUsers, cultureName);
                            }
                        }
                    }
                }
            }
        }

        private void SendTransactionAssignmentNotification(TransactionAssignment transactionAssignment, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
        NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
        IList<NotificationUser> notificationUsers, string cultureName)
        {
            try
            {
                if (SystemConfigurations.IsNotificationEnabled)
                {
                    Transaction transaction = transactionAssignment.Transaction;
                    if (transaction == null)
                    {
                        transaction = TransactionBL.GetTransactionById(transactionAssignment.TransactionId, cultureName);
                    }
                    IUserManagementBL userManagementBL = new UserManagementBL();
                    UserProfile FromUser = userManagementBL.GetUserById(transactionAssignment.FromUserId);
                    transactionAssignment.FromUser = FromUser;
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    IOrgUnitBL OrgUnitBL = new OrgUnitBL();

                    keyValues["{Number}"] = transaction.Number.ToString();
                    keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                    keyValues["{TransTypeId}"] = transaction.TransactionCategoryId.ToString();
                    keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == "ar").Text;
                    keyValues["{TransactionTypeIdEn}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == "en").Text;
                    keyValues["{sender}"] = transactionAssignment.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == "ar").FirstOrDefault().Text;
                    keyValues["{senderEn}"] = transactionAssignment.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == "en").FirstOrDefault().Text;

                    keyValues["{Date}"] = transactionAssignment.DateH;
                    keyValues["{TrayName}"] = TrayBaseBL.GetTrayById(transactionAssignment.TrayId, "ar").LocalName;
                    keyValues["{TrayNameEn}"] = TrayBaseBL.GetTrayById(transactionAssignment.TrayId, "en").LocalName;

                    if (transaction.Priority.LocalizationIdentifier != null)
                    {
                        keyValues["{PriorityId}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == "ar").Text;
                        keyValues["{PriorityIdEn}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == "en").Text;

                    }
                    else
                    {
                        keyValues["{PriorityId}"] = transaction.Priority.Text;
                    }
                    if (transaction.Confidentiality.Name != null)
                    {
                        keyValues["{ConfidentialityId}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == "ar").Text;
                        keyValues["{ConfidentialityIdEn}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == "en").Text;

                    }
                    else
                    {
                        keyValues["{ConfidentialityId}"] = transaction.Confidentiality.LocalName;
                    }
                    keyValues["{OrgUnit}"] = OrgUnitBL.GetOrgUnitName(o => o.Id == transactionAssignment.ToEntityId, cultureName);
                    keyValues["{TransactionId}"] = StringCipher.Encrypt(transaction.Id.ToString());

                    //Notification Web
                    NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);

                    //Tenant Notification  Email
                    if (SystemConfigurations.MultiTenantEnabled)
                    {
                        TenantBL tenantBL = new TenantBL();
                        tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType,
                            notificationEmailSubject, notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                    }
                    else
                    {
                        var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                        //System Notification  Email
                        NotificationsManager.EmailNotification(notificationSource, notificationEmailTemplateType,
                            notificationEmailSubject, notificationUsersEmail, cultureName, null, keyValues);
                    }
                }
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void SendTransactionNotification(Transaction transaction, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
           NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
                       IList<NotificationUser> notificationUsers, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                keyValues["{Number}"] = transaction.Number.ToString();
                keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                keyValues["{TransTypeId}"] = transaction.TransactionCategoryId.ToString();
                keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                keyValues["{sender}"] = User.UserName;
                keyValues["{Date}"] = transaction.DateH;
                keyValues["{PriorityId}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{ConfidentialityId}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{TransactionId}"] = StringCipher.Encrypt(transaction.Id.ToString());
                keyValues["{UserName}"] = User.UserName;
                keyValues["{OrgName}"] = OrgUnitBL.GetOrgUnitName(o => o.Id == transaction.OrgUnitId, cultureName);

                //System Notification Web
                NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);

                //System Notification Email
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    TenantBL tenantBL = new TenantBL();
                    tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType,
                        notificationEmailSubject, notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                }
                else
                {
                    var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                    //System Notification  Email
                    NotificationsManager.EmailNotification(notificationSource, notificationEmailTemplateType,
                        notificationEmailSubject, notificationUsersEmail, cultureName, null, keyValues);
                }
            }
        }
    }
}
