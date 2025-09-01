using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class MyTransactionsTrayBL : TrayBaseBL, IMyTransactionsTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.MyTransactions; }
        }

        public override string TrayPermission { get { return UserClaims.Files.MyTransactions; } }

        public override void Save(int transactionId, int OrgUnitId, string remarks, string cultureName, bool SaveWithComplete = false)
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                TransactionAssignment transactionAssignment = null;

                Transaction transaction = transactionRepository.GetTransactionById(transactionId);
                if (SaveWithComplete && transaction.TransactionCategoryId != TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                {
                    return;
                }

                if (SaveWithComplete)
                {
                    transactionAssignment = transactionAssignmentRepository.GetLastTransactionAssignments(transactionId, cultureName);
                }
                else
                {
                    transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts =>
                            ts.ToUserId == User.Id & ts.ToEntityId == OrgUnitId &
                            ts.TransactionId == transactionId);
                }

                if (transactionAssignment == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }
                List<int> rejectedStatus = new List<int> { (int)FollowupStatus.WithDrow, (int)FollowupStatus.Cancled, (int)FollowupStatus.Completed };
                //if (transaction.FollowUp != null && transaction.FollowUp.Where(x => !x.IsDeleted && !rejectedStatus.Any(rs => rs == x.FollowUpStatusId)).Count() > 0)
                //{
                //    var followup = transaction.FollowUp.Where(x => !x.IsDeleted && !rejectedStatus.Any(rs => rs == x.FollowUpStatusId)).FirstOrDefault();
                //    if (followup.DateTo > DateTime.Now)
                //    {
                //        throw new BusinessException(StatusCode.TransactionHasActiveFollowup);
                //    }
                //}
                transactionAssignment.TrayId = (int)TrayType.Saved;
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                transactionAssignment.Description = remarks;
                if (SaveWithComplete == false)
                {
                    transactionAssignment.ActionId = (int)ActionTransactionType.Saved;
                }
                else
                {
                    transactionAssignment.ActionId = (int)ActionTransactionType.Completed;
                }
                transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);
                transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

                int newStatusId = SaveWithComplete ? Common.TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty) : Common.TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                transactionRepository.UpdateTransactionStatus(transactionId, newStatusId);
                transactionRepository.UpdateTransactionSavedReason(transactionId, remarks);

                transactionRepository.FollowUpUpdateIsDeleted(transactionId, -1);
                var followUp = transactionRepository.GetFollowUpByTransactionIdAndUserId(transactionId, -1);
                if (followUp != null && followUp.FollowUpUserId.HasValue)
                {
                    SendTaskNotificationWeb(transactionId, followUp.FollowUpUserId.Value, NotificationSource.CancelFollowupSendToSaved, NotificationTemplateType.CancelFollowupSendToSavedWeb,
                        NotificationTemplateType.CancelFollowupSendToSavedEmail, NotificationEmailSubject.CancelFollowupSendToSavedEmail,
                        NotificationWebSubject.CancelFollowupSendToSaved, cultureName);
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
        public override void LinkedSave(int transactionId, int OrgUnitId, string remarks, int UserId, string cultureName, bool SaveWithComplete = false)
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                TransactionAssignment transactionAssignment = null;

                Transaction transaction = transactionRepository.GetTransactionById(transactionId);
                if (SaveWithComplete && transaction.TransactionCategoryId != TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                {
                    return;
                }

                if (SaveWithComplete)
                {
                    transactionAssignment = transactionAssignmentRepository.GetLastTransactionAssignments(transactionId, cultureName);
                }
                else
                {
                    transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts =>
                            (ts.ToUserId == UserId || ts.ToUserId == null) & ts.ToEntityId == OrgUnitId &
                            ts.TransactionId == transactionId);
                }

                if (transactionAssignment == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }
                List<int> rejectedStatus = new List<int> { (int)FollowupStatus.WithDrow, (int)FollowupStatus.Cancled, (int)FollowupStatus.Completed };
                if (transaction.FollowUp != null && transaction.FollowUp.Where(x => !x.IsDeleted && !rejectedStatus.Any(rs => rs == x.FollowUpStatusId)).Count() > 0)
                {
                    var followup = transaction.FollowUp.Where(x => !x.IsDeleted && !rejectedStatus.Any(rs => rs == x.FollowUpStatusId)).FirstOrDefault();
                    if (followup.DateTo > DateTime.Now)
                    {
                        throw new BusinessException(StatusCode.TransactionHasActiveFollowup);
                    }
                }
                transactionAssignment.TrayId = (int)TrayType.Saved;
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                transactionAssignment.Description = remarks;
                if (SaveWithComplete == false)
                {
                    transactionAssignment.ActionId = (int)ActionTransactionType.Saved;
                }
                else
                {
                    transactionAssignment.ActionId = (int)ActionTransactionType.Completed;
                }
                transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);
                transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

                int newStatusId = SaveWithComplete ? Common.TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty) : Common.TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                transactionRepository.UpdateTransactionStatus(transactionId, newStatusId);
                transactionRepository.UpdateTransactionSavedReason(transactionId, remarks);

                transactionRepository.FollowUpUpdateIsDeleted(transactionId, -1);
                var followUp = transactionRepository.GetFollowUpByTransactionIdAndUserId(transactionId, -1);
                if (followUp != null && followUp.FollowUpUserId.HasValue)
                {
                    SendTaskNotificationWeb(transactionId, followUp.FollowUpUserId.Value, NotificationSource.CancelFollowupSendToSaved, NotificationTemplateType.CancelFollowupSendToSavedWeb,
                        NotificationTemplateType.CancelFollowupSendToSavedEmail, NotificationEmailSubject.CancelFollowupSendToSavedEmail,
                        NotificationWebSubject.CancelFollowupSendToSaved, cultureName);
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

        public override void RevertAssignTransaction(int transactionId, int OrgUnitId, int trayId)
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
                transactionAssignmentBL.RevertAssignByTransaction(transactionId, OrgUnitId, trayId);
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

        private void SendTaskNotificationWeb(int transactionId, int userId, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject,
            NotificationWebSubject notificationWebSubject, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                IUserManagementBL userManagementBL = new UserManagementBL();
                NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(userId, cultureName);

                 
                if (notificationSubscriptions.HasFlag(NotificationSubscriptions.Followup))
                {
                    notificationUsers.Add(NotificationsManager.BuildNotificationUser(userId));
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    var transaction = TransactionBL.GetTransactionById(transactionId, cultureName);
                    keyValues.Add("{Number}", transaction.Number.ToString());
                    keyValues.Add("{Subject}", transaction.Subject);
                    keyValues.Add("{TransactionTypeId}", transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text);
                    keyValues.Add("{PriorityId}", transaction.Priority.Text);
                    keyValues.Add("{ConfidentialityId}", transaction.Confidentiality.LocalName);

                    //Notification Web
                    NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);
                    //Notification Email
                    if (SystemConfigurations.MultiTenantEnabled)
                    {
                        TenantBL tenantBL = new TenantBL();
                        tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType, notificationEmailSubject,
                        notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
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
}
