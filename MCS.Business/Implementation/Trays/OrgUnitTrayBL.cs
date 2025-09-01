using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public class OrgUnitTrayBL : TrayBaseBL, IOrgUnitTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.OrgUnit; }
        }

        public override string TrayPermission { get { return UserClaims.Files.OrgUnit; } }

        public override void Assign(int transactionId, int OrgUnitId, string cultureName)
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                transactionAssignmentBL.Assign(transactionId, OrgUnitId, cultureName);
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
        public override IList<TransactionTrayInfo> GetUserTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria, TransactionDateType transactionDate, out int rowsCount)
        {
            try
            {
                return GetTransactionsInfoByTray(c => c.ToUserId == -1, OrgUnitId, searchCriteria, transactionDate, trayType, out rowsCount);
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
