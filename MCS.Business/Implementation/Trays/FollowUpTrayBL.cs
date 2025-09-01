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
    public class FollowUpTrayBL : TrayBaseBL, IFollowUpTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.FollowUp; }
        }

        public override string TrayPermission { get { return UserClaims.Files.FollowUp; } }

        public override IList<TransactionTrayInfo> GetUserTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria, TransactionDateType transactionDate, out int rowsCount)
        {

            try
            {
                IList<Transaction> transactions = null;
                IList<TransactionAssignment> transactionAssignments = null;
                List<TransactionTrayInfo> transactionTrayInfos = new List<TransactionTrayInfo>();

                ICollaborationBL collaborationBL = new CollaborationBL();

                transactions = TransactionBL.GetUserTransactionsTray(User.Id, OrgUnitId, trayType, transactionDate, searchCriteria, out rowsCount);
                if (transactions != null)
                {
                    foreach (var transaction in transactions)
                    {
                        transactionAssignments = transaction.Assignments.ToList();

                        TransactionTrayInfo transactionTrayInfo = new TransactionTrayInfo();

                        transactionTrayInfo.TransactionAssignmentInfos = new List<TransactionAssignmentInfo>();

                        foreach (var transactionAssignment in transactionAssignments)
                        {
                            TransactionAssignmentInfo transactionAssignmentInfo = TransactionAssignmentBL.MapTransactionAssignment(transactionAssignment, searchCriteria.CultureName);
                            transactionAssignmentInfo.HasCollaboration = collaborationBL.HasCollaboration(transactionAssignment.FromUserId, transaction.Id);
                            transactionTrayInfo.TransactionAssignmentInfos.Add(transactionAssignmentInfo);
                        }

                        transactionTrayInfo.transactionDetailsInfo = TransactionBL.MapTransaction(transaction, searchCriteria.CultureName);

                        if (transaction.RemindDate.HasValue)
                        {
                            if (transaction.RemindDate.Value.Date < DateTime.Now.Date)
                            {
                                transactionTrayInfo.transactionDetailsInfo.IsLate = true;
                            }
                        }
                        else
                        {
                            TransactionAssignment transactionAssignment = transactionAssignments.Where(a => a.ToEntityId == OrgUnitId && a.ToUserId.HasValue && a.ToUserId.Value == User.Id).FirstOrDefault();
                            if (transactionAssignment != null)
                            {
                                int processingPeriod = 0;
                                if (transactionAssignment.ToUserId.HasValue)
                                {
                                    IUserManagementBL userManagementBL = new UserManagementBL();
                                    UserProfile userProfile = userManagementBL.GetUserById(transactionAssignment.ToUserId.Value);
                                    processingPeriod = userProfile.TransactionProcessingPeriod;
                                }

                                DateTime date = DateTime.Now.Date;

                                if (processingPeriod > 0)
                                {
                                    date = DateTime.Now.Date.AddDays(-processingPeriod);
                                }

                                if (transactionAssignment.Date.Date > date)
                                {
                                    transactionTrayInfo.transactionDetailsInfo.IsLate = true;
                                }
                            }
                        }

                        transactionTrayInfos.Add(transactionTrayInfo);
                    }
                }

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
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                Tray tray = GetTrayById((int)TrayType, searchCriteria.CultureName);

                TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo()
                {
                    Id = tray.Id,
                    Name = tray.LocalName,
                    TransactionTraysInfo = new List<TransactionTrayInfo>()
                };

                trayDetailsInfo.TodayTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any);
                trayDetailsInfo.TransactionTraysInfo = GetUserTransactionsByTray(TrayType.FollowUp, OrgUnitId, searchCriteria, TransactionDateType.Any, out rowsCount);
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
    }
}
