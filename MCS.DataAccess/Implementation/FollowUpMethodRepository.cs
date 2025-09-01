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
    public class FollowUpMethodRepository : BaseLookupRepository<FollowUpMethod>, IFollowUpMethodRepository
    {
        #region Attributes

        #endregion Attributes

        #region Constructors

        public FollowUpMethodRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddFollowUpMethod(FollowUpMethod  followUpMethod)
        {
            try
            {
                followUpMethod.IsActive = true;
                _oMCSDbContext.FollowUpMethods.Add(followUpMethod);

                _oMCSDbContext.SaveChanges();

                return followUpMethod.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateFollowUpMethod(FollowUpMethod followUpMethod)
        {
            try
            {
                FollowUpMethod FollowUpMethodOld = GetFollowUpMethodById(followUpMethod.Id);
                followUpMethod.IsActive = FollowUpMethodOld.IsActive;
                if (FollowUpMethodOld != null)
                {
                    _oMCSDbContext.Entry(FollowUpMethodOld).CurrentValues.SetValues(FollowUpMethodOld);

                    foreach (Localization localization in followUpMethod.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = FollowUpMethodOld.LocalizationIdentifier.Localizations
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

        public FollowUpMethod GetFollowUpMethodById(int FollowUpMethodId)
        {
            try
            {
                return this.FindBy(t => t.Id == FollowUpMethodId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteFollowUpMethod(int id)
        {
            try
            {
                FollowUpMethod followUpMethod = _oMCSDbContext.FollowUpMethods.Where(l => l.Id == id).FirstOrDefault();

                if (followUpMethod != null)
                {
                    _oMCSDbContext.FollowUpMethods.Remove(followUpMethod);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckIfFollowUpMethodUsed(int FollowUpMethodId)
        {
            try
            {
                return (_oMCSDbContext.TransactionFollowUps.FirstOrDefault(l => l.Id== FollowUpMethodId) != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void LockUnlockLookup(int FollowUpMethodId, int userId)
        {
            try
            {
                var FollowUpMethodtoUpdate = _oMCSDbContext.FollowUpMethods.FirstOrDefault(f => f.Id == FollowUpMethodId);
                if (FollowUpMethodtoUpdate != null)
                {
                    FollowUpMethodtoUpdate.IsLocked = !FollowUpMethodtoUpdate.IsLocked;

                    if (FollowUpMethodtoUpdate.IsLocked)
                    {
                        FollowUpMethodtoUpdate.LockedBy = userId;
                    }
                    else
                    {
                        FollowUpMethodtoUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int FollowUpMethodId)
        {
            try
            {
                var FollowUpMethod = _oMCSDbContext.FollowUpMethods.FirstOrDefault(f => f.Id == FollowUpMethodId);
                if (FollowUpMethod != null)
                {
                    FollowUpMethod.IsActive = !FollowUpMethod.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<FollowUpMethod> GetFollowUpMethods(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<FollowUpMethod> FollowUpMethods = (from FollowUpMethod in _oMCSDbContext.FollowUpMethods
                                                                          select FollowUpMethod);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(FollowUpMethod).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            FollowUpMethods = this.SortByText(FollowUpMethods, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            FollowUpMethods = this.SortByTransactionCategory(FollowUpMethods, filter.Value);
                        }
                        else
                        {
                            FollowUpMethods = WhereQuery(FollowUpMethods, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = FollowUpMethods.Where(l => !l.IsInternal).Count();

                if (searchCriteria.Ascending)
                {
                    FollowUpMethods = FollowUpMethods.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    FollowUpMethods = FollowUpMethods.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                return FollowUpMethods.ToList().Select(tl => new FollowUpMethod
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

        public IList<FollowUpMethod> GetFollowUpMethods(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<FollowUpMethod> FollowUpMethods = (from FollowUpMethod in _oMCSDbContext.FollowUpMethods
                                                                     where (FollowUpMethod.TransactionCategories.HasFlag(sourceTransactionType) && FollowUpMethod.IsActive==true)
                                     select new
                                     {
                                         FollowUpMethod.Id,
                                         FollowUpMethod.TransactionCategories,
                                         FollowUpMethod.LocalizationIdentifier.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         FollowUpMethod.IsInternal
                                     }).ToList().Select(l => new FollowUpMethod
                                     {
                                         Id = l.Id,
                                         TransactionCategories = l.TransactionCategories,
                                         Text = l.Text,
                                         IsInternal = l.IsInternal
                                     }).ToList();
                return FollowUpMethods;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<FollowUpMethod> SortByText(IQueryable<FollowUpMethod> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from followUpMethod in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select followUpMethod);
                case FilterType.EndsWidth:
                    return (from followUpMethod in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select followUpMethod);
                case FilterType.StartsWith:
                    return (from followUpMethod in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select followUpMethod);
                case FilterType.Equals:
                    return (from followUpMethod in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select followUpMethod);
            }

            return source;
        }

        private IQueryable<FollowUpMethod> SortByTransactionCategory(IQueryable<FollowUpMethod> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from followUpMethod in source
                    where ((int)followUpMethod.TransactionCategories == value)
                    select followUpMethod);
        }

        #endregion Methods
    }
}
