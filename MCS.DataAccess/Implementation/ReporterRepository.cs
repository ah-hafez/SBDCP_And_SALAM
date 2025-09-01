using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ReporterRepository : BaseRepository<Reporter>, IReporterRepository
    {
        public ReporterRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator) : base(ambienTTransactionContextLocator)
        {
        }

        public int AddReporter(Reporter reporter)
        {
            try
            {
                string nameEN = reporter.LocalizationIdentifier.Localizations.FirstOrDefault(a => a.CultureId == (int)CultureType.English).Text.ToLower();
                string nameAR = reporter.LocalizationIdentifier.Localizations.FirstOrDefault(a => a.CultureId == (int)CultureType.Arabic).Text.ToLower();

                var result =
                _oMCSDbContext.Reporters
                .Where(r => (r.LocalizationIdentifier.Localizations.FirstOrDefault(a => a.CultureId == (int)CultureType.English).Text.ToLower() == nameEN
                            | r.LocalizationIdentifier.Localizations.FirstOrDefault(a => a.CultureId == (int)CultureType.Arabic).Text.ToLower() == nameAR)
                            & r.ToEntityId == reporter.ToEntityId
                            )
                            .FirstOrDefault();

                if (result != null)
                {
                    throw new DataAccessException();
                }

                _oMCSDbContext.Reporters.Add(reporter);
                _oMCSDbContext.SaveChanges();
                return reporter.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<Reporter> GetReporters(string cultureName, int orgUnitId)
        {
            try
            {
               var reporters = (from reporter in _oMCSDbContext.Reporters
                                                  where !reporter.IsDeleted && reporter.IsActive && reporter.ToEntityId == orgUnitId
                                                  select new
                                                  {
                                                      reporter.Id,
                                                      reporter.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                  }).ToList().Select(p => new Reporter
                                                  {
                                                      Id = p.Id,
                                                      Text = p.Text
                                                  }).ToList();

                return reporters;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Reporter GetReporterById(int id, string cultureName)
        {
            try
            {
                IList<Reporter> reporters = (from reporter in _oMCSDbContext.Reporters
                                                  where !reporter.IsDeleted && reporter.Id == id && reporter.IsActive
                                                  select new
                                                  {
                                                      reporter.Id,
                                                      reporter.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                  }).ToList().Select(p => new Reporter
                                                  {
                                                      Id = p.Id,
                                                      Text = p.Text
                                                  }).ToList();

                return reporters.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
    }
}
