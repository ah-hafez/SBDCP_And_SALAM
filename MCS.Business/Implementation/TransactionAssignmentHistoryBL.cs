using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using System.Linq;

namespace MCS.Business
{
    public class TransactionAssignmentHistoryBL : BaseBL, ITransactionAssignmentHistoryBL
    {
        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistories(int transactionId, string cultureName)
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
                ITransactionAssignmentHistoryRepository transactionAssignmentHistory = IoC.Resolve<TransactionAssignmentHistoryRepository>();
                return transactionAssignmentHistory.GetTransactionAssignmentHistory(transactionId, cultureName, userWeight);
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
        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryWithContent(int transactionId, string cultureName)
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
                ITransactionAssignmentHistoryRepository transactionAssignmentHistory = IoC.Resolve<TransactionAssignmentHistoryRepository>();
                return transactionAssignmentHistory.GetTransactionAssignmentHistoryWithContent(transactionId, cultureName, userWeight);
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
        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistoryByTransactionId(int transactionId)
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
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();
                return transactionAssignmentHistoryRepository.GetTransactionAssignmentHistoryByTransactionId(transactionId, userWeight);
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
        public int AddTransactionAssignmentHistory(TransactionAssignment transactionAssignment)
        {
            try
            {
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();
                TransactionAssignmentHistory transactionAssignmentHistory = new TransactionAssignmentHistory()
                {
                    Date = DateTime.Now,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendarWithTime(DateTime.Now),
                    Description = transactionAssignment.Description,
                    FromEntityId = transactionAssignment.FromEntityId,
                    ToEntityId = transactionAssignment.ToEntityId,
                    ToUserId = transactionAssignment.ToUserId,
                    TransactionId = transactionAssignment.TransactionId,
                    TrayId = transactionAssignment.TrayId,
                    FromUserId = transactionAssignment.FromUserId,
                    UserDelegationId = transactionAssignment.UserDelegationId,
                    GeneralExplanation = transactionAssignment.GeneralExplanation,
                    SpecialExplanation = transactionAssignment.SpecialExplanation,
                };

                if (transactionAssignment.ActionId.HasValue)
                {
                    transactionAssignmentHistory.ActionId = transactionAssignment.ActionId.Value;
                }
                return transactionAssignmentHistoryRepository.AddTransactionAssignmentHistory(transactionAssignmentHistory);
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
        public TransactionAssignmentHistory GetTransactionAssignmentHistoryById(int assignmentHistoryId)
        {
            try
            {
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();
                return transactionAssignmentHistoryRepository.Get(assignmentHistoryId);
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
        public IList<TransactionAssignmentHistory> GetTransactionAssignmentHistories(Expression<Func<TransactionAssignmentHistory, bool>> @where)
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
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();
                return transactionAssignmentHistoryRepository.GetTransactionAssignmentHistories(@where, userWeight);
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
        public IList<TransactionAssignmentHistory> GetUserMobileTransactionAssignmentHistories(Expression<Func<TransactionAssignmentHistory, bool>> @where, string cultureName, int userId)
        {
            try
            {
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();
                return transactionAssignmentHistoryRepository.GetUserMobileTransactionAssignmentHistories(@where);
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

        public void UpdateTransactionAssignmentHistory(int transId, int ExplanationId)
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
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<ITransactionAssignmentHistoryRepository>();
                if (ExplanationId > 0)
                    transactionAssignmentHistoryRepository.UpdateTransactionAssignmentHistoryExplanation(transId, ExplanationId, userWeight);
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

        public TransactionAssignmentHistory GetLastTransactionAssignmentHistory(int transactionId)
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
                ITransactionAssignmentHistoryRepository transactionAssignmentHistory = IoC.Resolve<TransactionAssignmentHistoryRepository>();
                return transactionAssignmentHistory.GetLastTransactionAssignmentHistory(transactionId, userWeight, User.Id);
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

        public void HideTransactionAssignment(int assignmentId)
        {
            try
            {
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();

                transactionAssignmentHistoryRepository.HideTransactionHistory(assignmentId);
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
        public void HideTransaction(int transactionId)
        {
            try
            {
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();

                transactionAssignmentHistoryRepository.HideTransaction(transactionId);
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


        public void HideTransactionAssignments(string assignmentIds)
        {
            try
            {
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();

                transactionAssignmentHistoryRepository.HideTransactionHistories(assignmentIds);
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
        public void HideTransactions(string transactionIds)
        {
            try
            {
                ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<TransactionAssignmentHistoryRepository>();

                transactionAssignmentHistoryRepository.HideTransactions(transactionIds);
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
