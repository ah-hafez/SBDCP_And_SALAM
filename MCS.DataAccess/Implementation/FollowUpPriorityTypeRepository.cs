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
    public class FollowUpPriorityTypeRepository : BaseLookupRepository<FollowUpPriorityType>, IFollowUpPriorityTypeRepository
    {
        #region Attributes

        #endregion Attributes

        #region Constructors

        public FollowUpPriorityTypeRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddFollowUpPriorityType(FollowUpPriorityType  followUpPriorityType)
        {
            try
            {
                followUpPriorityType.IsActive = true;
                _oMCSDbContext.FollowUpPriorityTypes.Add(followUpPriorityType);

                _oMCSDbContext.SaveChanges();

                return followUpPriorityType.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateFollowUpPriorityType(FollowUpPriorityType followUpPriorityType)
        {
            try
            {
                FollowUpPriorityType FollowUpPriorityTypeOld = GetFollowUpPriorityTypeById(followUpPriorityType.Id);
                followUpPriorityType.IsActive = FollowUpPriorityTypeOld.IsActive;
                if (FollowUpPriorityTypeOld != null)
                {
                    _oMCSDbContext.Entry(FollowUpPriorityTypeOld).CurrentValues.SetValues(FollowUpPriorityTypeOld);

                    foreach (Localization localization in followUpPriorityType.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = FollowUpPriorityTypeOld.LocalizationIdentifier.Localizations
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

        public FollowUpPriorityType GetFollowUpPriorityTypeById(int FollowUpPriorityTypeId)
        {
            try
            {
                return this.FindBy(t => t.Id == FollowUpPriorityTypeId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteFollowUpPriorityType(int id)
        {
            try
            {
                FollowUpPriorityType followUpPriorityType = _oMCSDbContext.FollowUpPriorityTypes.Where(l => l.Id == id).FirstOrDefault();

                if (followUpPriorityType != null)
                {
                    _oMCSDbContext.FollowUpPriorityTypes.Remove(followUpPriorityType);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckIfFollowUpPriorityTypeUsed(int FollowUpPriorityTypeId)
        {
            try
            {
                return (_oMCSDbContext.TransactionFollowUps.FirstOrDefault(l => l.Id== FollowUpPriorityTypeId) != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void LockUnlockLookup(int FollowUpPriorityTypeId, int userId)
        {
            try
            {
                var FollowUpPriorityTypetoUpdate = _oMCSDbContext.FollowUpPriorityTypes.FirstOrDefault(f => f.Id == FollowUpPriorityTypeId);
                if (FollowUpPriorityTypetoUpdate != null)
                {
                    FollowUpPriorityTypetoUpdate.IsLocked = !FollowUpPriorityTypetoUpdate.IsLocked;

                    if (FollowUpPriorityTypetoUpdate.IsLocked)
                    {
                        FollowUpPriorityTypetoUpdate.LockedBy = userId;
                    }
                    else
                    {
                        FollowUpPriorityTypetoUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int FollowUpPriorityTypeId)
        {
            try
            {
                var FollowUpPriorityType = _oMCSDbContext.FollowUpPriorityTypes.FirstOrDefault(f => f.Id == FollowUpPriorityTypeId);
                if (FollowUpPriorityType != null)
                {
                    FollowUpPriorityType.IsActive = !FollowUpPriorityType.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<FollowUpPriorityType> GetFollowUpPriorityTypes(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<FollowUpPriorityType> FollowUpPriorityTypes = (from FollowUpPriorityType in _oMCSDbContext.FollowUpPriorityTypes
                                                                          select FollowUpPriorityType);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(FollowUpPriorityType).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            FollowUpPriorityTypes = this.SortByText(FollowUpPriorityTypes, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            FollowUpPriorityTypes = this.SortByTransactionCategory(FollowUpPriorityTypes, filter.Value);
                        }
                        else
                        {
                            FollowUpPriorityTypes = WhereQuery(FollowUpPriorityTypes, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = FollowUpPriorityTypes.Where(l => !l.IsInternal).Count();

                if (searchCriteria.Ascending)
                {
                    FollowUpPriorityTypes = FollowUpPriorityTypes.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    FollowUpPriorityTypes = FollowUpPriorityTypes.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                return FollowUpPriorityTypes.ToList().Select(tl => new FollowUpPriorityType
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

        public IList<FollowUpPriorityType> GetFollowUpPriorityTypes(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<FollowUpPriorityType> FollowUpPriorityTypes = (from FollowUpPriorityType in _oMCSDbContext.FollowUpPriorityTypes
                                                                     where (FollowUpPriorityType.TransactionCategories.HasFlag(sourceTransactionType) && FollowUpPriorityType.IsActive==true)
                                     select new
                                     {
                                         FollowUpPriorityType.Id,
                                         FollowUpPriorityType.TransactionCategories,
                                         FollowUpPriorityType.LocalizationIdentifier.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         FollowUpPriorityType.IsInternal
                                     }).ToList().Select(l => new FollowUpPriorityType
                                     {
                                         Id = l.Id,
                                         TransactionCategories = l.TransactionCategories,
                                         Text = l.Text,
                                         IsInternal = l.IsInternal
                                     }).ToList();
                return FollowUpPriorityTypes;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<FollowUpPriorityType> SortByText(IQueryable<FollowUpPriorityType> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from followUpPriorityType in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select followUpPriorityType);
                case FilterType.EndsWidth:
                    return (from followUpPriorityType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select followUpPriorityType);
                case FilterType.StartsWith:
                    return (from followUpPriorityType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select followUpPriorityType);
                case FilterType.Equals:
                    return (from followUpPriorityType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select followUpPriorityType);
            }

            return source;
        }

        private IQueryable<FollowUpPriorityType> SortByTransactionCategory(IQueryable<FollowUpPriorityType> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from followUpPriorityType in source
                    where ((int)followUpPriorityType.TransactionCategories == value)
                    select followUpPriorityType);
        }

        #endregion Methods
    }
}
