using System;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using System.Linq;

namespace MCS.Business
{
    public class SavedTrayBL : TrayBaseBL, ISavedTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.Saved; }
        }

        public override string TrayPermission { get { return UserClaims.Files.Saved; } }

        public override void SaveRevert(int transactionId, int OrgUnitId)
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                TransactionAssignment transactionAssignment = null;

                transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts =>
                        ts.ToUserId == User.Id &
                        ts.ToEntityId == OrgUnitId &
                        ts.TransactionId == transactionId
                        );

                if (transactionAssignment == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                transactionAssignment.Description = string.Empty;
                transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);
                transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

                transactionRepository.UpdateTransactionStatus(transactionId, TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty));
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
    }
}
