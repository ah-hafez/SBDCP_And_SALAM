using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public class OutboundExternalTrayBL : TrayBaseBL, IOutboundExternalTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.OutboundExternal; }
        }

        public override string TrayPermission { get { return UserClaims.Files.OutboundExternal; } }

        public override IList<TransactionTrayInfo> GetUserTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria, TransactionDateType transactionDate, out int rowsCount)
        {
            try
            {
                CheckTrayAuthorization();

                List<TransactionTrayInfo> transactionTraysInfos = new List<TransactionTrayInfo>();

                IList<Transaction> transactions = transactions = TransactionBL.GetUserTransactionsTray(User.Id, OrgUnitId, TrayType, transactionDate, searchCriteria, out rowsCount);

                foreach (Transaction transaction in transactions)
                {
                    TransactionTrayInfo trayInfo = new TransactionTrayInfo()
                    {
                        TransactionAssignmentInfos = null,

                        transactionDetailsInfo = TransactionBL.MapTransaction(transaction, searchCriteria.CultureName)
                    };
                    transactionTraysInfos.Add(trayInfo);
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
    }
}
