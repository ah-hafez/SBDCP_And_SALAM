using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CorrespondentRepository : BaseRepository<Reporter>, ICorrespondentRepository
    {
        public CorrespondentRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
          : base(ambienTTransactionContextLocator)
        {

        }

        public int AddReporter(Reporter Reporter)
        {
            try
            {
                Reporter.IsActive = true;
                _oMCSDbContext.Reporters.Add(Reporter);
                return _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteReporter(int id)
        {
            try
            {
                Reporter reporter = _oMCSDbContext.Reporters.FirstOrDefault(c => c.Id == id);
                if (reporter != null)
                {
                    _oMCSDbContext.Reporters.Remove(reporter);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Reporter GetReporterById(int ReporterId)
        {
            try
            {
                return _oMCSDbContext.Reporters.FirstOrDefault(c => c.Id == ReporterId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Reporter> GetReporters(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Reporter> reporters = (from reporter in _oMCSDbContext.Reporters
                                                  select reporter);

                rowsCount = reporters.Count();

                if (searchCriteria.Ascending)
                {
                    reporters = reporters.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    reporters = reporters.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return reporters.ToList().Select(c => new Reporter
                {
                    Id = c.Id,
                    IsActive = c.IsActive,
                    IsLocked = c.IsLocked,
                    LockedBy = c.LockedBy,
                    LocalizationIdentifier = c.LocalizationIdentifier,
                    ToEntityId = c.ToEntityId,
                    OrgUnit = c.OrgUnit
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void LockUnlockLookup(int ReporterId, int userId)
        {
            try
            {
                var ReporterToUpdate = _oMCSDbContext.Reporters.FirstOrDefault(f => f.Id == ReporterId);
                if (ReporterToUpdate != null)
                {
                    ReporterToUpdate.IsLocked = !ReporterToUpdate.IsLocked;

                    if (ReporterToUpdate.IsLocked)
                    {
                        ReporterToUpdate.LockedBy = userId;
                    }
                    else
                    {
                        ReporterToUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void ActiveDeactiveLookup(int ReporterId)
        {
            try
            {
                var Reporter = _oMCSDbContext.Reporters.FirstOrDefault(f => f.Id == ReporterId);
                if (Reporter != null)
                {
                    Reporter.IsActive = !Reporter.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void UpdateReporter(Reporter Reporter)
        {
            try
            {
                Reporter ReporterOld = GetReporterById(Reporter.Id);

                if (ReporterOld != null)
                {
                    Reporter.IsActive = ReporterOld.IsActive;

                    _oMCSDbContext.Entry(ReporterOld).CurrentValues.SetValues(Reporter);

                    foreach (Localization localization in Reporter.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = ReporterOld.LocalizationIdentifier.Localizations
                         .Where(l => l.Id == localization.Id)
                         .FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
    }
}
