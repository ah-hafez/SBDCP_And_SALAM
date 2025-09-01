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
    public class FollowUpProccessRepository : BaseLookupRepository<FollowUpProccess>, IFollowUpProccessRepository
    {
        #region Attributes

        #endregion Attributes

        #region Constructors

        public FollowUpProccessRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddFollowUpProccess(FollowUpProccess  followUpProccess)
        {
            try
            {
                followUpProccess.IsActive = true;
                _oMCSDbContext.FollowUpProccess.Add(followUpProccess);

                _oMCSDbContext.SaveChanges();

                return followUpProccess.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateFollowUpProccess(FollowUpProccess followUpProccess)
        {
            try
            {
                FollowUpProccess FollowUpProccessOld = GetFollowUpProccessById(followUpProccess.Id);
                followUpProccess.IsActive = FollowUpProccessOld.IsActive;
                if (FollowUpProccessOld != null)
                {
                    _oMCSDbContext.Entry(FollowUpProccessOld).CurrentValues.SetValues(FollowUpProccessOld);

                    foreach (Localization localization in followUpProccess.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = FollowUpProccessOld.LocalizationIdentifier.Localizations
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

        public FollowUpProccess GetFollowUpProccessById(int FollowUpProccessId)
        {
            try
            {
                return this.FindBy(t => t.Id == FollowUpProccessId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteFollowUpProccess(int id)
        {
            try
            {
                FollowUpProccess followUpProccess = _oMCSDbContext.FollowUpProccess.Where(l => l.Id == id).FirstOrDefault();

                if (followUpProccess != null)
                {
                    _oMCSDbContext.FollowUpProccess.Remove(followUpProccess);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckIfFollowUpProccessUsed(int FollowUpProccessId)
        {
            try
            {
                return (_oMCSDbContext.TransactionFollowUps.FirstOrDefault(l => l.Id== FollowUpProccessId) != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void LockUnlockLookup(int FollowUpProccessId, int userId)
        {
            try
            {
                var FollowUpProccesstoUpdate = _oMCSDbContext.FollowUpProccess.FirstOrDefault(f => f.Id == FollowUpProccessId);
                if (FollowUpProccesstoUpdate != null)
                {
                    FollowUpProccesstoUpdate.IsLocked = !FollowUpProccesstoUpdate.IsLocked;

                    if (FollowUpProccesstoUpdate.IsLocked)
                    {
                        FollowUpProccesstoUpdate.LockedBy = userId;
                    }
                    else
                    {
                        FollowUpProccesstoUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int FollowUpProccessId)
        {
            try
            {
                var FollowUpProccess = _oMCSDbContext.FollowUpProccess.FirstOrDefault(f => f.Id == FollowUpProccessId);
                if (FollowUpProccess != null)
                {
                    FollowUpProccess.IsActive = !FollowUpProccess.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<FollowUpProccess> GetFollowUpProccesss(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<FollowUpProccess> FollowUpProccesss = (from FollowUpProccess in _oMCSDbContext.FollowUpProccess
                                                                          select FollowUpProccess);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(FollowUpProccess).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            FollowUpProccesss = this.SortByText(FollowUpProccesss, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            FollowUpProccesss = this.SortByTransactionCategory(FollowUpProccesss, filter.Value);
                        }
                        else
                        {
                            FollowUpProccesss = WhereQuery(FollowUpProccesss, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = FollowUpProccesss.Where(l => !l.IsInternal).Count();

                if (searchCriteria.Ascending)
                {
                    FollowUpProccesss = FollowUpProccesss.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    FollowUpProccesss = FollowUpProccesss.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                return FollowUpProccesss.ToList().Select(tl => new FollowUpProccess
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

        public IList<FollowUpProccess> GetFollowUpProccesss(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<FollowUpProccess> FollowUpProccesss = (from FollowUpProccess in _oMCSDbContext.FollowUpProccess
                                                                     where (FollowUpProccess.TransactionCategories.HasFlag(sourceTransactionType) && FollowUpProccess.IsActive==true)
                                     select new
                                     {
                                         FollowUpProccess.Id,
                                         FollowUpProccess.TransactionCategories,
                                         FollowUpProccess.LocalizationIdentifier.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         FollowUpProccess.IsInternal
                                     }).ToList().Select(l => new FollowUpProccess
                                     {
                                         Id = l.Id,
                                         TransactionCategories = l.TransactionCategories,
                                         Text = l.Text,
                                         IsInternal = l.IsInternal
                                     }).ToList();
                return FollowUpProccesss;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<FollowUpProccess> SortByText(IQueryable<FollowUpProccess> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from followUpProccess in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select followUpProccess);
                case FilterType.EndsWidth:
                    return (from followUpProccess in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select followUpProccess);
                case FilterType.StartsWith:
                    return (from followUpProccess in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select followUpProccess);
                case FilterType.Equals:
                    return (from followUpProccess in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select followUpProccess);
            }

            return source;
        }

        private IQueryable<FollowUpProccess> SortByTransactionCategory(IQueryable<FollowUpProccess> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from followUpProccess in source
                    where ((int)followUpProccess.TransactionCategories == value)
                    select followUpProccess);
        }

        #endregion Methods
    }
}
