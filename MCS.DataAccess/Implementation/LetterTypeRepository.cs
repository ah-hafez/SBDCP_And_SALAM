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
    public class LetterTypeRepository : BaseLookupRepository<LetterType>, ILetterTypeRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public LetterTypeRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddLetterType(LetterType letterType)
        {
            try
            {
                _oMCSDbContext.LetterTypes.Add(letterType);

                _oMCSDbContext.SaveChanges();

                return letterType.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateLetterType(LetterType letterType)
        {
            try
            {
                LetterType letterTypeOld = GetLetterTypeById(letterType.Id);

                if (letterTypeOld != null)
                {
                    _oMCSDbContext.Entry(letterTypeOld).CurrentValues.SetValues(letterType);

                    foreach (Localization localization in letterType.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = letterTypeOld.LocalizationIdentifier.Localizations
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

        public LetterType GetLetterTypeById(int letterTypeId)
        {
            try
            {
                return FindBy(l => l.Id == letterTypeId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteLetterType(int id)
        {
            try
            {
                LetterType letterType = _oMCSDbContext.LetterTypes.Where(l => l.Id == id).FirstOrDefault();

                if (letterType != null)
                {
                    _oMCSDbContext.LetterTypes.Remove(letterType);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<LetterType> GetLetterTypes(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<LetterType> letterTypes = (from letterType in _oMCSDbContext.LetterTypes
                                                      select letterType);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(LetterType).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            letterTypes = SortByText(letterTypes, filter.Value, filter.Type);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            letterTypes = SortByTransactionCategory(letterTypes, filter.Value);
                        }
                        else
                        {
                            letterTypes = WhereQuery(letterTypes, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = letterTypes.Count();

                if (searchCriteria.Ascending)
                {
                    letterTypes = letterTypes.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    letterTypes = letterTypes.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return letterTypes.ToList().Select(t => new LetterType
                {
                    Id = t.Id,
                    IsPopularization = t.IsPopularization,
                    TransactionCategories = t.TransactionCategories,
                    Text = t.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    LocalizationIdentifier = t.LocalizationIdentifier,
                    Notify = t.Notify,
                    WithExtraField = t.WithExtraField
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<LetterType> GetLetterTypes(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<LetterType> letterTypes = (from letterType in _oMCSDbContext.LetterTypes
                                                 where letterType.TransactionCategories.HasFlag(sourceTransactionType)
                                                 select new
                                                 {
                                                     letterType.Id,
                                                     letterType.IsPopularization,
                                                     letterType.TransactionCategories,
                                                     letterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                 }).ToList().Select(t => new LetterType
                                                 {
                                                     Id = t.Id,
                                                     IsPopularization = t.IsPopularization,
                                                     TransactionCategories = t.TransactionCategories,
                                                     Text = t.Text
                                                 }).ToList();
                return letterTypes;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<LetterType> GetLetterTypes(string cultureName)
        {
            try
            {
                IList<LetterType> letterTypes = (from letterType in _oMCSDbContext.LetterTypes
                                                 select new
                                                 {
                                                     letterType.Id,
                                                     letterType.IsPopularization,
                                                     letterType.TransactionCategories,
                                                     letterType.LocalizationIdentifier.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                 }).ToList().Select(l => new LetterType
                                                 {
                                                     Id = l.Id,
                                                     IsPopularization = l.IsPopularization,
                                                     TransactionCategories = l.TransactionCategories,
                                                     Text = l.Text
                                                 }).ToList();
                return letterTypes;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<LetterType> SortByText(IQueryable<LetterType> source, string textValue, FilterType filterType)
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

        private IQueryable<LetterType> SortByTransactionCategory(IQueryable<LetterType> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from letterType in source
                    where ((int)letterType.TransactionCategories == value)
                    select letterType);
        }

        #endregion Methods
    }
}
