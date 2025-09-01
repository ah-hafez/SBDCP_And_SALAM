using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public class CopiesTrayBL : TrayBaseBL, ICopiesTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.InternalInboundCopies; }
        }

        public override string TrayPermission { get { return UserClaims.Files.Copies; } }

        public override IList<TransactionTrayInfo> GetUserTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria, TransactionDateType transactionDate, out int rowsCount)
        {
            try
            {
                IList<TransactionCopy> transactions = null;
                IList<Transaction> transactionAssignments = null;
                int Delete = TransCopyStatus.Delete.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                int Viewed = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                Expression<Func<TransactionCopy, bool>> where = null;

                if (searchCriteria.Filters == null)
                {
                    where = tc => tc.EntityId == OrgUnitId &
                            (tc.UserId == User.Id | tc.UserId == null) &
                            tc.IsSent == 1 & tc.Status != Delete &
                            !tc.Transaction.IsDeleted;
                }
                else
                {
                    int StatusId = Convert.ToInt32(searchCriteria.Filters.SingleOrDefault(e => e.ColumnName == "Status")?.Value);
                    if (StatusId == 0)
                    {
                        where = tc => tc.EntityId == OrgUnitId &
                                                   (tc.UserId == User.Id | tc.UserId == null) &
                                                   tc.IsSent == 1 & tc.Status != Delete &
                                                   !tc.Transaction.IsDeleted;
                    }
                    else
                    {
                        where = tc => tc.EntityId == OrgUnitId &
                                                 (tc.UserId == User.Id | tc.UserId == null) &
                                                 tc.IsSent == 1 & tc.Status == StatusId &
                                                 !tc.Transaction.IsDeleted;
                    }
                }

                transactions = TransactionBL.GetTransactionCopies(where, trayType, searchCriteria, transactionDate, User.Id, out rowsCount);

                IList<TransactionTrayInfo> transactionTrayInfos = TransactionBL.MapTransactionCopy(transactions, searchCriteria.CultureName);
                //yousefs
                transactionAssignments = TransactionBL.GetUserTransactionsTray(User.Id, OrgUnitId, trayType, transactionDate, searchCriteria, out int assignmentsRowsCount);

                TransactionBL.MapTransaction(transactionAssignments, searchCriteria.CultureName).ToList().ForEach(t => transactionTrayInfos.Add(t));

                return transactionTrayInfos;
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

        public override TrayDetailsInfo GetTrayDetailsInfo(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                CheckTrayAuthorization();

                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                Tray tray = TrayBaseBL.GetTrayById((int)TrayType, searchCriteria.CultureName);

                TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo()
                {
                    Id = tray.Id,
                    Name = tray.LocalName,
                    TransactionTraysInfo = new List<TransactionTrayInfo>()
                };

                trayDetailsInfo.TodayTransactionCount = TransactionBL.GetTransactionCopiesCount(User.Id, OrgUnitId, DateTime.Now);//transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any) +
                trayDetailsInfo.TransactionTraysInfo = GetUserTransactionsByTray(TrayType, OrgUnitId, searchCriteria, TransactionDateType.Any, out rowsCount);
                trayDetailsInfo.AllTransactionCount = TransactionBL.GetTransactionCopiesCount(User.Id, OrgUnitId, null);//transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any) +

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

        public override void Viewed(int transactionId, int OrgUnitId, int userId, string cultureName)
        {
            try
            {

                TransactionCopy transactionCopy = TransactionBL.GetCopyTransactionByID(transactionId);
                Transaction transaction = TransactionBL.GetTransactionById(transactionCopy.TransactionId);
                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                if (transactionCopy != null)
                {
                    transactionBL.SetTransactionCopyToViewed(transactionCopy);
                }

                IUserManagementBL userManagementBL = new UserManagementBL();


                TransactionAssignment transAssignment = transaction.Assignments.Where(a => a.TransactionId == transaction.Id && a.ToEntityId == OrgUnitId
                            && a.TrayId == (int)TrayType.Copies).FirstOrDefault();

                if (transAssignment != null)
                {
                    transactionAssignmentBL.SetTransactionAssignmentToViewed(transAssignment);
                }

                if (transaction.Copies != null && transaction.Copies.Count > 0)
                {
                    IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                    Dictionary<int, string> userIdsWithorgNames = new Dictionary<int, string>();
                    foreach (var item in transaction.Copies)
                    {
                        string orgName = string.Empty;
                        if (item.EntityId.HasValue)
                        {
                            orgName = OrgUnitBL.GetOrgUnitName(o => o.Id == item.EntityId.Value, cultureName);
                        }
                        if (item.UserId != null)
                        {
                            NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(item.UserId.Value);
                            if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                            {
                                var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(item.UserId.Value) };
                                SendTransactionNotification(transaction, NotificationSource.Viewed, NotificationTemplateType.ViewedWeb, NotificationTemplateType.ViewedEmail,
                                    NotificationEmailSubject.ViewedEmail, NotificationWebSubject.Viewed, notificationUsers, orgName, cultureName);
                            }
                            userIdsWithorgNames.Add(item.UserId.Value, orgName);
                        }
                    } 
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

        public override void DeleteCopy(int transactionCopyId, int OrgUnitId, int userId, string cultureName)
        {
            try
            {
                TransactionCopy transactionCopy = TransactionBL.GetCopyTransactionByID(transactionCopyId);
                Transaction transaction = TransactionBL.GetTransactionById(transactionCopy.TransactionId);
                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));


                if (transaction.Copies != null)
                {
                    transactionBL.SetTransactionCopyToDelete(transactionCopy);

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
        public override void SetTransactionCopyToUndo(int transactionId, int OrgUnitId, int userId, string cultureName)
        {
            try
            {
                Transaction transaction = TransactionBL.GetTransactionById(transactionId);
                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                if (transaction.Copies != null)
                {
                    foreach (TransactionCopy Transaction in transaction.Copies)
                    {
                        TransactionCopy transactionCopy;
                        //For Org Unit
                        if (Transaction.UserId == null)
                        {
                            transactionCopy = transaction.Copies.Where(tc => tc.TransactionId == transaction.Id && tc.EntityId == OrgUnitId && tc.IsSent == 1).FirstOrDefault();
                        }//For User
                        else
                        {
                            transactionCopy = transaction.Copies.Where(tc => tc.TransactionId == transaction.Id && tc.EntityId == OrgUnitId && tc.IsSent == 1 && tc.UserId == userId).FirstOrDefault();
                        }

                        if (transactionCopy != null)
                        {
                            transactionBL.SetTransactionCopyToUndo(transactionCopy);
                        }
                    }
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

        public override TrayDetailsInfo GetPopulariazations(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            rowsCount = 0;

            Tray tray = TrayBaseBL.GetTrayById((int)TrayType, searchCriteria.CultureName);

            TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo()
            {
                Id = tray.Id,
                Name = tray.LocalName,
                TransactionTraysInfo = new List<TransactionTrayInfo>()
            };

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

            int? userWeigth = permissions.Max(p => p.Weight);

            Expression<Func<TransactionAssignment, bool>> where = (s =>
                              s.ToUserId == User.Id &
                              s.TrayId == (int)TrayType &
                              s.ToEntityId == OrgUnitId &
                              s.Action.Type.Id == ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty) &
                              !s.Viewed &
                              s.IsPopulariazation
                              );

            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<TransactionAssignmentRepository>();

            IList<Transaction> transactions = transactionAssignmentRepository.GetUserTransactionsTray(where, userWeigth, searchCriteria, User.Id, out rowsCount);

            if (transactions != null)
            {
                TransactionBL.MapTransaction(transactions, searchCriteria.CultureName).ToList().ForEach(t => trayDetailsInfo.TransactionTraysInfo.Add(t));
            }

            return trayDetailsInfo;
        }

        private void SendTransactionNotification(
            Transaction transaction, 
            NotificationSource notificationSource, 
            NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, 
            NotificationEmailSubject notificationEmailSubject, 
            NotificationWebSubject notificationWebSubject,
            IList<NotificationUser> notificationUsers, 
            string orgName, 
            string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();

                keyValues["{Number}"] = transaction.Number.ToString();
                keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                keyValues["{PriorityId}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{ConfidentialityId}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{OrgName}"] = orgName;
                keyValues["{UserName}"] = User.UserName;

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
