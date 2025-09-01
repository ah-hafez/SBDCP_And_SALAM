using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class TransactionIndexLogBL : BaseBL, ITransactionIndexLogBL
    {
        public int AddIndex(TransactionIndexLog transactionIndex)
        {
            try
            {
                ITransactionIndexLogRepository transactionIndexRepository = IoC.Resolve<TransactionIndexLogRepository>();
                return transactionIndexRepository.AddIndex(transactionIndex);
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

        public void UpdateIndex(TransactionIndexLog transactionIndex)
        {
            try
            {
                ITransactionIndexLogRepository transactionIndexRepository = IoC.Resolve<TransactionIndexLogRepository>();

                TransactionIndexLog transactionIndexLog = transactionIndexRepository.Get(transactionIndex.TransId);

                transactionIndex.IsUpdated = true;
                transactionIndex.Id = transactionIndexLog.Id;
                transactionIndex.Date = transactionIndexLog.Date;
                transactionIndex.DateH = transactionIndexLog.DateH;
                transactionIndex.Year = transactionIndexLog.Year;
                transactionIndex.YearH = transactionIndexLog.YearH;
                transactionIndex.Barcode = transactionIndexLog.Barcode;

                transactionIndexRepository.UpdateIndex(transactionIndex);
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

        public IList<TransactionIndexLog> GetIndexedTransactions(Expression<Func<TransactionIndexLog, bool>> where)
        {
            try
            {
                ITransactionIndexLogRepository transactionIndexRepository = IoC.Resolve<TransactionIndexLogRepository>();
                return transactionIndexRepository.GetIndexedTransactions(where);
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
