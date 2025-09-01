using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CounterRepository : BaseRepository<Counter>, ICounterRepository
    {
        #region Attributes

        

        #endregion Attributes

        #region Constructors

        public CounterRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
            
        }

        #endregion Constructors

        #region Methods

        public void UpdateCounter(Counter counter)
        {
            try
            {
                _oMCSDbContext.Entry(counter).State=System.Data.Entity.EntityState.Modified;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }

        }

        public Counter GetCounterById(int counterId)
        {
            try
            {
                return this.FindBy(p => p.Id == counterId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Counter GetGeneralCounter()
        {
            try
            {
                return _oMCSDbContext.Counters.Where(c => c.IsGeneral == true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public CounterDetail GetCounterDetailById(int counterDetailId)
        {
            try
            {
                return _oMCSDbContext.CounterDetails.Where(c => c.Id == counterDetailId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<CounterDetail> GetCounterDetailsByCounterId(int counterId)
        {
            try
            {
                return _oMCSDbContext.CounterDetails.Where(c => c.Counter.Id == counterId).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteCounterDetailById(int counterDetailId)
        {
            try
            {
                var counterDetail = _oMCSDbContext.CounterDetails.Find(counterDetailId);
                _oMCSDbContext.CounterDetails.Remove(counterDetail);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
