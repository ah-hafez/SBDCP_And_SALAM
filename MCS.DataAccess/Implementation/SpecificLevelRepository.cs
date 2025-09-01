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
    public class SpecificLevelRepository : BaseLookupRepository<SpecificLevel>, ISpecificLevelRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public SpecificLevelRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddSpecificLevel(SpecificLevel specificLevel)
        {
            try
            {
                _oMCSDbContext.SpecificLevels.Add(specificLevel);

                _oMCSDbContext.SaveChanges();

                return specificLevel.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateSpecificLevel(SpecificLevel specificLevel)
        {
            try
            {
                SpecificLevel specificLevelOld = GetSpecificLevelById(specificLevel.Id);

                if (specificLevelOld != null)
                {
                    _oMCSDbContext.Entry(specificLevelOld).CurrentValues.SetValues(specificLevel);

                    foreach (Localization localization in specificLevel.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = specificLevelOld.LocalizationIdentifier.Localizations
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

        public SpecificLevel GetSpecificLevelById(int specificLevelId)
        {
            try
            {
                return FindBy(l => l.Id == specificLevelId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteSpecificLevel(int id)
        {
            try
            {
                SpecificLevel specificLevel = _oMCSDbContext.SpecificLevels.Where(l => l.Id == id).FirstOrDefault();

                if (specificLevel != null)
                {
                    _oMCSDbContext.SpecificLevels.Remove(specificLevel);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<SpecificLevel> GetSpecificLevels(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<SpecificLevel> specificLevels = (from specificLevel in _oMCSDbContext.SpecificLevels
                                                            select specificLevel);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(SpecificLevel).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            specificLevels = SortByText(specificLevels, filter.Value, filter.Type);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            specificLevels = SortByTransactionCategory(specificLevels, filter.Value);
                        }
                        else
                        {
                            specificLevels = WhereQuery(specificLevels, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = specificLevels.Count();

                if (searchCriteria.Ascending)
                {
                    specificLevels = specificLevels.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    specificLevels = specificLevels.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return specificLevels.ToList().Select(t => new SpecificLevel
                {
                    Id = t.Id,
                    TransactionCategories = t.TransactionCategories,
                    Text = t.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    LocalizationIdentifier = t.LocalizationIdentifier
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<SpecificLevel> GetSpecificLevels(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<SpecificLevel> specificLevels = (from specificLevel in _oMCSDbContext.SpecificLevels
                                                       where specificLevel.TransactionCategories.HasFlag(sourceTransactionType)
                                                 select new
                                                 {
                                                     specificLevel.Id,
                                                     specificLevel.TransactionCategories,
                                                     specificLevel.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                 }).ToList().Select(t => new SpecificLevel
                                                 {
                                                     Id = t.Id,
                                                     TransactionCategories = t.TransactionCategories,
                                                     Text = t.Text
                                                 }).ToList();
                return specificLevels;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<SpecificLevel> GetSpecificLevels(string cultureName)
        {
            try
            {
                IList<SpecificLevel> specificLevels = (from specificLevel in _oMCSDbContext.SpecificLevels
                                                       select new
                                                 {
                                                     specificLevel.Id,
                                                     specificLevel.TransactionCategories,
                                                     specificLevel.LocalizationIdentifier.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                 }).ToList().Select(l => new SpecificLevel
                                                 {
                                                     Id = l.Id,
                                                     TransactionCategories = l.TransactionCategories,
                                                     Text = l.Text
                                                 }).ToList();
                return specificLevels;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<SpecificLevel> SortByText(IQueryable<SpecificLevel> source, string textValue, FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from form in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select form);
                case FilterType.EndsWidth:
                    return (from form in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select form);
                case FilterType.StartsWith:
                    return (from form in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select form);
                case FilterType.Equals:
                    return (from form in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select form);
            }

            return source;
        }

        private IQueryable<SpecificLevel> SortByTransactionCategory(IQueryable<SpecificLevel> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from specificLevel in source
                    where ((int)specificLevel.TransactionCategories == value)
                    select specificLevel);
        }

        #endregion Methods
    }
}
