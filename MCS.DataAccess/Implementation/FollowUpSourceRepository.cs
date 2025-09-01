using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class FollowUpSourceRepository : BaseLookupRepository<FollowUpSource>, IFollowUpSourceRepository
    {
        #region Attributes

        #endregion Attributes

        #region Constructors

        public FollowUpSourceRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddFollowUpSource(FollowUpSource  followUpSource)
        {
            try
            {
                followUpSource.IsActive = true;
                _oMCSDbContext.FollowUpSources.Add(followUpSource);

                _oMCSDbContext.SaveChanges();

                return followUpSource.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateFollowUpSource(FollowUpSource followUpSource)
        {
            try
            {
                FollowUpSource FollowUpSourceOld = GetFollowUpSourceById(followUpSource.Id);
                followUpSource.IsActive = FollowUpSourceOld.IsActive;
                if (FollowUpSourceOld != null)
                {
                    _oMCSDbContext.Entry(FollowUpSourceOld).CurrentValues.SetValues(FollowUpSourceOld);

                    foreach (Localization localization in followUpSource.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = FollowUpSourceOld.LocalizationIdentifier.Localizations
                         .Where(l => l.Id == localization.Id)
                         .FirstOrDefault();

                        _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                    }

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public FollowUpSource GetFollowUpSourceById(int FollowUpSourceId)
        {
            try
            {
                return this.FindBy(t => t.Id == FollowUpSourceId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteFollowUpSource(int id)
        {
            try
            {
                FollowUpSource followUpSource = _oMCSDbContext.FollowUpSources.Where(l => l.Id == id).FirstOrDefault();

                if (followUpSource != null)
                {
                    _oMCSDbContext.FollowUpSources.Remove(followUpSource);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckIfFollowUpSourceUsed(int FollowUpSourceId)
        {
            try
            {
                return (_oMCSDbContext.TransactionFollowUps.FirstOrDefault(l => l.Id== FollowUpSourceId) != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void LockUnlockLookup(int FollowUpSourceId, int userId)
        {
            try
            {
                var FollowUpSourcetoUpdate = _oMCSDbContext.FollowUpSources.FirstOrDefault(f => f.Id == FollowUpSourceId);
                if (FollowUpSourcetoUpdate != null)
                {
                    FollowUpSourcetoUpdate.IsLocked = !FollowUpSourcetoUpdate.IsLocked;

                    if (FollowUpSourcetoUpdate.IsLocked)
                    {
                        FollowUpSourcetoUpdate.LockedBy = userId;
                    }
                    else
                    {
                        FollowUpSourcetoUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int FollowUpSourceId)
        {
            try
            {
                var FollowUpSource = _oMCSDbContext.FollowUpSources.FirstOrDefault(f => f.Id == FollowUpSourceId);
                if (FollowUpSource != null)
                {
                    FollowUpSource.IsActive = !FollowUpSource.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<FollowUpSource> GetFollowUpSources(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<FollowUpSource> FollowUpSources = (from FollowUpSource in _oMCSDbContext.FollowUpSources
                                                                          select FollowUpSource);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(FollowUpSource).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            FollowUpSources = this.SortByText(FollowUpSources, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            FollowUpSources = this.SortByTransactionCategory(FollowUpSources, filter.Value);
                        }
                        else
                        {
                            FollowUpSources = WhereQuery(FollowUpSources, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = FollowUpSources.Where(l => !l.IsInternal).Count();

                if (searchCriteria.Ascending)
                {
                    FollowUpSources = FollowUpSources.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    FollowUpSources = FollowUpSources.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                return FollowUpSources.ToList().Select(tl => new FollowUpSource
                {
                    Id = tl.Id,
                    TransactionCategories = tl.TransactionCategories,
                    //Text = tl.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    IsInternal = tl.IsInternal,
                    IsActive = tl.IsActive,
                    IsLocked = tl.IsLocked,
                    LockedBy = tl.LockedBy,
                    LocalizationIdentifier = tl.LocalizationIdentifier
                }).ToList();
                 
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<FollowUpSource> GetFollowUpSources(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<FollowUpSource> FollowUpSources = (from FollowUpSource in _oMCSDbContext.FollowUpSources
                                                                     where (FollowUpSource.TransactionCategories.HasFlag(sourceTransactionType) && FollowUpSource.IsActive==true)
                                     select new
                                     {
                                         FollowUpSource.Id,
                                         FollowUpSource.TransactionCategories,
                                         FollowUpSource.LocalizationIdentifier.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         FollowUpSource.IsInternal
                                     }).ToList().Select(l => new FollowUpSource
                                     {
                                         Id = l.Id,
                                         TransactionCategories = l.TransactionCategories,
                                         Text = l.Text,
                                         IsInternal = l.IsInternal
                                     }).ToList();
                return FollowUpSources;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<FollowUpSource> SortByText(IQueryable<FollowUpSource> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from followUpSource in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select followUpSource);
                case FilterType.EndsWidth:
                    return (from followUpSource in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select followUpSource);
                case FilterType.StartsWith:
                    return (from followUpSource in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select followUpSource);
                case FilterType.Equals:
                    return (from followUpSource in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select followUpSource);
            }

            return source;
        }

        private IQueryable<FollowUpSource> SortByTransactionCategory(IQueryable<FollowUpSource> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from followUpSource in source
                    where ((int)followUpSource.TransactionCategories == value)
                    select followUpSource);
        }

        #endregion Methods
    }
}
