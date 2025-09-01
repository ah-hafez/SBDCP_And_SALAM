using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public class ManagerTrayBL : TrayBaseBL, IManagerTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.Manager; }
        }

        public override string TrayPermission { get { return UserClaims.Files.Manager; } }

        public override TrayDetailsInfo GetTrayDetailsInfo(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                CheckTrayAuthorization();
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                Tray tray = GetTrayById((int)TrayType, searchCriteria.CultureName);

                TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo()
                {
                    Id = tray.Id,
                    Name = tray.LocalName,
                    TransactionTraysInfo = new List<TransactionTrayInfo>()
                };

                trayDetailsInfo.TodayTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any);

                trayDetailsInfo.TransactionTraysInfo = GetUserTransactionsByTray(TrayType, OrgUnitId, searchCriteria, TransactionDateType.Any, out rowsCount);

                trayDetailsInfo.AllTransactionCount = rowsCount;

                return trayDetailsInfo;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message));
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
        public override IList<TransactionTrayInfo> GetUserTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria, TransactionDateType transactionDate, out int rowsCount)
        {
            try
            {
                CheckTrayAuthorization();

                List<TransactionTrayInfo> transactionTraysInfos = new List<TransactionTrayInfo>();

                IList<Transaction> transactions = TransactionBL.GetUserTransactionsTray(User.Id, OrgUnitId, TrayType, transactionDate, searchCriteria, out rowsCount);

                if (transactions != null)
                {
                    foreach (Transaction transaction in transactions)
                    {
                        TransactionTrayInfo trayInfo = new TransactionTrayInfo()
                        {
                            TransactionAssignmentInfos = transaction.Assignments.Select(ta =>
                            {
                                TransactionAssignmentInfo transactionAssignmentInfo = TransactionAssignmentBL.MapTransactionAssignment(ta, searchCriteria.CultureName);
                                return transactionAssignmentInfo;
                            }).ToList(),

                            transactionDetailsInfo = TransactionBL.MapTransaction(transaction, searchCriteria.CultureName)
                        };
                        transactionTraysInfos.Add(trayInfo);
                    }
                }

                return transactionTraysInfos;
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
        public override void ManagerRevert(int assignmentId, int OrgUnitId)
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                TransactionAssignment transactionAssignment = transactionAssignmentBL.GetTransactionAssignmentById(assignmentId);

                if (transactionAssignment.ToUser.Id != User.Id)
                {
                    CheckManagerPermissions(transactionAssignment);

                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    transactionTaskBL.MoveUserTasks(assignmentId, transactionAssignment.ToUser.Id);

                    transactionAssignment.ToUserId = User.Id;
                    transactionAssignment.Date = DateTime.Now;
                    transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                    transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);
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
        public override void ManagerSave(int transactionId, int OrgUnitId, int trayId, TrayActionType trayActionType, int? assignmentId)
        {
            try
            {
                ManagerRevert(assignmentId.Value, OrgUnitId);
                IFileBL fileBL = new FileBL();
                fileBL.MoveTransaction(transactionId, OrgUnitId, (int)TrayType.Manager, TrayActionType.Save, assignmentId, "");
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
        public override void Save(int transactionId, int OrgUnitId, string remarks, string cultureName, bool SaveWithComplete = false)
        {
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
            TransactionAssignment transactionAssignment = null;
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();

            transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts =>
                    ts.ToUserId == User.Id & ts.ToEntityId == OrgUnitId & ts.TransactionId == transactionId);

            if (transactionAssignment == null)
            {
                throw new BusinessException(StatusCode.TransactionNotFound);
            }
            if (transactionAssignment.Transaction.FollowUp != null && transactionAssignment.Transaction.FollowUp.Where(x => !x.IsDeleted).Count() > 0)
            {
                var followup = transactionAssignment.Transaction.FollowUp.Where(x => !x.IsDeleted).FirstOrDefault();
                if (followup.DateTo.HasValue &&  followup.DateTo > DateTime.Now)
                {
                    throw new BusinessException(StatusCode.TransactionHasActiveFollowup);
                }
            }
            transactionAssignment.TrayId = (int)TrayType.Saved;
            transactionAssignment.Date = DateTime.Now;
            transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            transactionAssignment.Description = remarks;
            transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);
            transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

            transactionRepository.UpdateTransactionStatus(transactionId, Common.TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty));
            transactionRepository.FollowUpUpdateIsDeleted(transactionId, -1);
            var followUp = transactionRepository.GetFollowUpByTransactionIdAndUserId(transactionId, -1);
            if (followUp != null && followUp.FollowUpUserId.HasValue)
            {
                SendTaskNotificationWeb(transactionId, followUp.FollowUpUserId.Value, NotificationSource.CancelFollowupSendToSaved,
                    NotificationTemplateType.CancelFollowupSendToSavedWeb, NotificationTemplateType.CancelFollowupSendToSavedEmail,
                    NotificationEmailSubject.CancelFollowupSendToSavedEmail, NotificationWebSubject.CancelFollowupSendToSaved, cultureName);
            }
        }
        public override void ManagerAssign(int transactionId, int assignmentId, IList<TransactionAssignment> transactionAssignments, int OrgUnit, string cultureName = "ar")
        {
            try
            {
                ManagerRevert(assignmentId, OrgUnit);
                INotificationBL notificationBL = IoC.Resolve<INotificationBL>();
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                List<Transaction> transactions = new List<Transaction>();

                transactions.Add(TransactionBL.GetTransactionById(transactionId));

                transactionAssignmentBL.AssignTransaction(transactions, transactionAssignments, cultureName);
                //foreach (var transaction in transactions)
                //{
                //    notificationBL.SendAssignmentNotification(transaction, transactionAssignments, cultureName);
                //}
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
        private void CheckManagerPermissions(TransactionAssignment transactionAssignment)
        {
            if (transactionAssignment.FromEntityId == transactionAssignment.ToEntityId && transactionAssignment.ToUserId != null && !User.HasClaim(UserClaims.Assignments.WithdrawTransaction))
            {
                throw new BusinessException(StatusCode.PermissionAssignmentsWithdrawTransaction);
            }

            if (transactionAssignment.FromEntityId != transactionAssignment.ToEntityId && !User.HasClaim(UserClaims.Assignments.WithdrawTransactionFromTidyCabins))
            {
                throw new BusinessException(StatusCode.PermissionWithdrawTransactionFromTidyCabins);
            }

            if (transactionAssignment.ToEntityId > 0 && !User.HasClaim(UserClaims.Assignments.WithdrawTransactionFromAllCabins))
            {
                throw new BusinessException(StatusCode.PermissionWithdrawTransactionFromAllCabins);
            }
        }

        private void SendTaskNotificationWeb(int transactionId, int userId, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
            string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                IUserManagementBL userManagementBL = new UserManagementBL();
                NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(userId, cultureName);
                 
                if (notificationSubscriptions.HasFlag(NotificationSubscriptions.Followup))
                {
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
