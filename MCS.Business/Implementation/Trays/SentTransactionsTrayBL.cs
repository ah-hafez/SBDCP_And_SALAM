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
    public class SentTransactionsTrayBL : TrayBaseBL, ISentTransactionsTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.SentTransactions; }
        }

        public override string TrayPermission { get { return UserClaims.Files.SentTransactions; } }

        public override TrayDetailsInfo GetTrayDetailsInfo(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                CheckTrayAuthorization();

                ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

                Tray tray = GetTrayById((int)TrayType, searchCriteria.CultureName);

                TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo()
                {
                    Id = tray.Id,
                    Name = tray.LocalName,
                    TransactionTraysInfo = new List<TransactionTrayInfo>()
                };

                IList<TransactionAssignment> TransactionAssignments = transactionAssignmentBL.GetTransactionAssignments(OrgUnitId, searchCriteria, out rowsCount, TrayType, null);

                trayDetailsInfo.TodayTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any);
                trayDetailsInfo.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any);

                foreach (TransactionAssignment transactionAssignment in TransactionAssignments)
                {
                    TransactionTrayInfo transactionTrayInfo = new TransactionTrayInfo();

                    transactionTrayInfo.TransactionAssignmentInfos = new List<TransactionAssignmentInfo>();
                    transactionTrayInfo.TransactionAssignmentInfos.Add(TransactionAssignmentBL.MapTransactionAssignment(transactionAssignment, searchCriteria.CultureName));
                    transactionTrayInfo.transactionDetailsInfo = TransactionBL.MapTransaction(transactionAssignment.Transaction, searchCriteria.CultureName);

                    trayDetailsInfo.TransactionTraysInfo.Add(transactionTrayInfo);
                }

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

                ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

                List<TransactionTrayInfo> TransactionTraysInfos = new List<TransactionTrayInfo>();

                IList<TransactionAssignment> TransactionAssignments = transactionAssignmentBL.GetTransactionAssignments(OrgUnitId, searchCriteria, out rowsCount, TrayType, (int)transactionDate);

                foreach (TransactionAssignment transactionAssignment in TransactionAssignments)
                {
                    TransactionTrayInfo transactionTrayInfo = new TransactionTrayInfo();

                    transactionTrayInfo.TransactionAssignmentInfos = new List<TransactionAssignmentInfo>();

                    transactionTrayInfo.TransactionAssignmentInfos.Add(TransactionAssignmentBL.MapTransactionAssignment(transactionAssignment, searchCriteria.CultureName));

                    transactionTrayInfo.transactionDetailsInfo = TransactionBL.MapTransaction(transactionAssignment.Transaction, searchCriteria.CultureName);

                    TransactionTraysInfos.Add(transactionTrayInfo);
                }
                return TransactionTraysInfos;
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

        public override void RevertAssignTransaction(int transactionId, int OrgUnitId, int trayId)
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
                transactionAssignmentBL.RevertAssignByTransaction(transactionId, OrgUnitId, trayId);
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
