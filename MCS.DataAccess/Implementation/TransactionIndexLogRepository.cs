using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionIndexLogRepository : BaseRepository<TransactionIndexLog>, ITransactionIndexLogRepository
    {
        #region Attributes

        

        #endregion Attributes

        #region Constructors

        public TransactionIndexLogRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
            
        }

        #endregion Constructors

        #region Methods

        public int AddIndex(TransactionIndexLog transactionIndex)
        {
            try
            {
                _oMCSDbContext.TransactionIndexes.Add(transactionIndex);

                _oMCSDbContext.SaveChanges();

                return transactionIndex.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateIndex(TransactionIndexLog transactionIndex)
        {
            try
            {
                TransactionIndexLog transactionIndexOld = this.FindBy(t => t.TransId == transactionIndex.TransId);

                if (transactionIndexOld != null)
                {
                    _oMCSDbContext.Entry(transactionIndexOld).CurrentValues.SetValues(transactionIndex);

                    _oMCSDbContext.SaveChanges();
                }               
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionIndexLog GetIndexByTransactionId(int transactionId)
        {
            try
            {
                return this.FindBy(t => t.TransId == transactionId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionIndexLog> GetIndexedTransactions(Expression<Func<TransactionIndexLog, bool>> @where)
        { 
            try
            {
                return _oMCSDbContext.TransactionIndexes.Where(@where).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
