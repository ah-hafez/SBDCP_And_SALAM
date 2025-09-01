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
    public class FileBL : BaseBL, IFileBL
    {
        public IList<TrayDetailsInfo> GetUserTrays(int OrgUnitId, string cultureName)
        {
            try
            {
                return TrayBaseBL.GetUserTrays(User.Id, OrgUnitId, cultureName);
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
        public TrayDetailsInfo GetTrayDetailsInfo(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                ITrayBL trayBL = TrayBaseBL.Create(trayType);
                return trayBL.GetTrayDetailsInfo(OrgUnitId, searchCriteria, out rowsCount);
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


        public Transaction GetNextTransactionId(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria)
        {
            try
            {
                ITrayBL trayBL = TrayBaseBL.Create(trayType);
                return trayBL.GetNextTransaction(OrgUnitId, searchCriteria);
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
        public TrayDetailsInfo GetWithdrawalData(int? transId, int? orgunitId, int? transactionTypeId, int? year, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                ITrayBL trayBL = TrayBaseBL.Create(TrayType.Withdrawal);
                return trayBL.GetWithdrawalData(transId, orgunitId, transactionTypeId, year, searchCriteria, out rowsCount);
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

        public TransactionAssignment GetTransactionAssignmentLight(int orgUnitId, int transactionId)
        {
            try
            {
                ITrayBL trayBL = TrayBaseBL.Create(TrayType.OrgUnit);
                var result = trayBL.GetTransactionAssignmentLightByOrgUnitIdAndTransactionId(orgUnitId, transactionId);
                if (result == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }
                return result;
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
        public TrayDetailsInfo GetSelectedTransactions(List<int> transactionsIds, string CultureName)
        {
            try
            {
                IPermissionBL permissionBL = new PermissionBL();

                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();


                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                IList<Transaction> transactions = transactionAssignmentRepository.GetTransactionsByIds(transactionsIds, CultureName, userWeight, User.Id);

                List<TransactionTrayInfo> transactionTrayInfos = new List<TransactionTrayInfo>();

                foreach (Transaction transaction in transactions)
                {
                    TransactionTrayInfo transactionTrayInfo = new TransactionTrayInfo();

                    transactionTrayInfo.transactionDetailsInfo = TransactionBL.MapTransaction(transaction, CultureName);

                    transactionTrayInfos.Add(transactionTrayInfo);
                }

                TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo();
                trayDetailsInfo.TransactionTraysInfo = transactionTrayInfos;

                return trayDetailsInfo;
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
        public IList<TransactionTrayInfo> GetAllUserTransactionsByTray(TrayType trayType, int OrgUnitId, Common.TransactionDateType transactionDate, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                if (trayType == TrayType.DeletedDraftOutbound)
                {
                    searchCriteria.IsDeleted = true;
                }
                TrayType blTrayType = trayType;
                if (trayType == TrayType.InternalInboundCopies)
                {
                    blTrayType = TrayType.Copies;
                }

                ITrayBL trayBL = TrayBaseBL.Create(blTrayType);
                return trayBL.GetUserTransactionsByTray(trayType, OrgUnitId, searchCriteria, transactionDate, out rowsCount);
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
        public void MoveTransaction(int transactionId, int OrgUnitId, int trayId, TrayActionType trayActionType, int? assignmentId, string remarks, int userId = 0, string cultureName = "", params object[] extraParams)
        {
            try
            {
                ITrayBL trayBL = TrayBaseBL.Create((TrayType)trayId);

                switch (trayActionType)
                {
                    case TrayActionType.Save:
                        trayBL.Save(transactionId, OrgUnitId, remarks, cultureName);
                        break;

                    case TrayActionType.Assign:
                        trayBL.Assign(transactionId, OrgUnitId, cultureName);
                        break;

                    case TrayActionType.Revert:
                        trayBL.RevertAssignTransaction(transactionId, OrgUnitId, trayId);
                        break;

                    case TrayActionType.RejectRevert:
                        trayBL.RevertReject(transactionId, OrgUnitId, trayId, remarks, cultureName);
                        break;

                    case TrayActionType.RejectRevertToCreator:
                        trayBL.RevertRejectToCreator(transactionId, OrgUnitId, trayId, remarks, cultureName);
                        break;

                    case TrayActionType.DeleteDraft:
                        trayBL.DeleteDraft(transactionId);
                        break;

                    case TrayActionType.SaveRevert:
                        trayBL.SaveRevert(transactionId, OrgUnitId);
                        break;

                    case TrayActionType.Viewed:
                        trayBL.Viewed(transactionId, OrgUnitId, userId, cultureName);
                        break;

                    case TrayActionType.DeleteCopy:
                        trayBL.DeleteCopy(transactionId, OrgUnitId, userId, cultureName);
                        break;
                    case TrayActionType.UndoDeleteCopy:
                        trayBL.SetTransactionCopyToUndo(transactionId, OrgUnitId, userId, cultureName);
                        break;

                    case TrayActionType.ManagerRevert:
                        if (assignmentId.HasValue)
                        {
                            trayBL.ManagerRevert(assignmentId.Value, OrgUnitId);
                        }
                        break;
                    case TrayActionType.ManagerSave:
                        if (assignmentId.HasValue)
                        {
                            trayBL.ManagerSave(transactionId, OrgUnitId, trayId, trayActionType, assignmentId);
                        }

                        break;
                    case TrayActionType.ManagerAssign:
                        int assignId = 0;

                        if (assignmentId.HasValue)
                        {
                            assignId = assignmentId.Value;
                        }
                        IList<TransactionAssignment> transactionAssignments = null;
                        transactionAssignments = extraParams[0] as IList<TransactionAssignment>;
                        trayBL.ManagerAssign(transactionId, assignId, transactionAssignments, OrgUnitId, cultureName);
                        break;
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
        public void LinkedMoveTransaction(int transactionId, int OrgUnitId, int trayId, TrayActionType trayActionType, int? assignmentId, string remarks, int userId = 0, string cultureName = "", params object[] extraParams)
        {
            try
            {
                ITrayBL trayBL = TrayBaseBL.Create((TrayType)trayId);

                switch (trayActionType)
                {
                    case TrayActionType.Save:
                        trayBL.LinkedSave(transactionId, OrgUnitId, remarks, userId, cultureName);
                        break;

                    case TrayActionType.Assign:
                        trayBL.Assign(transactionId, OrgUnitId, cultureName);
                        break;

                    case TrayActionType.Revert:
                        trayBL.RevertAssignTransaction(transactionId, OrgUnitId, trayId);
                        break;

                    case TrayActionType.RejectRevert:
                        trayBL.RevertReject(transactionId, OrgUnitId, trayId, remarks, cultureName);
                        break;

                    case TrayActionType.RejectRevertToCreator:
                        trayBL.RevertRejectToCreator(transactionId, OrgUnitId, trayId, remarks, cultureName);
                        break;

                    case TrayActionType.DeleteDraft:
                        trayBL.DeleteDraft(transactionId);
                        break;

                    case TrayActionType.SaveRevert:
                        trayBL.SaveRevert(transactionId, OrgUnitId);
                        break;

                    case TrayActionType.Viewed:
                        trayBL.Viewed(transactionId, OrgUnitId, userId, cultureName);
                        break;

                    case TrayActionType.DeleteCopy:
                        trayBL.DeleteCopy(transactionId, OrgUnitId, userId, cultureName);
                        break;
                    case TrayActionType.UndoDeleteCopy:
                        trayBL.SetTransactionCopyToUndo(transactionId, OrgUnitId, userId, cultureName);
                        break;

                    case TrayActionType.ManagerRevert:
                        if (assignmentId.HasValue)
                        {
                            trayBL.ManagerRevert(assignmentId.Value, OrgUnitId);
                        }
                        break;
                    case TrayActionType.ManagerSave:
                        if (assignmentId.HasValue)
                        {
                            trayBL.ManagerSave(transactionId, OrgUnitId, trayId, trayActionType, assignmentId);
                        }

                        break;
                    case TrayActionType.ManagerAssign:
                        int assignId = 0;

                        if (assignmentId.HasValue)
                        {
                            assignId = assignmentId.Value;
                        }
                        IList<TransactionAssignment> transactionAssignments = null;
                        transactionAssignments = extraParams[0] as IList<TransactionAssignment>;
                        trayBL.ManagerAssign(transactionId, assignId, transactionAssignments, OrgUnitId, cultureName);
                        break;
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

        public TransactionDetails CreateOutboundExternal(int transactionId, int trayId, Transaction transactionExternal)
        {
            try
            {
                ITrayBL trayBL = TrayBaseBL.Create((TrayType)trayId);
                return trayBL.CreateOutboundExternal(transactionId, transactionExternal);
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
        public Transaction PrepareOutboundCreation(int transactionId, int OrgUnitId, int trayId, string cultureName)
        {
            try
            {
                ITrayBL trayBL = TrayBaseBL.Create((TrayType)trayId);
                return trayBL.PrepareOutboundCreation(transactionId, OrgUnitId, cultureName);
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

        public void SendLateTransactionReminderToSender(string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                List<int> transactionsIds = new List<int>();
                List<Transaction> lateTransactions = transactionRepository.GetLateTransactions();

                foreach (var transaction in lateTransactions)
                {
                    SendLateTransactionNotification(transaction, NotificationSource.LateTransaction, NotificationTemplateType.LateTransactionWeb,
                        NotificationTemplateType.LateTransactionEmail, NotificationEmailSubject.LateTransaction,
                        NotificationWebSubject.LateTransaction, cultureName);
                    transactionsIds.Add(transaction.Id);
                }
                if (transactionsIds.Count > 0)
                {
                    // transactionTaskRepository.UpdateTaskReminderBeforeEnded(transactionsIds);
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
        public List<Transaction> GetTransactionsByExternalPartyId(int externalPartyId, int orgUnitId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransactionsByExternalPartyId(externalPartyId, orgUnitId);
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

        private void SendLateTransactionNotification(Transaction transaction, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
        NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
        string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                int userId;
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                IUserManagementBL userManagementBL = new UserManagementBL();

                if (transaction.Assignments != null && transaction.Assignments.Count > 0)
                {
                    userId = transaction.Assignments[0].FromUserId;
                    notificationUsers.Add(NotificationsManager.BuildNotificationUser(userId));


                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    //keyValues.Add("{TaskId}", task.Id.ToString());
                    //keyValues.Add("{TransactionId}", transaction.Id.ToString());
                    keyValues.Add("{Number}", transaction.Number.ToString());
                    //keyValues.Add("{Subject}", transaction.Subject);
                    //keyValues.Add("{TransactionTypeId}", transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text);
                    //keyValues.Add("{PriorityId}", transaction.Priority.Text);
                    //keyValues.Add("{ConfidentialityId}", transaction.Confidentiality.LocalName);
                    //keyValues.Add("{UserName}", User?.UserName);
                    //keyValues.Add("{DeliveryDateH}", task.DeliveryDateH);
                    //keyValues.Add("{StatusDescription}", task.StatusDescription);
                    //keyValues.Add("{TaskProcessingPeriod}", taskProcessingPeriod);

                    //Notification Web
                    // NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);

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

        public void SendLateTransactionWithNotifyLetterTypes(string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                List<int> transactionsIds = new List<int>();
                List<Transaction> lateTransactions = transactionRepository.LateTransactionWithNotifyLetterTypes();

                foreach (var transaction in lateTransactions)
                {
                    SendLateLetterTypeTransactionsNotification(transaction, NotificationSource.LateTransaction, NotificationTemplateType.LateTransactionWeb,
                        NotificationTemplateType.LateTransactionEmail, NotificationEmailSubject.LateTransaction,
                        NotificationWebSubject.LateTransaction, cultureName);
                    transactionsIds.Add(transaction.Id);
                }
                if (transactionsIds.Count > 0)
                {
                    // transactionTaskRepository.UpdateTaskReminderBeforeEnded(transactionsIds);
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
        private void SendLateLetterTypeTransactionsNotification(Transaction transaction, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
        NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
        string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                IUserManagementBL userManagementBL = new UserManagementBL();

                if (transaction.Assignments != null && transaction.Assignments.Count > 0)
                {
                    TransactionAssignment transactionAssignment = transaction.Assignments[0];
                    if (transactionAssignment.ToUserId.HasValue)
                    {
                        if (transactionAssignment.FromUserId == transactionAssignment.ToUserId)
                        {
                            notificationUsers.Add(NotificationsManager.BuildNotificationUser(transactionAssignment.FromUserId));
                        }
                        else
                        {
                            notificationUsers.Add(NotificationsManager.BuildNotificationUser(transactionAssignment.FromUserId));
                            notificationUsers.Add(NotificationsManager.BuildNotificationUser(transactionAssignment.ToUserId.Value));
                        }
                    }
                    else
                    {
                        notificationUsers.Add(NotificationsManager.BuildNotificationUser(transactionAssignment.FromUserId));
                    }

                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    //keyValues.Add("{TaskId}", task.Id.ToString());
                    keyValues.Add("{Number}", transaction.Number.ToString());
                    //keyValues.Add("{Number}", transaction.Number.ToString());
                    //keyValues.Add("{Subject}", transaction.Subject);
                    //keyValues.Add("{TransactionTypeId}", transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text);
                    //keyValues.Add("{PriorityId}", transaction.Priority.Text);
                    //keyValues.Add("{ConfidentialityId}", transaction.Confidentiality.LocalName);
                    //keyValues.Add("{UserName}", User?.UserName);

                    //keyValues["{Number}"] = transaction.Number.ToString();
                    //keyValues["{TransTypeId}"] = transaction.TransactionCategoryId.ToString();
                    //keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                    //keyValues["{sender}"] = User.UserName;
                    //keyValues["{Date}"] = transaction.DateH;
                    //keyValues["{PriorityId}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                    //keyValues["{ConfidentialityId}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                    //keyValues["{TransactionId}"] = transaction.Id.ToString();
                    //keyValues["{UserName}"] = User.UserName;
                    // keyValues["{OrgName}"] = OrgUnitBL.GetOrgUnitName(o => o.Id == transaction.OrgUnitId, cultureName);

                    //keyValues.Add("{DeliveryDateH}", task.DeliveryDateH);
                    //keyValues.Add("{StatusDescription}", task.StatusDescription);
                    //keyValues.Add("{TaskProcessingPeriod}", taskProcessingPeriod);

                    //Notification Web
                    // NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);

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

        public void SendNearlyLateTransaction(string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                List<int> transactionsIds = new List<int>();
                List<Transaction> lateTransactions = transactionRepository.SendNearlyLateTransaction();

                foreach (var transaction in lateTransactions)
                {
                    SendLateLetterTypeTransactionsNotification(transaction, NotificationSource.LateTransaction, NotificationTemplateType.LateTransactionWeb,
                        NotificationTemplateType.LateTransactionEmail, NotificationEmailSubject.LateTransaction,
                        NotificationWebSubject.LateTransaction, cultureName);
                    transactionsIds.Add(transaction.Id);
                }
                if (transactionsIds.Count > 0)
                {
                    // transactionTaskRepository.UpdateTaskReminderBeforeEnded(transactionsIds);
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
    }
}
