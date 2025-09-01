using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public class TrayRepository : BaseRepository<Tray>, ITrayRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TrayRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public IList<Tray> GetAllTrays(string cultureName)
        {
            try
            {
                IList<Tray> trays = (from tray in _oMCSDbContext.Trays
                                          select new
                                          {
                                              tray.Id,
                                              tray.Sort,
                                              tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                          }).ToList().Select(t => new Tray
                                          {
                                              Id = t.Id,
                                              Sort = t.Sort,
                                              LocalName = t.Text
                                          }).ToList();

                return trays;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTray(Tray tray)
        {
            try
            {
                Tray trayOld = GetTrayById(tray.Id);

                if (trayOld != null)
                {
                    trayOld.Sort = tray.Sort;

                    _oMCSDbContext.Entry(trayOld).CurrentValues.SetValues(tray);

                    if (tray.Name != null)
                    {
                        foreach (LookupLocalization localization in tray.Name.Localizations)
                        {
                            LookupLocalization lookupLocalization = trayOld.Name.Localizations
                                                                        .Where(l => l.Id == localization.Id)
                                                                        .FirstOrDefault();

                            if (lookupLocalization != null)
                            {
                                _oMCSDbContext.Entry(lookupLocalization).CurrentValues.SetValues(localization);
                            }
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

        public void UpdateTrays(IList<Tray> trays)
        {
            try
            {
                foreach (var tray in trays)
                {
                    UpdateTray(tray);
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Tray GetTrayById(int id)
        {
            try
            {
                return _oMCSDbContext.Trays.Where(t => t.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Tray> GetTrays(SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Tray> trays = (from tray in _oMCSDbContext.Trays
                                          select tray);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Tray).GetProperty(filter.ColumnName).PropertyType))
                        {
                            trays = SortByText(trays, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }

                rowsCount = trays.Count();

                if (searchCriteria.Ascending)
                {
                    trays = trays.OrderBy(x => x.Sort).ThenBy(p => p.Name.Localizations
                        .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    trays = trays.OrderBy(x => x.Sort).ThenByDescending(p => p.Name.Localizations
                        .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return trays.ToList().Select(t => new Tray
                {
                    Id = t.Id,
                    LocalName = t.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Tray> SortByText(IQueryable<Tray> source, string textValue, FilterType filterType, string CultureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from tray in _oMCSDbContext.Trays.Where(p => p.Name.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select tray);
                case FilterType.EndsWidth:
                    return (from tray in _oMCSDbContext.Trays.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text.EndsWith(textValue))
                            select tray);
                case FilterType.StartsWith:
                    return (from tray in _oMCSDbContext.Trays.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text.StartsWith(textValue))
                            select tray);
                case FilterType.Equals:
                    return (from tray in _oMCSDbContext.Trays.Where(p => p.Name.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text.Equals(textValue))
                            select tray);
            }

            return source;
        }

        #endregion Methods
    }
}
