using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Security;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public class TransactionAssignmentBL : BaseBL, ITransactionAssignmentBL
    {
        [Flags]
        private enum AssignmentFlag
        {
            None = 0,
            SentToUser = 1,
            SentToOrgUnit = 2,
            SentAsCopy = 4
        }

        public void SetTransactionAssignmentToViewed(TransactionAssignment transactionAssignment)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                transactionAssignmentRepository.SetTransactionAssignmentToViewed(transactionAssignment);
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
        public void SetTransactionAssignmentToViewed(int transactionAssignmentId)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                transactionAssignmentRepository.SetTransactionAssignmentToViewed(transactionAssignmentId);
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
        public int AddTransactionAssignment(TransactionAssignment transactionAssignment)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                return transactionAssignmentRepository.AddTransactionAssignment(transactionAssignment);
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

        public void UpdateTransactionAssignment(TransactionAssignment transactionAssignment)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignment);
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
        public void MoveAllUserTransactions(int UserId)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                transactionAssignmentRepository.MoveAllUserTransactions(UserId);
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

        public void DeleteTransactionAssignments(IList<int> ids)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignment = IoC.Resolve<ITransactionAssignmentRepository>();
                foreach (int id in ids)
                {
                    transactionAssignment.DeleteTransactionAssignment(id);
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
        public TransactionAssignment GetTransactionAssignmentLight(int userId, int trayId, int OrgUnitId, int transactionId)
        {
            Expression<Func<TransactionAssignment, bool>> where = null;
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
            int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
            int MultiOwnership = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
            int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);

            where = s => s.ToUser == null & s.TrayId == (int)TrayType.OrgUnit & s.ToEntityId == OrgUnitId &
                   (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == MultiOwnership) &
                   s.Transaction.TransactionCategoryId != ExternalOutbound & s.Transaction.Id == transactionId;
            var result = transactionAssignmentRepository.GetTransactionAssignmentLight(where);

            return result;
        }
        public int GetTransactionAssignmentCount(int userId, int trayId, int OrgUnitId, TransactionDateType transactionDateType = TransactionDateType.Any)
        {
            try
            {
                IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                IList<int> transactionsIds;
                IList<Permission> permissions =
                    permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                if (permissions != null && permissions.Count > 0)
                {
                    int? userWeigth = permissions.Max(s => s.Weight);
                    //TODO: combine the expression
                    ITransactionAssignmentRepository transactionAssignmentRepository =
                        IoC.Resolve<ITransactionAssignmentRepository>();

                    Expression<Func<TransactionAssignment, bool>> where = null;

                    TrayType trayType = (TrayType)trayId;


                    int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Rejected = TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int MultiOwnership = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int TempSave = TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Completed = TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int Outbound = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Sent = TransactionStatus.Sent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Reserved = TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int SendCopyToView = ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty);
                    int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int electronicDelivery = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);
                    int electronicPaperDelivery = DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);
                    int DeletedCopy = TransCopyStatus.Delete.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                    int ViewedCopy = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                    int deletedStatus = TransactionStatus.Deleted.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int reserved = TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    switch (trayType)
                    {
                        case TrayType.MyTransactions:
                            where = s =>
                                      s.ToUserId == userId &
                                      s.TrayId == trayId &
                                      s.ToEntityId == OrgUnitId &
                                      (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == Rejected | s.Transaction.StatusId == MultiOwnership) &
                                      s.Transaction.TransactionCategoryId != ExternalOutbound;
                            break;
                        case TrayType.DraftOutbound:
                            where = (s =>
                         s.ToUserId == userId &
                         s.ToEntityId == OrgUnitId &&
                         s.Transaction.StatusId != Reserved &&
                         (((s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected) & s.Transaction.TransactionCategoryId == DraftOutbound & s.TrayId == (int)trayType)
                         | (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & (s.Transaction.IsDraft | s.Transaction.IsPresentationDraft))
                            /*| (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & s.Transaction.DeliveryMethodId == electronicPaperDelivery
                               & !s.Transaction.PrintedDeliveryReport & s.Transaction.StatusId != Outbound & s.Transaction.StatusId != Sent & s.Transaction.StatusId != Reserved)*/
                            )
                         );
                            break;
                        case TrayType.DeletedDraftOutbound:
                            where = (s =>
                            s.ToUserId == userId &
                            s.ToEntityId == OrgUnitId &
                            (s.Transaction.IsDraft | s.Transaction.IsPresentationDraft) & s.Transaction.StatusId == deletedStatus);

                            break;
                        case TrayType.OutboundExternal:
                            {
                                transactionsIds = transactionRepository.GetELcOutBoundIds(userId, OrgUnitId);
                                where = (s =>
                                transactionsIds.Contains(s.Transaction.Id) &&
                                         (s.Transaction.TransactionCategoryId == ExternalOutbound &&
                                         (s.DeliveryMethodId == electronicDelivery ||
                                         (s.DeliveryMethodId == electronicPaperDelivery &&
                                          s.Transaction.PrintedDeliveryReport))) &&
                                         !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft
                               );
                            }
                            break;
                        // where = (s =>
                        //           s.FromEntityId == OrgUnitId &
                        //          (s.Transaction.TransactionCategoryId == ExternalOutbound &
                        //          (s.DeliveryMethodId == electronicDelivery ||
                        //          (s.DeliveryMethodId == electronicPaperDelivery &&
                        //           s.Transaction.PrintedDeliveryReport))) &
                        //          !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft  
                        //);
                        // break;
                        case TrayType.ElcOutBound:
                            transactionsIds = transactionRepository.GetELcOutBoundIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id) &
                                    !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft &
                                    (s.Transaction.TransactionCategoryId == InternalOutbound);

                            break;
                        case TrayType.SentTransactions:
                            where = s => (
                            (s.FromEntityId == s.ToEntityId & s.FromUserId != s.ToUserId) | (s.FromEntityId != s.ToEntityId)) &
                            s.FromEntityId == OrgUnitId &
                            s.FromUserId == User.Id &
                            (s.TrayId == (int)TrayType.MyTransactions | s.TrayId == (int)TrayType.OrgUnit | s.TrayId == (int)TrayType.DraftOutbound)
                             && s.Transaction.StatusId == InProcess;

                            //Yousef TODO
                            //transactionsIds = transactionRepository.GetSentTransactionsIds(userId, OrgUnitId);
                            //where = s => transactionsIds.Contains(s.Transaction.Id) &
                            //        (s.TrayId == (int)TrayType.MyTransactions | s.TrayId == (int)TrayType.OrgUnit | s.TrayId == (int)TrayType.DraftOutbound)
                            //        && s.Transaction.StatusId == InProcess;



                            break;
                        case TrayType.Saved:

                            where = s =>
                                     s.TrayId == trayId &
                                     s.ToEntityId == OrgUnitId &
                                     //s.ToUserId == userId &
                                     (s.Transaction.StatusId == TempSave | s.Transaction.StatusId == Completed) &
                                     s.Transaction.TransactionCategoryId != ExternalOutbound;

                            break;
                        case TrayType.OrgUnit:

                            where = s =>
                                       s.ToUser == null &
                                       s.TrayId == trayId &
                                       s.ToEntityId == OrgUnitId &
                                       (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == MultiOwnership) &
                                       s.Transaction.TransactionCategoryId != ExternalOutbound & !s.Transaction.NeedAcknowled;
                            break;
                        case TrayType.Manager:
                            where = s => s.ToEntityId == OrgUnitId &
                                        s.ToUserId != userId &
                                       (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == MultiOwnership) &
                                       s.Transaction.TransactionCategoryId != ExternalOutbound &
                                       s.TrayId == (int)TrayType.MyTransactions;
                            break;
                        case TrayType.Copies:
                            where = (s =>
                        s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                            (tc.UserId == userId | tc.UserId == null) &
                            tc.IsSent == 1 & tc.Status != DeletedCopy & !tc.SpecialCopy &
                            !tc.Transaction.IsDeleted & tc.Status != ViewedCopy).Any() & s.Transaction.TransactionCategoryId == Inbound);
                            break;
                        case TrayType.InternalInboundCopies:
                            where = (s =>
                         s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                             (tc.UserId == userId | tc.UserId == null) &
                             tc.IsSent == 1 & tc.Status != DeletedCopy & !tc.SpecialCopy &
                             !tc.Transaction.IsDeleted & tc.Status != ViewedCopy).Any() & s.Transaction.TransactionCategoryId == InternalOutbound);
                            break;
                        case TrayType.CopiesOutbound:
                            where = (s =>
                       s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                           (tc.UserId == userId | tc.UserId == null) &
                           tc.IsSent == 1 & tc.Status != DeletedCopy & !tc.SpecialCopy &
                           !tc.Transaction.IsDeleted & tc.Status != ViewedCopy).Any() & s.Transaction.TransactionCategoryId == ExternalOutbound);
                            break;
                        case TrayType.SpecialCopies:
                            where = (s =>
                       s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                           (tc.UserId == userId | tc.UserId == null) &
                           tc.IsSent == 1 & tc.Status != DeletedCopy & tc.SpecialCopy &
                           !tc.Transaction.IsDeleted & tc.Status != ViewedCopy).Any());
                            break;
                        case TrayType.SavedCopies:
                            {
                                transactionsIds = transactionRepository.GetSavedCopiesIds(userId, OrgUnitId);
                                where = (s => transactionsIds.Contains(s.Transaction.Id));
                            }
                            break;
                        case TrayType.YESSER:
                            where = s =>
                                     s.ToUserId == userId &
                                     s.TrayId == trayId &
                                     s.ToEntityId == OrgUnitId &
                                     (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == MultiOwnership) &
                                     s.Transaction.TransactionCategoryId != ExternalOutbound;
                            break;
                        case TrayType.Tasks:
                            transactionsIds = transactionRepository.GetUserTasksTransactionsIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id) &
                                    s.Transaction.TransactionCategoryId != ExternalOutbound;
                            break;
                        case TrayType.FollowUp:
                            transactionsIds = transactionRepository.GetUserFollowUpTransactionsIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpUnderProcess:
                            transactionsIds = transactionRepository.GetUserFollowProcessIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpComplete:
                            transactionsIds = transactionRepository.GetUserFollowCompleteIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpCanceld:
                            transactionsIds = transactionRepository.GetUserFollowDeleteIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpEscalation:
                            transactionsIds = transactionRepository.GetUserFollowUpEscalationIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpReminder:
                            transactionsIds = transactionRepository.GetUserFollowReminderIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpLate:
                            transactionsIds = transactionRepository.GetUserFollowLateIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.Reservation:
                            where = s =>
                                     s.ToUserId == userId &
                                     //s.TrayId == trayId &
                                     s.ToEntityId == OrgUnitId &
                                     (s.Transaction.TransactionCategoryId == Inbound || s.Transaction.TransactionCategoryId == ExternalOutbound) &
                                     (s.Transaction.StatusId == Reserved | s.Transaction.StatusId == MultiOwnership);
                            break;
                    }

                    switch (transactionDateType)
                    {
                        case TransactionDateType.Today:
                            {
                                if (trayType == TrayType.SentTransactions)
                                {
                                    where = ExpressionUtility.AndAlso(where, ts =>
                                         ts.ModefiedOn.Value.Year == DateTime.Now.Year &
                                         ts.ModefiedOn.Value.Day == DateTime.Now.Day &
                                         ts.ModefiedOn.Value.Month == DateTime.Now.Month);
                                }
                                else
                                {
                                    where = ExpressionUtility.AndAlso(where, ts =>
                                   ts.Date.Year == DateTime.Now.Year &
                                   ts.Date.Day == DateTime.Now.Day &
                                   ts.Date.Month == DateTime.Now.Month);
                                }

                            }
                            break;
                        case TransactionDateType.Late:
                            {
                                IUserManagementBL userManagementBL = new UserManagementBL();

                                UserProfile userProfile = userManagementBL.GetUserById(userId);

                                DateTime date = DateTime.Now.Date;

                                if (userProfile.TransactionProcessingPeriod > 0)
                                {
                                    date = DateTime.Now.AddDays(-userProfile.TransactionProcessingPeriod);
                                }
                                where = ExpressionUtility.AndAlso(where, ts =>
                                ts.Transaction.RemindDate < date ||
                              (ts.TransactionAssignmentProcessPeriod == null && (
                                ts.Date < date)) ||
                                (ts.TransactionAssignmentProcessPeriod != null && (
                                ts.TransactionAssignmentProcessPeriod < date))

                               );
                            }
                            break;
                        case TransactionDateType.HasDate:
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                             ts.Transaction.RemindDate != null
                             );
                            }
                            break;
                        case TransactionDateType.Decisions:
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                             ts.Transaction.TransactionTypeId == 4
                             );
                            }
                            break;

                        case TransactionDateType.Circulars:
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                             ts.Transaction.LetterTypeId == 3
                             );
                            }
                            break;
                        case TransactionDateType.SublimeMatter:
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                             ts.Transaction.LetterTypeId == 58
                             );
                            }
                            break;
                    }

                    int count = 0;
                    if (where != null)
                    {
                        count = transactionAssignmentRepository.GetTransactionAssignmentCount(where);
                    }
                    return count;
                }

                return 0;
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

        public TransactionAssignment GetTransactionAssignmentById(int transactionAssignmentId)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                return transactionAssignmentRepository.Get(transactionAssignmentId);
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

        public TransactionAssignment GetTransactionAssignment(int userId, int transactionID)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetTransactionAssignment(ts => ts.ToUser.Id == User.Id & ts.Transaction.Id == transactionID);
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

        public IList<TransactionAssignment> GetTransactionAssignments(int transactionId, string cultureName)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetTransactionAssignments(transactionId, cultureName);
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

        public TransactionAssignment GetLastTransactionAssignments(int transactionId, string cultureName)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetLastTransactionAssignments(transactionId, cultureName);
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

        public IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> @where)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetTransactionAssignments(@where);
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

        public IList<TransactionAssignment> GetTransactionAssignments(Expression<Func<TransactionAssignment, bool>> @where, string cultureName)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetTransactionAssignments(@where, cultureName);
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

        public IList<TransactionAssignment> GetTransactionAssignments(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount, TrayType trayType, int? transactionDate)
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

                Expression<Func<TransactionAssignment, bool>> where = null;

                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                if (trayType == TrayType.SentTransactions)
                {
                    Expression<Func<TransactionAssignment, bool>> sentTransactionsWhere = null;

                    sentTransactionsWhere = a => ((a.FromUserId != a.ToUserId) || (a.FromUserId == a.ToUserId && a.FromEntityId != a.ToEntityId))
                       && a.FromEntityId == OrgUnitId && a.FromUserId == User.Id &&
                       (a.TrayId == (int)TrayType.MyTransactions | a.TrayId == (int)TrayType.OrgUnit | a.TrayId == (int)TrayType.DraftOutbound)
                       && a.Transaction.StatusId == InProcess
                       && !a.IsHidden
                       && a.Transaction.TransactionCategoryId != ExternalOutbound;


                    if (transactionDate.HasValue && transactionDate.Value == (int)TransactionDateType.Today)
                    {
                        sentTransactionsWhere = ExpressionUtility.AndAlso<TransactionAssignment>(sentTransactionsWhere, ts =>
                                   ts.ModefiedOn.Value.Year == DateTime.Now.Year &
                                   ts.ModefiedOn.Value.Day == DateTime.Now.Day &
                                   ts.ModefiedOn.Value.Month == DateTime.Now.Month);
                    }


                    return transactionAssignmentRepository.GetTransactionAssignments(sentTransactionsWhere, searchCriteria, out rowsCount, userWeight, User.Id);
                }

                if (trayType == TrayType.Manager)
                {

                    where = a => a.ToEntity.Id == OrgUnitId && a.Transaction.StatusId == InProcess
                   && a.Transaction.TransactionCategoryId != ExternalOutbound
                   && a.Transaction.TransactionCategoryId != DraftOutbound
                    && a.ToUser != null && a.ToUser.Id != User.Id;

                }
                else
                {
                    where = a => a.ToEntity.Id == OrgUnitId && a.Transaction.StatusId == InProcess
                  && a.Transaction.TransactionCategoryId != ExternalOutbound
                  && a.Transaction.TransactionCategoryId != DraftOutbound
                   && a.ToUser != null;

                }



                if (transactionDate.HasValue && transactionDate.Value == (int)TransactionDateType.Today)
                {
                    where = ExpressionUtility.AndAlso<TransactionAssignment>(where, ts =>
                               ts.ModefiedOn.Value.Year == DateTime.Now.Year &
                               ts.ModefiedOn.Value.Day == DateTime.Now.Day &
                               ts.ModefiedOn.Value.Month == DateTime.Now.Month);
                }

                return transactionAssignmentRepository.GetAssignments(where, searchCriteria, out rowsCount, userWeight, User.Id);
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

        public IList<TransactionAssignmentInfo> GetTransactionAssignmentsInfo(int transactionId, string cultureName)
        {
            try
            {
                IList<TransactionAssignmentInfo> transactionAssignmentInfos = new List<TransactionAssignmentInfo>();
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                IList<TransactionAssignment> transactionAssignments =
                    transactionAssignmentRepository.GetTransactionAssignments(transactionId, cultureName);

                foreach (TransactionAssignment transactionAssignment in transactionAssignments)
                {
                    transactionAssignmentInfos.Add(MapTransactionAssignment(transactionAssignment, cultureName));
                }

                return transactionAssignmentInfos;
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

        public static bool IsMultiOwnerShip(int transactionId)
        {
            try
            {
                int SendCopyToView = ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty);
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                return (transactionAssignmentRepository.GetTransactionAssignments(s => s.TransactionId ==
                    transactionId && s.ActionId != SendCopyToView).Count > 1);
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

        public void RevertAssignByTransaction(int transactionId, int OrgUnitId, int trayId)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                TransactionAssignment transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment
                    (
                    ts =>
                        ts.TransactionId == transactionId &
                        ts.FromEntityId == OrgUnitId &
                        ts.FromUserId == User.Id &
                       (ts.TrayId == (int)TrayType.MyTransactions ||
                       ts.TrayId == (int)TrayType.DraftOutbound ||
                       ts.TrayId == (int)TrayType.SentTransactions ||
                        ts.TrayId == (int)TrayType.OrgUnit) &
                        ts.Viewed == false
                      );

                //Change transaction Assignment to SentTransactions just in case Revert transaction from tray org
                //if (transactionAssignment.TrayId == (int)TrayType.OrgUnit)
                //{
                //    transactionAssignment.TrayId = (int)TrayType.SentTransactions;
                //}
                if (transactionAssignment.TrayId == (int)TrayType.OrgUnit && transactionAssignment.Transaction.TransactionCategory.Id == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                {
                    transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
                }
                if (transactionAssignment.TrayId == (int)TrayType.OrgUnit && transactionAssignment.Transaction.TransactionCategory.Id == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                {
                    transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                }
                if (transactionAssignment.TrayId == (int)TrayType.OrgUnit && transactionAssignment.Transaction.TransactionCategory.Id == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                {
                    transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                }
                transactionAssignment.ActionId = (int)ActionTransactionType.Withdraw;
                PreRevertAssignTransaction(transactionAssignment, OrgUnitId, trayId);

                OnRevertAssignTransaction(transactionAssignment);

                PostRevertAssignTransaction(transactionAssignment, (int)TrayActionType.Revert);

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
        public void RevertReject(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                TransactionAssignment transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment
                    (
                    ts =>
                        ts.TransactionId == transactionId &
                        ts.ToEntityId == OrgUnitId &
                        (ts.ToUserId == User.Id || ts.ToUserId == null) //&
                                                                        // ts.TrayId == trayId
                      );

                if (transactionAssignment.FromUserId == transactionAssignment.ToUserId)
                {
                    throw new BusinessException(StatusCode.CantReturnToSelf);
                }

                transactionAssignment.ToUserId = transactionAssignment.FromUserId;
                transactionAssignment.ToEntityId = transactionAssignment.FromEntityId;
                transactionAssignment.FromUserId = User.Id;
                transactionAssignment.FromEntityId = OrgUnitId;
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionAssignment.Date);
                transactionAssignment.Description = remarks;
                transactionAssignment.CurrentPathStep = transactionAssignment.CurrentPathStep.HasValue ? transactionAssignment.CurrentPathStep - 1 : transactionAssignment.CurrentPathStep;
                transactionAssignment.Viewed = false;
                transactionAssignment.TrayId = trayId;
                transactionAssignment.ActionId = (int)ActionTransactionType.Rejected;
                switch (transactionAssignment.Transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty))
                {
                    case (int)TransactionCategory.Inbound:
                    case (int)TransactionCategory.InternalOutbound:
                        {
                            transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                            break;
                        }
                    case (int)TransactionCategory.DraftOutbound:
                        {
                            transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
                            break;
                        }
                }

                OnRevertAssignTransaction(transactionAssignment);

                PostRevertAssignTransaction(transactionAssignment, null);

                SendTrayActionNotification(transactionAssignment, NotificationSource.RevertRejectTransaction, NotificationTemplateType.RevertRejectTransactionWeb,
                    NotificationTemplateType.RevertRejectTransactionEmail, NotificationEmailSubject.RevertRejectTransactionEmail,
                    NotificationWebSubject.RevertRejectTransaction, NotificationType.Web, (TrayType)trayId, cultureName);

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

        public void RevertRejectToCreator(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                TransactionAssignment transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment
                    (
                    ts =>
                        ts.TransactionId == transactionId &
                        ts.ToEntityId == OrgUnitId &
                        (ts.ToUserId == User.Id || ts.ToUserId == null) &
                        ts.TrayId == trayId
                      );

                if (transactionAssignment.FromUserId == transactionAssignment.ToUserId)
                {
                    throw new BusinessException(StatusCode.CantReturnToSelf);
                }

                transactionAssignment.ToUserId = transactionAssignment.Transaction.UserId;
                transactionAssignment.ToEntityId = transactionAssignment.Transaction.OrgUnitId;
                transactionAssignment.FromUserId = User.Id;
                transactionAssignment.FromEntityId = OrgUnitId;
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionAssignment.Date);
                transactionAssignment.Description = remarks;
                transactionAssignment.CurrentPathStep = null;
                transactionAssignment.Viewed = false;
                transactionAssignment.ActionId = (int)ActionTransactionType.Rejected;

                switch (transactionAssignment.Transaction.TransactionCategoryId)
                {
                    case (int)TransactionCategory.Inbound:
                    case (int)TransactionCategory.InternalOutbound:
                        {
                            transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                            break;
                        }
                    case (int)TransactionCategory.DraftOutbound:
                        {
                            transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
                            break;
                        }
                }

                OnRevertAssignTransaction(transactionAssignment);

                PostRevertAssignTransaction(transactionAssignment, null);

                SendTrayActionNotification(transactionAssignment, NotificationSource.RevertRejectTransaction, NotificationTemplateType.RevertRejectTransactionWeb,
                    NotificationTemplateType.RevertRejectTransactionEmail, NotificationEmailSubject.RevertRejectTransactionEmail,
                    NotificationWebSubject.RevertRejectTransaction, NotificationType.Web, (TrayType)trayId, cultureName);

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

        public void RevertAssignById(int assignmentId, int OrgUnitId)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

                TransactionAssignment transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts => ts.Id == assignmentId);

                if (transactionAssignment.ToUser.Id == User.Id)
                {
                    throw new BusinessException(StatusCode.RevertAssignmentToYou);
                }

                transactionAssignment.ToUserId = User.Id;
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignment);
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

        public static TransactionAssignmentInfo MapTransactionAssignment(TransactionAssignment transactionAssignment, string cultureName)
        {
            try
            {
                TransactionAssignmentInfo transactionAssignmentInfo = new TransactionAssignmentInfo()
                {
                    Id = transactionAssignment.Id,
                    Action = (transactionAssignment.Action != null) ? transactionAssignment.Action.LocalName : null,
                    ActionId = (transactionAssignment.ActionId != null) ? transactionAssignment.ActionId : null,
                    Date = transactionAssignment.Date,
                    DateH = transactionAssignment.DateH,
                    FromEntity = (transactionAssignment.FromEntity != null) ? transactionAssignment.FromEntity.LocalName : null,
                    FromEntityId = transactionAssignment.FromEntityId,
                    FromUser = (transactionAssignment.FromUser != null) ? transactionAssignment.FromUser.LocalName : null,
                    FromUserId = transactionAssignment.FromUserId,
                    ToEntity = (transactionAssignment.ToEntity != null) ? transactionAssignment.ToEntity.LocalName : null,
                    ToEntityId = (transactionAssignment.ToEntityId),
                    ToUser = (transactionAssignment.ToUser != null) ? transactionAssignment.ToUser.LocalName : null,
                    ToUserId = (transactionAssignment.ToUser != null) ? transactionAssignment.ToUser.Id : -1,
                    TransactionId = transactionAssignment.TransactionId,
                    IsLate = false,
                    Viewed = transactionAssignment.Viewed,
                    Description = transactionAssignment.Description,
                };

                return transactionAssignmentInfo;
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

        public static TransactionAssignmentInfo MapTransactionFollowUp(TransactionAssignment transactionAssignment, string cultureName)
        {
            try
            {
                TransactionAssignmentInfo transactionAssignmentInfo = new TransactionAssignmentInfo()
                {
                    Id = transactionAssignment.Id,
                    Action = (transactionAssignment.Action != null) ? transactionAssignment.Action.LocalName : null,
                    ActionId = (transactionAssignment.ActionId != null) ? transactionAssignment.ActionId : null,
                    Date = transactionAssignment.Date,
                    DateH = transactionAssignment.DateH,
                    FromEntity = (transactionAssignment.FromEntity != null) ? transactionAssignment.FromEntity.LocalName : null,
                    FromEntityId = transactionAssignment.FromEntityId,
                    FromUser = (transactionAssignment.FromUser != null) ? transactionAssignment.FromUser.LocalName : null,
                    FromUserId = transactionAssignment.FromUserId,
                    ToEntity = (transactionAssignment.ToEntity != null) ? transactionAssignment.ToEntity.LocalName : null,
                    ToEntityId = (transactionAssignment.ToEntityId),
                    ToUser = (transactionAssignment.ToUser != null) ? transactionAssignment.ToUser.LocalName : null,
                    ToUserId = (transactionAssignment.ToUser != null) ? transactionAssignment.ToUser.Id : -1,
                    TransactionId = transactionAssignment.TransactionId,
                    IsLate = false,
                    Viewed = transactionAssignment.Viewed,
                    Description = transactionAssignment.Description,
                };

                return transactionAssignmentInfo;
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

        public void Assign(int transactionId, int OrgUnitId, string cultureName)
        {
            try
            {
                if (!User.HasClaim(UserClaims.Assignments.AssignTo))
                {
                    throw new BusinessException(StatusCode.PermissionAssignTo);
                }
                INotificationBL notificationBL = IoC.Resolve<INotificationBL>();
                ITransactionAssignmentRepository transactionAssignmentRepository =
                    IoC.Resolve<ITransactionAssignmentRepository>();

                TransactionAssignment transactionAssignment = transactionAssignmentRepository
                                                              .GetTransactionAssignment(ts => ts.ToUserId == null
                                                                                        & ts.TransactionId == transactionId
                                                                                        & ts.ToEntityId == OrgUnitId
                                                                                        & ts.TrayId == (int)TrayType.OrgUnit);

                PreAssign(transactionAssignment, OrgUnitId);

                OnAssign(transactionAssignment);

                PostAssign(transactionAssignment);

                if ((DeliveryMethodType)transactionAssignment.DeliveryMethodId.LookupInternalID(LookupCategory.DeliveryMethod, cultureName) == DeliveryMethodType.ElectronicPaper)
                {
                    IUserManagementBL userManagementBL = new UserManagementBL();

                    NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(transactionAssignment.FromUserId, cultureName);

                     if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ReceiveReport))
                    {
                        var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(transactionAssignment.FromUserId) };
                        notificationBL.SendTransactionNotification(transactionAssignment.Transaction, NotificationSource.TransactionAssignment, NotificationTemplateType.ReceiveReportWeb,
                            NotificationTemplateType.ReceiveReportEmail, NotificationEmailSubject.ReceiveReportEmail, NotificationWebSubject.ReceiveReport,
                            notificationUsers, cultureName);
                    }
                }
                else
                {
                    SendTrayActionNotification(transactionAssignment, NotificationSource.AssignTransaction, NotificationTemplateType.AssignTransactionWeb,
                        NotificationTemplateType.AssignTransactionEmail, NotificationEmailSubject.AssignTransactionEmail, NotificationWebSubject.AssignTransaction,
                        NotificationType.Web, TrayType.MyTransactions,
                        cultureName);
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

        public void AssignTransaction(IList<Transaction> transactions, IList<TransactionAssignment> transactionAssignments, string culturName = "ar")
        {
            try
            {
                if (!User.HasClaim(UserClaims.Assignments.Assign))
                {
                    throw new BusinessException(StatusCode.PermissionAssignments);
                }
               
                foreach (Transaction transaction in transactions)
                {

                    transactionAssignments.ToList().ForEach(a =>
                    {
                        a.TransactionId = transaction.Id;
                    });

                    PreAssignTransaction(transaction, transactionAssignments);

                    OnAssignTransaction(transaction, transactionAssignments);

                    PostAssignTransaction(transaction, transactionAssignments, culturName);
                   
                }
            }
            catch (BusinessException ex)
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

        public void AssignTransactionWithdrawal(IList<Transaction> transactions, IList<TransactionAssignment> transactionAssignments, string culturName = "ar")
        {
            try
            {
                if (!User.HasClaim(UserClaims.Assignments.Assign))
                {
                    throw new BusinessException(StatusCode.PermissionAssignments);
                }

                foreach (Transaction transaction in transactions)
                {

                    transactionAssignments.ToList().ForEach(a =>
                    {
                        a.TransactionId = transaction.Id;
                    });

                    //PreAssignTransaction(transaction, transactionAssignments);

                    OnAssignTransaction(transaction, transactionAssignments);

                    PostAssignTransaction(transaction, transactionAssignments, culturName);
                }
            }
            catch (BusinessException ex)
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


        public int AddAssignTransactionHistory(TransactionAssignment transactionAssignment)
        {
            try
            {
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
                return transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);
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

        protected virtual void PreRevertAssignTransaction(TransactionAssignment transactionAssignment, int OrgUnitId, int trayId)
        {
            if (!User.HasClaim(UserClaims.Assignments.WithdrawTransaction))
            {
                throw new BusinessException(StatusCode.PermissionAssignmentsWithdrawTransaction);
            }

            if (transactionAssignment == null)
            {
                throw new BusinessException(StatusCode.TransactionNotFound);
            }

            transactionAssignment.ToUserId = User.Id;
            transactionAssignment.ToEntityId = OrgUnitId;
            transactionAssignment.Date = DateTime.Now;
            transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionAssignment.Date);

            if (transactionAssignment.TrayId == (int)TrayType.SentTransactions)
            {
                switch (transactionAssignment.Transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionStatus, string.Empty))
                {
                    case (int)TransactionCategory.Inbound:
                    case (int)TransactionCategory.InternalOutbound:
                        {
                            transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                            break;
                        }
                    case (int)TransactionCategory.DraftOutbound:
                        {
                            transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
                            break;
                        }
                }
            }
        }

        protected virtual void OnRevertAssignTransaction(TransactionAssignment transactionAssignment)
        {
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
            transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignment);
        }

        protected void PostRevertAssignTransaction(TransactionAssignment transactionAssignment, int? trayActionType)
        {
            int transactionAssignmentHistoryId = AddAssignTransactionHistory(transactionAssignment);
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            var oldTransactionDeliveryReport = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transactionAssignment.TransactionId).LastOrDefault();
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            if (oldTransactionDeliveryReport != null)
            {
                oldTransactionDeliveryReport.TransactionAssignmentHistoryId = transactionAssignmentHistoryId;
                transactionDeliveryReportBL.UpdateTransactionDeliveryReport(oldTransactionDeliveryReport);
            }
            if (trayActionType != null)
            {
                transactionRepository.UpdateTransactionStatusAndEntityByTransId(transactionAssignment.TransactionId, TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), transactionAssignment.ToEntityId, transactionAssignment.ToUserId);

            }
            else
            {
                transactionRepository.UpdateTransactionStatusAndEntityByTransId(transactionAssignment.TransactionId, TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), transactionAssignment.ToEntityId, transactionAssignment.ToUserId);

            }
        }

        private void ValidateTransactionAssignment(TransactionAssignment transactionAssignment)
        {
            if (transactionAssignment.ActionId <= 0)
            {
                throw new BusinessException(StatusCode.InvalidActionType);
            }

            if (transactionAssignment.FromEntityId <= 0 || transactionAssignment.ToEntityId <= 0)
            {
                throw new BusinessException(StatusCode.InvalidOrgUnit);
            }
        }
        private void PreAssign(TransactionAssignment transactionAssignment, int OrgUnitId)
        {
            if (transactionAssignment == null)
            {
                throw new BusinessException(StatusCode.TransactionNotFound);
            }

            transactionAssignment.ToUserId = User.Id;
            transactionAssignment.ToEntityId = OrgUnitId;
            transactionAssignment.Date = DateTime.Now;
            transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionAssignment.Date);
            if (transactionAssignment.Transaction.TransactionCategory.Id == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
            {
                transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
            }
            else
            {
                transactionAssignment.TrayId = (int)TrayType.MyTransactions;
            }
        }

        private void OnAssign(TransactionAssignment transactionAssignment)
        {
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
            transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignment);
            TransactionAssignment sentTransactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts =>
                            ts.TransactionId == transactionAssignment.TransactionId &
                            ts.FromEntityId == transactionAssignment.FromEntityId &
                            ts.FromUserId == transactionAssignment.FromUserId &
                            ts.TrayId == (int)TrayType.SentTransactions);


            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();

            IList<TransactionAssignmentHistory> transactionAssignmentHistories = transactionAssignmentHistoryBL.GetTransactionAssignmentHistoryByTransactionId(transactionAssignment.TransactionId);

            if (transactionAssignmentHistories.Count > 1)
            {
                ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                transactionBL.SetTransactionCopiesSent(transactionAssignment.TransactionId);
            }

            if (sentTransactionAssignment != null)
            {
                transactionAssignmentRepository.DeleteTransactionAssignment(sentTransactionAssignment.Id);
            }

            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            transactionRepository.UpdateTransactionStatusAndEntityByTransId(transactionAssignment.TransactionId, TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), transactionAssignment.ToEntityId, transactionAssignment.ToUserId);
        }

        private void PostAssign(TransactionAssignment transactionAssignment)
        {
            AddAssignTransactionHistory(transactionAssignment);
        }

        private void PreAssignTransaction(Transaction transaction, IList<TransactionAssignment> transactionAssignments)
        {
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
            if (transactionAssignments == null || transactionAssignments.Count == 0)
            {
                throw new BusinessException(StatusCode.TransactionAssignmentsCannotBeNullOrEmpty);
            }
            TransactionAssignment transactionAssignmentOld = transactionAssignmentRepository.GetTransactionAssignment(ts => ts.TransactionId == transaction.Id);
            if (transactionAssignmentOld.ToUserId != null)
            {
                IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                var fromEntityId = transactionAssignments.FirstOrDefault().FromEntityId;

                if (orgUnitBL.ValidateManagerCanAssign(fromEntityId, User.Id, transaction.Id, transactionAssignmentOld.ToUserId.Value, User.HasClaim(UserClaims.Files.Manager)))
                {
                    if (transactionAssignmentOld.ToEntityId != fromEntityId)
                    {
                        throw new BusinessException(StatusCode.TransactionNotFound);
                    }
                }
                else
                {
                    if (transactionAssignmentOld.ToUserId != User.Id || transactionAssignmentOld.ToEntityId != fromEntityId)
                    {
                        throw new BusinessException(StatusCode.TransactionNotFound);
                    }
                }


                //List<int> ids = new List<int>();
                //ids.Add(transaction.Id);
                //bool hasViewPermission = TransactionBL.CheckUserHasPermission(ids, transactionAssignments.FirstOrDefault().ToUserId);

                //if (!hasViewPermission && transactionAssignments.FirstOrDefault().ToUserId.HasValue)
                //{
                //    if (!User.HasClaim(UserClaims.GeneralPermissions.AssignTransactionToUnauthorize))
                //    {
                //        throw new BusinessException(StatusCode.UserNotAuthorised);
                //    }

                //    TransactionBL.AddTransactionSpecialAuthorize(transaction.Id, transactionAssignments.FirstOrDefault().ToUserId.Value);
                //}



            }
            else
            {
                //if (transactionAssignmentOld.ToEntityId != transactionAssignments.FirstOrDefault().FromEntityId)
                //{
                //    throw new BusinessException(StatusCode.TransactionNotFound);
                //}
            }
        }

        private void OnAssignTransaction(Transaction transaction, IList<TransactionAssignment> transactionAssignments)
        {
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
            ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();
            IUserManagementBL userPreferenceBL = new UserManagementBL();
            IActionBL actionBL = IoC.Resolve<IActionBL>();
            string cultureName = string.Empty;
            int OrgUnitId = transactionAssignments.FirstOrDefault().FromEntityId;

            AssignmentFlag assignmentFlag = AssignmentFlag.None;

            int assignmentsNotAsCopyCount = 0;
            transaction.ProcessPeriodTransaction = 0;
            if (transaction.TransactionCategoryId != Common.TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName) && !transaction.NeedAcknowled)
            {

                foreach (TransactionCopy transactionCopy in transaction.Copies)
                {
                    transactionCopy.IsSent = 1;
                    transactionCopy.SentDate = DateTime.Now;
                }

            }

            foreach (TransactionAssignment transactionAssignment in transactionAssignments)
            {
                ValidateTransactionAssignment(transactionAssignment);

                if (transactionAssignment.FromEntityId != transactionAssignment.ToEntityId && transactionAssignment.ToUserId == null && !User.HasClaim(UserClaims.Assignments.AssignToOtherDepartment))
                {
                    throw new BusinessException(StatusCode.PermissionAssignToOtherDepartment);
                }

                if (transactionAssignment.FromEntityId != transactionAssignment.ToEntityId && transactionAssignment.ToUserId != null && !User.HasClaim(UserClaims.Assignments.AssignToEmployeeInOtherDepartment))
                {
                    throw new BusinessException(StatusCode.PermissionAssignToEmployeeInOtherDepartment);
                }
                TransactionAssignmentHistoryBL lateTransactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                TransactionAssignmentHistory lastTransactionAssignmentHistory = lateTransactionAssignmentHistoryBL.GetLastTransactionAssignmentHistory(transaction.Id);
                if (lastTransactionAssignmentHistory.UserDelegationId != null)
                {
                    UserDelegation userDelegation = userPreferenceBL.GetUserDelegationById(lastTransactionAssignmentHistory.UserDelegationId.Value, "ar");
                    if (userDelegation != null && userDelegation.UserProfileId == User.Id)
                    {
                        transactionAssignment.UserDelegationId = lastTransactionAssignmentHistory.UserDelegationId.Value;
                    }
                }
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                transactionAssignment.FromUserId = User.Id;
                int TransactionProcessingPeriod = 0;
                if (transactionAssignment.ToUserId.HasValue)
                {
                    UserPreference userPreference =
                        userPreferenceBL.GetUserPreferenceByUserId(transactionAssignment.ToUserId.Value);
                    if (userPreference != null)
                    {
                        TransactionProcessingPeriod = userPreference.UserProfile.TransactionProcessingPeriod;
                    }
                    int Approved = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, string.Empty);
                    if (userPreference != null /*&& userPreference.IsDelegationEnabled*/)
                    {
                        UserDelegation userDelegation = userPreference.UserDelegations.Where(d =>
                                                              d.FromDate < transactionAssignment.Date &&
                                                              d.ToDate > transactionAssignment.Date &&
                                                              d.StatusId == Approved
                                                        ).FirstOrDefault();
                        if (userDelegation != null)
                        {
                            //Send Copy For User who Delegat

                            List<int> allowedTransactionCategories = new List<int>();
                            if (!string.IsNullOrWhiteSpace(userDelegation.TransacionCategoryIds))//256
                            {
                                allowedTransactionCategories = userDelegation.TransacionCategoryIds.Split(',').ToList().Select(x => int.Parse(x)).ToList();
                            }

                            List<int> allowedTransacionConfidentialities = new List<int>();
                            if (!string.IsNullOrWhiteSpace(userDelegation.TransacionConfidentialityIds))
                            {
                                allowedTransacionConfidentialities = userDelegation.TransacionConfidentialityIds.Split(',').ToList().Select(x => int.Parse(x)).ToList();
                            }

                            if (allowedTransactionCategories.Contains(transactionAssignment.Transaction.TransactionCategoryId) &&
                                allowedTransacionConfidentialities.Contains(transactionAssignment.Transaction.ConfidentialityId))
                            {
                                if (userDelegation.ReceiveCopy == true)
                                {

                                    SettingBL settingBL = new SettingBL();
                                    List<Setting> settings = settingBL.GetSettingByKey(Constants.GeneralSettings.SelectAction);
                                    Setting setting = settings.Find(a => a.Key == Constants.GeneralSettings.SelectAction);

                                    TransactionCopy transactionCopy = new TransactionCopy();
                                    transactionCopy.UserId = transactionAssignment.ToUserId;
                                    transactionCopy.EntityId = transactionAssignment.ToEntityId;
                                    transactionCopy.FromUserId = transactionAssignment.FromUserId;
                                    transactionCopy.FromEntityId = transactionAssignment.FromEntityId;
                                    transactionCopy.TransactionId = transaction.Id;
                                    transactionCopy.Date = DateTime.Now;
                                    transactionCopy.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                                    transactionCopy.Status = TransCopyStatus.NotViewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                                    transactionCopy.ActionId = Convert.ToInt32(setting.Value);
                                    transactionCopy.IsSent = 1;
                                    transactionCopy.SentDate = DateTime.Now;
                                    transaction.Copies.Add(transactionCopy);
                                }

                                transactionAssignment.UserDelegationId = userDelegation.Id;
                                transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                                TransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                                transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

                                transactionAssignment.ToUserId = userDelegation.UserProfileId;
                                transactionAssignment.ToEntityId = userDelegation.OrgUnitId;
                            }
                        }
                    }
                }

                Domain.Action action = actionBL.GetActionById(transactionAssignment.ActionId.Value);

                if (transactionAssignment.ToUserId.HasValue)
                {
                    if (transaction.TransactionCategory.Id == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                    {
                        transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
                    }
                    else
                    {
                        transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                    }

                    assignmentFlag = assignmentFlag | AssignmentFlag.SentToUser;
                }
                else if (!transactionAssignment.ToUserId.HasValue && transaction.TransactionCategory.Id != TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                {
                    transactionAssignment.TrayId = (int)TrayType.OrgUnit;

                    assignmentFlag = assignmentFlag | AssignmentFlag.SentToOrgUnit;
                }


                if (transaction.TransactionCategory.Id == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                {


                    TransactionElcOutBound transactionElcOutBound = new TransactionElcOutBound();
                    transactionElcOutBound.TransactionId = transaction.Id;
                    transactionElcOutBound.EntityId = transactionAssignment.ToEntityId;
                    transactionElcOutBound.UserId = transactionAssignment.ToUserId;
                    transactionElcOutBound.Ishidden = false;
                    transactionElcOutBound.CreatedOn = DateTime.Now;
                    transactionElcOutBound.CreatedBy = transactionAssignment.FromUserId;

                    TransactionBL.TransactionElcOutBoundAdd(transactionElcOutBound);


                }
                assignmentsNotAsCopyCount++;

                transactionAssignment.Action = action;

                bool isMutliOwnership = assignmentsNotAsCopyCount > 1;

                if (isMutliOwnership)
                {
                    ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                    transaction.StatusId = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);

                    transactionBL.Update(transaction);
                }
                else
                {

                    ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                    transactionRepository.UpdateTransactionStatusAndEntityByTransId(transactionAssignment.TransactionId, TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty), transactionAssignment.ToEntityId, transactionAssignment.ToUserId);
                 }

                TransactionAssignment transactionAssignmentOld = transactionAssignmentRepository.GetTransactionAssignment(ts => ts.TransactionId == transaction.Id);

                int transactionPathDetailsCount = 0;
                if (transactionAssignmentOld.TransactionPathId.HasValue)
                {
                    transactionPathDetailsCount = transactionAssignmentRepository.GetTransactionPathCount(transactionAssignmentOld.TransactionPathId.Value);
                }

                if (transactionAssignmentOld != null)
                {
                    int tasksCount = 0;

                    if (transactionAssignmentOld != null && transactionAssignmentOld.Tasks != null)
                    {
                        int InProcess = TaskStatus.InProcess.LookupIdentity(LookupCategory.TaskStatus, string.Empty);
                        tasksCount = transactionAssignmentOld.Tasks.Where(t => t.StatusId == InProcess).Count();
                    }

                    if (tasksCount > 0)
                    {
                        throw new BusinessException(StatusCode.AssignmentTasksNotCompleted);
                    }
                    transactionAssignmentOld.FromEntityId = transactionAssignment.FromEntityId;
                    transactionAssignmentOld.FromUserId = transactionAssignment.FromUserId;
                    transactionAssignmentOld.Description = transactionAssignment.Description;
                    transactionAssignmentOld.ActionId = transactionAssignment.ActionId;
                    transactionAssignmentOld.ToEntityId = transactionAssignment.ToEntityId;
                    transactionAssignmentOld.ToUserId = transactionAssignment.ToUserId;
                    transactionAssignmentOld.DeliveryMethodId = transactionAssignment.DeliveryMethodId;
                    transactionAssignmentOld.Date = transactionAssignment.Date;
                    transactionAssignmentOld.DateH = transactionAssignment.DateH;
                    transactionAssignmentOld.TrayId = transactionAssignment.TrayId;
                    transactionAssignmentOld.DeliveryMethodId = transactionAssignment.DeliveryMethodId;
                    transactionAssignmentOld.SpecialExplanation = transactionAssignment.SpecialExplanation;
                    transactionAssignmentOld.GeneralExplanation = transactionAssignment.GeneralExplanation;
                    transactionAssignmentOld.CurrentPathStep = (transactionAssignmentOld.TransactionPathId.HasValue) ?
                        (transactionAssignmentOld.CurrentPathStep + 1 > transactionPathDetailsCount) ? transactionAssignmentOld.CurrentPathStep :
                                                                (transactionAssignmentOld.CurrentPathStep != null) ? transactionAssignmentOld.CurrentPathStep + 1 : 1 : null;
                    transactionAssignmentOld.Viewed = false;
                    transactionAssignmentOld.ModefiedOn = DateTime.Now;
                    transactionAssignmentOld.ModefiedBy = User.Id;
                    if (!transactionAssignmentOld.Transaction.RemindDate.HasValue)
                    {
                        transactionAssignmentOld.DueDate = transactionAssignment.Date.AddDays(TransactionProcessingPeriod);
                    }
                    transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignmentOld);
                }
                else
                {
                    transactionAssignmentRepository.Add(transactionAssignment);
                }
            }
        }

        private void PostAssignTransaction(Transaction transaction, IList<TransactionAssignment> transactionAssignments, string cultureName = "")
        {
            ITransactionRepository ITransactionRepository = IoC.Resolve<ITransactionRepository>();
            ITransactionEntityDetailsRepository transactionEntityDetailsRepository = IoC.Resolve<ITransactionEntityDetailsRepository>();
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();

            TransactionHistory transactionHistory = transactionHistoryBL.GetLastTransactionHistory(transaction.Id);

            ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<ITransactionAssignmentHistoryRepository>();
             

            int? assignTransactionHistoryId;
            int? transactionHistoryId = transactionHistory.Id;

            foreach (TransactionAssignment transactionAssignment in transactionAssignments)
            {
                transactionEntityDetailsRepository.AddTransactionEntityDetails(new TransactionEntityDetails() { TransactionId = transaction.Id, EntityId = transactionAssignment.ToEntityId });
                 assignTransactionHistoryId = AddAssignTransactionHistory(transactionAssignment);

                 
                int reportId = 0;
                IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id, false, true);


                int? DeliveryReportId = transactionDeliveryReports.Where(x => x.TransactionAssignmentHistoryId == assignTransactionHistoryId).FirstOrDefault()?.Id;
                if (DeliveryReportId == null)
                {
                    reportId = transactionDeliveryReportBL.AddTransactionDeliveryReport(new TransactionDeliveryReport()
                    {
                        TransactionAssignmentHistoryId = assignTransactionHistoryId,
                        Date = DateTime.Now,
                        DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                        TransactionHistoryId = transactionHistoryId,
                        UserId = User != null ? User.Id : transactionAssignment.FromUserId,
                        TransactionId = transaction.Id,
                        ReporterId = transactionAssignment.ReporterId,
                        OrgunitId = transactionAssignment.FromEntityId
                    });
                } 

                //add internal copy
                if (transaction.Copies.Count > 0)
                {
                    foreach (TransactionCopy copy in transaction.Copies)
                    {
                        int? CopyId = transactionDeliveryReports != null && transactionDeliveryReports.Count > 0 ?
                            transactionDeliveryReports.Where(x => x.TransactionCopyId.HasValue && x.TransactionCopyId.Value == copy.Id).FirstOrDefault()?.Id : null;

                        if (CopyId == null)
                        {
                            reportId = transactionDeliveryReportBL.AddTransactionDeliveryReport(new TransactionDeliveryReport()
                            {
                                TransactionAssignmentHistoryId = assignTransactionHistoryId,
                                Date = DateTime.Now,
                                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                TransactionHistoryId = transactionHistory.Id,
                                UserId = User.Id,
                                TransactionId = transaction.Id,
                                ReporterId = transactionAssignment.ReporterId,
                                OrgunitId = transactionAssignment.FromEntityId,
                                TransactionCopyId = copy.Id
                            });
                        }
                    }
                }

                //add external copy
                if (transaction.ExternalCopies.Count > 0)
                {
                    foreach (TransactionExternalCopy externalCopy in transaction.ExternalCopies)
                    {
                        int? externalCopyId = transactionDeliveryReports != null && transactionDeliveryReports.Count > 0 ?
                            transactionDeliveryReports.Where(x => x.TransactionExternalCopyId.HasValue && x.TransactionExternalCopyId.Value == externalCopy.Id).FirstOrDefault()?.Id
                            : null;

                        if (externalCopyId == null)
                        {
                            reportId = transactionDeliveryReportBL.AddTransactionDeliveryReport(new TransactionDeliveryReport()
                            {
                                TransactionAssignmentHistoryId = assignTransactionHistoryId,
                                Date = DateTime.Now,
                                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                TransactionHistoryId = transactionHistory.Id,
                                UserId = User.Id,
                                TransactionId = transaction.Id,
                                ReporterId = transactionAssignment.ReporterId,
                                OrgunitId = transactionAssignment.FromEntityId,
                                TransactionExternalCopyId = externalCopy.Id
                            });
                        }
                    }
                }

                 
            }
        }

        public void SetTransactionAssignmentToViewedByTransactionId(int transactionId)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                transactionAssignmentRepository.SetTransactionAssignmentToViewedByTransactionId(transactionId);
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

        private void SendTrayActionNotification(TransactionAssignment transactionAssignment, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
          NotificationType notificationType, TrayType trayType, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                Transaction transaction = transactionAssignment.Transaction;
                if (transaction == null)
                {
                    transaction = TransactionBL.GetTransactionById(transactionAssignment.TransactionId, cultureName);
                }
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                IUserManagementBL userManagementBL = new UserManagementBL();
                NotificationTemplateType notifyTemplateType = NotificationTemplateType.None;
                int userId = 0;
                if (notificationTemplateType == NotificationTemplateType.AssignTransactionWeb)
                {
                    notifyTemplateType = notificationTemplateType;
                    userId = transactionAssignment.FromUserId;
                    notificationUsers.Add(NotificationsManager.BuildNotificationUser(transactionAssignment.FromUserId));
                }
                else
                {
                    switch (trayType)
                    {
                        //رفض استلام معاملة
                        case TrayType.OrgUnit:
                            notifyTemplateType = NotificationTemplateType.RevertRejectTransactionWeb;
                            userId = transactionAssignment.ToUserId.Value;
                            notificationUsers.Add(NotificationsManager.BuildNotificationUser(transactionAssignment.ToUserId.Value));
                            break;
                        //إرجاع معاملة
                        case TrayType.MyTransactions:
                        case TrayType.SentTransactions:
                        case TrayType.DraftOutbound:
                        case TrayType.OutboundExternal:
                            notifyTemplateType = NotificationTemplateType.RevertTransactionWeb;
                            userId = transactionAssignment.ToUserId.Value;
                            notificationUsers.Add(NotificationsManager.BuildNotificationUser(transactionAssignment.ToUserId.Value));
                            break;
                        default:
                            break;
                    }
                }
                NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(userId, cultureName);
                 if (notificationSubscriptions.HasFlag(NotificationSubscriptions.MyTransactions) ||
                    notificationSubscriptions.HasFlag(NotificationSubscriptions.OutboundDraft))
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add("{Number}", transaction.Number.ToString());
                    keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                    keyValues.Add("{Subject}", transaction.Subject);
                    keyValues.Add("{TransactionId}", StringCipher.Encrypt(transaction.Id.ToString()));
                    keyValues.Add("{TransactionTypeId}", transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text);
                    keyValues.Add("{TransTypeId}", transaction.TransactionCategoryId.ToString());
                    if (!string.IsNullOrEmpty(transaction.Priority.Text))
                    {
                        keyValues.Add("{PriorityId}", transaction.Priority.Text);
                    }
                    else
                    {
                        keyValues.Add("{PriorityId}", transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text);
                    }
                    if (!string.IsNullOrEmpty(transaction.Confidentiality.LocalName))
                    {
                        keyValues.Add("{ConfidentialityId}", transaction.Confidentiality.LocalName);
                    }
                    else
                    {
                        keyValues.Add("{ConfidentialityId}", transaction.Confidentiality.Name.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text);
                    }
                    keyValues.Add("{UserName}", User.UserName);
                    keyValues.Add("{Remarks}", transactionAssignment.Description);

                    //Notification Web
                    NotificationsManager.SystemNotification(notificationSource, notifyTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);
                    //Notification Email
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
        public TransactionPathDetails GetTransactionPathNextStep(int transactionId, string cultureName)
        {
            try
            {
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetTransactionPathNextStep(transactionId, cultureName);
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

        public void SetCopyAsViewed(int transId, int? toUserId, int toOrgUnit)
        {
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

            transactionAssignmentRepository.SetCopyAsViewed(transId, toUserId, toOrgUnit, DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now));
        }
        public TransactionAssignment TransactionDirectReply(int transactionId, string remarks, int userId)
        {
            bool hasReassigned = false;
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
            ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<ITransactionAssignmentHistoryRepository>();
            TransactionAssignment transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts => ts.TransactionId == transactionId);

            if (userId != transactionAssignment.FromUserId)
            {
                int? oldToUserId = transactionAssignment.ToUserId;
                transactionAssignment.ToUserId = transactionAssignment.FromUserId;
                transactionAssignment.FromUserId = oldToUserId.Value;

                int? oldToEntityId = transactionAssignment.ToEntityId;
                transactionAssignment.ToEntityId = transactionAssignment.FromEntityId;
                transactionAssignment.FromEntityId = oldToEntityId.Value;
                transactionAssignment.Description = remarks;

                transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignment);

                hasReassigned = true;
            }

            return transactionAssignment;
        }

        public int GetTransactionAssignmentHistoryCount(int userId, int trayId, int OrgUnitId, TransactionDateType transactionDateType = TransactionDateType.Any)
        {
            try
            {
                IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                IList<Permission> permissions =
                    permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                if (permissions != null && permissions.Count > 0)
                {
                    int? userWeigth = permissions.Max(s => s.Weight);
                    //TODO: combine the expression
                    ITransactionAssignmentRepository transactionAssignmentRepository =
                        IoC.Resolve<ITransactionAssignmentRepository>();

                    Expression<Func<TransactionAssignment, bool>> where = null;


                    int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);

                    where = where = a => ((a.FromUserId != a.ToUserId) || (a.FromUserId == a.ToUserId && a.FromEntityId != a.ToEntityId))
                       && a.FromEntityId == OrgUnitId && a.FromUserId == User.Id &&
                       (a.TrayId == (int)TrayType.MyTransactions | a.TrayId == (int)TrayType.OrgUnit | a.TrayId == (int)TrayType.DraftOutbound)
                       && a.Transaction.StatusId == InProcess
                       && a.Transaction.TransactionCategoryId != ExternalOutbound
                       && !a.IsHidden;



                    switch (transactionDateType)
                    {
                        case TransactionDateType.Today:
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                                         ts.ModefiedOn.Value.Year == DateTime.Now.Year &
                                         ts.ModefiedOn.Value.Day == DateTime.Now.Day &
                                         ts.ModefiedOn.Value.Month == DateTime.Now.Month);

                            }
                            break;
                        case TransactionDateType.Late:
                            {
                                IUserManagementBL userManagementBL = new UserManagementBL();

                                UserProfile userProfile = userManagementBL.GetUserById(userId);

                                DateTime date = DateTime.Now.Date;

                                if (userProfile.TransactionProcessingPeriod > 0)
                                {
                                    date = DateTime.Now.AddDays(-userProfile.TransactionProcessingPeriod);
                                }
                                where = ExpressionUtility.AndAlso(where, ts =>
                                ts.Transaction.RemindDate < date);
                            }
                            break;
                        case TransactionDateType.HasDate:
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                             ts.Transaction.RemindDate != null
                             );
                            }
                            break;
                    }

                    int count = 0;
                    if (where != null)
                    {
                        count = transactionAssignmentRepository.GetTransactionAssignmentHistoryCount(where);
                    }
                    return count;
                }

                return 0;
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

        public int GetMyTransactionTrayCount(int userId, int trayId, int OrgUnitId, TrayProcedureFilter trayProcedureFilter = TrayProcedureFilter.OthersAll, TransactionDateType transactionDateType = TransactionDateType.Any)
        {
            try
            {
                IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                IList<int> transactionsIds;
                IList<Permission> permissions =
                    permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                if (permissions != null && permissions.Count > 0)
                {
                    int? userWeigth = permissions.Max(s => s.Weight);
                    //TODO: combine the expression
                    ITransactionAssignmentRepository transactionAssignmentRepository =
                        IoC.Resolve<ITransactionAssignmentRepository>();

                    Expression<Func<TransactionAssignment, bool>> where = null;

                    TrayType trayType = (TrayType)trayId;


                    int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Rejected = TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int MultiOwnership = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int TempSave = TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Completed = TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int Outbound = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Sent = TransactionStatus.Sent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int Reserved = TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    int SendCopyToView = ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty);
                    int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                    int electronicDelivery = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);
                    int electronicPaperDelivery = DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);
                    int DeletedCopy = TransCopyStatus.Delete.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                    int ViewedCopy = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                    switch (trayType)
                    {
                        case TrayType.MyTransactions:
                            int actionId = 0;
                            switch (trayProcedureFilter)
                            {
                                case TrayProcedureFilter.Necessary:
                                    actionId = (int)TrayProcedureId.Necessary;
                                    break;
                                case TrayProcedureFilter.Opinion:
                                    actionId = (int)TrayProcedureId.Opinion;
                                    break;
                                case TrayProcedureFilter.Follow:
                                    actionId = (int)TrayProcedureId.Follow;
                                    break;
                                case TrayProcedureFilter.Reviews:
                                    actionId = (int)TrayProcedureId.Reviews;
                                    break;
                                case TrayProcedureFilter.OthersAll:
                                    actionId = (int)TrayProcedureId.OthersAll;
                                    break;
                            }


                            where = s =>
                                      s.ToUserId == userId &
                                      s.TrayId == trayId &
                                      s.ToEntityId == OrgUnitId &
                                      (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == Rejected | s.Transaction.StatusId == MultiOwnership) &
                                      s.Transaction.TransactionCategoryId != ExternalOutbound &
                                      ((s.ActionId == actionId) || (actionId == 0 & (s.ActionId != (int)TrayProcedureId.Necessary && s.ActionId != (int)TrayProcedureId.Opinion
                                      && s.ActionId != (int)TrayProcedureId.Follow && s.ActionId != (int)TrayProcedureId.Reviews))) & s.Viewed == false;
                            break;
                        case TrayType.DraftOutbound:
                            where = (s =>
                         s.ToUserId == userId &
                         s.ToEntityId == OrgUnitId &&
                         s.Transaction.StatusId != Reserved &&
                         (((s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected) & s.Transaction.TransactionCategoryId == DraftOutbound & s.TrayId == (int)trayType)
                         | (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & (s.Transaction.IsDraft | s.Transaction.IsPresentationDraft))
                            /*| (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & s.Transaction.DeliveryMethodId == electronicPaperDelivery
                               & !s.Transaction.PrintedDeliveryReport & s.Transaction.StatusId != Outbound & s.Transaction.StatusId != Sent & s.Transaction.StatusId != Reserved)*/
                            )
                         );
                            break;
                        case TrayType.DeletedDraftOutbound:
                            where = (s =>
                            s.ToUserId == userId &
                            s.ToEntityId == OrgUnitId &
                            s.Transaction.IsDraft & s.Transaction.IsDeleted == true);

                            break;
                        case TrayType.OutboundExternal:
                            where = (s =>
                                      s.ToEntityId == OrgUnitId &
                                      s.FromUserId == userId &
                                     (s.Transaction.TransactionCategoryId == ExternalOutbound &
                                     (s.DeliveryMethodId == electronicDelivery ||
                                     (s.DeliveryMethodId == electronicPaperDelivery &&
                                      s.Transaction.PrintedDeliveryReport))) &
                                     !s.Transaction.IsDraft & !s.Transaction.IsSigned
                           );
                            break;
                        //case TrayType.OutboundExternal:
                        //    where = (s =>
                        //              s.ToEntityId == OrgUnitId &
                        //              s.FromUserId == userId &
                        //             (s.Transaction.TransactionCategoryId == ExternalOutbound &
                        //             (s.DeliveryMethodId == electronicDelivery ||
                        //             (s.DeliveryMethodId == electronicPaperDelivery &&
                        //              s.Transaction.PrintedDeliveryReport))) &
                        //             !s.Transaction.IsDraft & !s.Transaction.IsSigned
                        //   );
                        //    break;
                        case TrayType.ElcOutBound:
                            transactionsIds = transactionRepository.GetELcOutBoundIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id) &
                                    !s.Transaction.IsDraft &
                                    (s.Transaction.TransactionCategoryId == ExternalOutbound
                                    | s.Transaction.TransactionCategoryId == InternalOutbound);

                            break;
                        case TrayType.SentTransactions:
                            where = where = s => ((s.FromEntityId == s.ToEntityId & s.FromUserId != s.ToUserId) | (s.FromEntityId != s.ToEntityId)) &
                            s.FromEntityId == OrgUnitId &
                            s.FromUserId == User.Id &
                            (s.TrayId == (int)TrayType.MyTransactions | s.TrayId == (int)TrayType.OrgUnit | s.TrayId == (int)TrayType.DraftOutbound)
                             && s.Transaction.StatusId == InProcess;

                            break;
                        case TrayType.Saved:

                            where = s =>
                                     s.TrayId == trayId &
                                     s.ToEntityId == OrgUnitId &
                                     //s.ToUserId == userId &
                                     (s.Transaction.StatusId == TempSave | s.Transaction.StatusId == Completed) &
                                     s.Transaction.TransactionCategoryId != ExternalOutbound;

                            break;
                        case TrayType.OrgUnit:

                            where = s =>
                                       s.ToUser == null &
                                       s.TrayId == trayId &
                                       s.ToEntityId == OrgUnitId &
                                       (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == MultiOwnership) &
                                       s.Transaction.TransactionCategoryId != ExternalOutbound & !s.Transaction.NeedAcknowled;
                            break;
                        case TrayType.Manager:
                            where = s => s.ToEntityId == OrgUnitId &
                                        s.ToUserId != userId &
                                       (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == MultiOwnership) &
                                       s.Transaction.TransactionCategoryId != ExternalOutbound &
                                       s.TrayId == (int)TrayType.MyTransactions;
                            break;
                        case TrayType.Copies:
                            where = (s =>
                        s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                            (tc.UserId == userId | tc.UserId == null) &
                            tc.IsSent == 1 & tc.Status != DeletedCopy & !tc.SpecialCopy &
                            !tc.Transaction.IsDeleted & tc.Status != ViewedCopy).Any() & s.Transaction.TransactionCategoryId == Inbound);
                            break;
                        case TrayType.InternalInboundCopies:
                            where = (s =>
                         s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                             (tc.UserId == userId | tc.UserId == null) &
                             tc.IsSent == 1 & tc.Status != DeletedCopy & !tc.SpecialCopy &
                             !tc.Transaction.IsDeleted & tc.Status != ViewedCopy).Any() & s.Transaction.TransactionCategoryId == InternalOutbound);
                            break;
                        case TrayType.CopiesOutbound:
                            where = (s =>
                       s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                           (tc.UserId == userId | tc.UserId == null) &
                           tc.IsSent == 1 & tc.Status != DeletedCopy & !tc.SpecialCopy &
                           !tc.Transaction.IsDeleted & tc.Status != ViewedCopy).Any() & s.Transaction.TransactionCategoryId == ExternalOutbound);
                            break;
                        case TrayType.SavedCopies:
                            {
                                transactionsIds = transactionRepository.GetSavedCopiesIds(userId, OrgUnitId);
                                where = (s => transactionsIds.Contains(s.Transaction.Id));
                            }
                            break;
                        case TrayType.SpecialCopies:
                            where = (s =>
                       s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                           (tc.UserId == userId | tc.UserId == null) &
                           tc.IsSent == 1 & tc.Status != DeletedCopy & tc.SpecialCopy &
                           !tc.Transaction.IsDeleted & tc.Status != ViewedCopy).Any());
                            break;
                        case TrayType.YESSER:
                            where = s =>
                                     s.ToUserId == userId &
                                     s.TrayId == trayId &
                                     s.ToEntityId == OrgUnitId &
                                     (s.Transaction.StatusId == InProcess | s.Transaction.StatusId == MultiOwnership) &
                                     s.Transaction.TransactionCategoryId != ExternalOutbound;
                            break;
                        case TrayType.Tasks:
                            transactionsIds = transactionRepository.GetUserTasksTransactionsIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id) &
                                    s.Transaction.TransactionCategoryId != ExternalOutbound;
                            break;
                        case TrayType.FollowUp:
                            transactionsIds = transactionRepository.GetUserFollowUpTransactionsIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpUnderProcess:
                            transactionsIds = transactionRepository.GetUserFollowProcessIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpComplete:
                            transactionsIds = transactionRepository.GetUserFollowCompleteIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpCanceld:
                            transactionsIds = transactionRepository.GetUserFollowDeleteIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpEscalation:
                            transactionsIds = transactionRepository.GetUserFollowUpEscalationIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpReminder:
                            transactionsIds = transactionRepository.GetUserFollowReminderIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.FollowUpLate:
                            transactionsIds = transactionRepository.GetUserFollowLateIds(userId, OrgUnitId);
                            where = s => transactionsIds.Contains(s.Transaction.Id);
                            break;
                        case TrayType.Reservation:
                            where = s =>
                                     s.ToUserId == userId &
                                     //s.TrayId == trayId &
                                     s.ToEntityId == OrgUnitId &
                                     (s.Transaction.TransactionCategoryId == Inbound || s.Transaction.TransactionCategoryId == ExternalOutbound) &
                                     (s.Transaction.StatusId == Reserved | s.Transaction.StatusId == MultiOwnership);
                            break;
                    }

                    switch (transactionDateType)
                    {
                        case TransactionDateType.Today:
                            {
                                if (trayType == TrayType.SentTransactions)
                                {
                                    where = ExpressionUtility.AndAlso(where, ts =>
                                         ts.ModefiedOn.Value.Year == DateTime.Now.Year &
                                         ts.ModefiedOn.Value.Day == DateTime.Now.Day &
                                         ts.ModefiedOn.Value.Month == DateTime.Now.Month);
                                }
                                else
                                {
                                    where = ExpressionUtility.AndAlso(where, ts =>
                                   ts.Date.Year == DateTime.Now.Year &
                                   ts.Date.Day == DateTime.Now.Day &
                                   ts.Date.Month == DateTime.Now.Month);
                                }

                            }
                            break;
                        case TransactionDateType.Late:
                            {
                                IUserManagementBL userManagementBL = new UserManagementBL();

                                UserProfile userProfile = userManagementBL.GetUserById(userId);

                                DateTime date = DateTime.Now.Date;

                                if (userProfile.TransactionProcessingPeriod > 0)
                                {
                                    date = DateTime.Now.AddDays(-userProfile.TransactionProcessingPeriod);
                                }
                                where = ExpressionUtility.AndAlso(where, ts =>
                                ts.Transaction.RemindDate < date ||
                              (ts.TransactionAssignmentProcessPeriod == null && (
                                ts.Date < date)) ||
                                (ts.TransactionAssignmentProcessPeriod != null && (
                                ts.TransactionAssignmentProcessPeriod < date))

                               );
                            }
                            break;
                        case TransactionDateType.HasDate:
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                             ts.Transaction.RemindDate != null
                             );
                            }
                            break;
                    }

                    int count = 0;
                    if (where != null)
                    {
                        count = transactionAssignmentRepository.GetTransactionAssignmentCount(where);
                    }
                    return count;
                }

                return 0;
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
        public void RejectTransactionMobile(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName, int userId)
        {
            try
            {
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                TransactionAssignmentHistory transactionAssignmentHistorie = transactionAssignmentHistoryBL.GetUserMobileTransactionAssignmentHistories(t => t.ToEntityId == OrgUnitId && t.TransactionId == transactionId && t.ToUserId == userId, cultureName, userId).FirstOrDefault();
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                TransactionAssignment transactionAssignment;

                transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment
                   (
                   ts =>
                       ts.TransactionId == transactionId &
                       ts.ToEntityId == OrgUnitId &
                       (ts.ToUserId == userId || ts.ToUserId == null)
                     );

                //if (transactionAssignment.FromUserId == transactionAssignment.ToUserId)
                //{
                //    throw new BusinessException(StatusCode.CantReturnToSelf);
                //}

                transactionAssignment.ToUserId = transactionAssignmentHistorie.FromUserId;
                transactionAssignment.ToEntityId = transactionAssignmentHistorie.FromEntityId;
                transactionAssignment.FromUserId = userId;
                transactionAssignment.FromEntityId = OrgUnitId;
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionAssignment.Date);
                transactionAssignment.Description = remarks;
                transactionAssignment.CurrentPathStep = transactionAssignment.CurrentPathStep.HasValue ? transactionAssignment.CurrentPathStep - 1 : transactionAssignment.CurrentPathStep;
                transactionAssignment.Viewed = false;
                transactionAssignment.TrayId = trayId;
                transactionAssignment.ActionId = (int)ActionTransactionType.Rejected;


                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();

                OnRevertAssignTransaction(transactionAssignment);
                PostRevertAssignTransaction(transactionAssignment, null);

                SendTrayActionNotification(transactionAssignment, NotificationSource.RevertRejectTransaction, NotificationTemplateType.RevertRejectTransactionWeb,
                    NotificationTemplateType.RevertRejectTransactionEmail, NotificationEmailSubject.RevertRejectTransactionEmail,
                    NotificationWebSubject.RevertRejectTransaction, NotificationType.Web, (TrayType)trayId, cultureName);

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
