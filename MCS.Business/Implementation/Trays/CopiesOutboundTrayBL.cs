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
    public class CopiesOutboundTrayBL : TrayBaseBL, ICopiesOutboundTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.CopiesOutbound; }
        }

        public override string TrayPermission { get { return UserClaims.Files.CopiesOutbound; } }

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

                transactionAssignments = TransactionBL.GetUserTransactionsTray(User.Id, OrgUnitId, trayType, transactionDate, searchCriteria, out int assignmentsRowsCount);

                TransactionBL.MapTransaction(transactionAssignments, searchCriteria.CultureName).ToList().ForEach(t => transactionTrayInfos.Add(t));

                transactionTrayInfos = transactionTrayInfos.Where(s => s.transactionDetailsInfo.TransactionCategoryId == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) | s.transactionDetailsInfo.TransactionCategoryId == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)).ToList();

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

                trayDetailsInfo.TodayTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any) +
                      TransactionBL.GetTransactionCopiesCount(User.Id, OrgUnitId, DateTime.Now);

                trayDetailsInfo.TransactionTraysInfo = GetUserTransactionsByTray(TrayType, OrgUnitId, searchCriteria, TransactionDateType.Any, out rowsCount);
                trayDetailsInfo.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any) +
                    TransactionBL.GetTransactionCopiesCount(User.Id, OrgUnitId, null);

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

        public override void Viewed(int transactionId, int OrgUnitId, int userId, string cultureName = "")
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

                TransactionAssignment transAssignment = transaction.Assignments
                    .Where(a => a.TransactionId == transaction.Id && a.ToUserId == User.Id && a.ToEntityId == OrgUnitId
                        && a.TrayId == (int)TrayType.Copies).FirstOrDefault();

                if (transAssignment != null)
                {
                    transactionAssignmentBL.SetTransactionAssignmentToViewed(transAssignment);
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

        public override void DeleteCopy(int transactionId, int OrgUnitId, int userId, string cultureName = "")
        {
            try
            {
                Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                TransactionCopy transactionCopy = transaction.Copies.Where(tc => tc.TransactionId == transaction.Id && tc.UserId == User.Id && tc.EntityId == OrgUnitId && tc.IsSent == 1).FirstOrDefault();

                if (transactionCopy != null)
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
        public override void SetTransactionCopyToUndo(int transactionId, int OrgUnitId, int userId, string cultureName = "")
        {
            try
            {
                Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                TransactionCopy transactionCopy = transaction.Copies.Where(tc => tc.TransactionId == transaction.Id && tc.UserId == User.Id && tc.EntityId == OrgUnitId && tc.IsSent == 1).FirstOrDefault();

                if (transactionCopy != null)
                {
                    transactionBL.SetTransactionCopyToUndo(transactionCopy);
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

            Tray tray = GetTrayById((int)TrayType, searchCriteria.CultureName);

            TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo()
            {
                Id = tray.Id,
                Name = tray.LocalName,
                TransactionTraysInfo = new List<TransactionTrayInfo>()
            };

            IPermissionBL permissionBL = new PermissionBL();

            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

            int? userWeight = permissions.Max(p => p.Weight);

            Expression<Func<TransactionAssignment, bool>> where = (s =>
                              s.ToUserId == User.Id &
                              s.TrayId == (int)TrayType &
                              s.ToEntityId == OrgUnitId &
                              s.Action.Type.Id == ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty) &
                              !s.Viewed &
                              s.IsPopulariazation
                              );

            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<TransactionAssignmentRepository>();

            IList<Transaction> transactions = transactionAssignmentRepository.GetUserTransactionsTray(where, userWeight, searchCriteria, User.Id, out rowsCount);

            if (transactions != null)
            {
                TransactionBL.MapTransaction(transactions, searchCriteria.CultureName).ToList().ForEach(t => trayDetailsInfo.TransactionTraysInfo.Add(t));
            }

            return trayDetailsInfo;
        }
    }
}
