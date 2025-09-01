using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class PriorityExceptionRepository : BaseRepository<PriorityException>, IPriorityExceptionRepository
    {
        #region Constructors

        public PriorityExceptionRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods
        public int AddPriorityException(PriorityException priorityException)
        {
            try
            {
                _oMCSDbContext.PriorityExceptions.Add(priorityException);
                Priority priority = _oMCSDbContext.Priorities.Where(p => p.Id == priorityException.PriorityId).FirstOrDefault();
                    priority.HasPriorityExceptions = true;
                return _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw DataAccessException.Translate(ex);
            }
        }

        public void DeletePriorityException(int priorityExceptionId)
        {
            try
            {
                PriorityException priorityException = _oMCSDbContext.PriorityExceptions.FirstOrDefault(pe => pe.Id == priorityExceptionId);
                if (priorityException != null)
                {
                    _oMCSDbContext.PriorityExceptions.Remove(priorityException);
                    _oMCSDbContext.SaveChanges();
                }
                Priority priority = _oMCSDbContext.Priorities.Where(p => p.Id == priorityException.PriorityId).FirstOrDefault();
                var NoExeption = _oMCSDbContext.PriorityExceptions.FirstOrDefault(pe => pe.PriorityId == priorityException.PriorityId);
                if (NoExeption == null)
                {
                    priority.HasPriorityExceptions = false;
                    _oMCSDbContext.SaveChanges();

                }

            }
            catch (Exception ex)
            {

                throw DataAccessException.Translate(ex);
            }
        }

        public PriorityException GetPriorityExceptionById(int priorityExceptionId)
        {
            try
            {
                return _oMCSDbContext.PriorityExceptions.FirstOrDefault(pe => pe.Id == priorityExceptionId);
            }
            catch (Exception ex)
            {

                throw DataAccessException.Translate(ex);
            }
        }
        public PriorityException GetPriorityExceptionByPriorityId(int priorityId)
        {
            try
            {
                return _oMCSDbContext.PriorityExceptions.Include(pe => pe.Priority).FirstOrDefault(pe => pe.PriorityId == priorityId);
            }
            catch (Exception ex)
            {

                throw DataAccessException.Translate(ex);
            }
        }

        public IList<PriorityException> GetPriorityExceptions(SearchCriteria searchCriteria, int priorityId, out int rowsCount)
        {
            try
            {
                IQueryable<PriorityException> priorityExceptions = (from PriorityException in _oMCSDbContext.PriorityExceptions
                                                                    where PriorityException.PriorityId == priorityId
                                                                    select PriorityException);

                rowsCount = priorityExceptions.Count();

                if (searchCriteria.Ascending)
                {
                    priorityExceptions = priorityExceptions.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    priorityExceptions = priorityExceptions.OrderByDescending(p => p.Id)
                    .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return priorityExceptions.ToList().Select(pe => new PriorityException
                {
                    Id = pe.Id,
                    Priority = pe.Priority,
                    OrgUnit = pe.OrgUnit,
                    UserProfile = pe.UserProfile,
                    LateOnUsersAfter = pe.LateOnUsersAfter
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdatePriorityException(PriorityException priorityException)
        {
            try
            {
                PriorityException oldPriorityException = GetPriorityExceptionById(priorityException.Id);
                if (oldPriorityException != null)
                {
                    _oMCSDbContext.Entry(oldPriorityException).CurrentValues.SetValues(priorityException);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
