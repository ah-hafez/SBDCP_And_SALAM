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
    public class LinkRepository : BaseLookupRepository<Link>, ILinkRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public LinkRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddLink(Link link)
        {
            try
            {
                link.IsActive = true;
                _oMCSDbContext.Links.Add(link);

                _oMCSDbContext.SaveChanges();

                return link.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateLink(Link link)
        {
            try
            {
                Link linkOld = GetLinkById(link.Id);
                link.IsActive = linkOld.IsActive;
                if (linkOld != null)
                {
                    _oMCSDbContext.Entry(linkOld).CurrentValues.SetValues(link);

                    foreach (Localization localization in link.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = linkOld.LocalizationIdentifier.Localizations
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

        public Link GetLinkById(int linkId)
        {
            try
            {
                return this.FindBy(t => t.Id == linkId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteLink(int id)
        {
            try
            {
                Link link = _oMCSDbContext.Links.Where(l => l.Id == id).FirstOrDefault();

                if (link != null)
                {
                    _oMCSDbContext.Links.Remove(link);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckIfLinkTypeUsed(int linkTypeId)
        {
            try
            {
                return (_oMCSDbContext.TransactionLinks.FirstOrDefault(l => l.Type.Id == linkTypeId) != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void LockUnlockLookup(int LinkId, int userId)
        {
            try
            {
                var linktoUpdate = _oMCSDbContext.Links.FirstOrDefault(f => f.Id == LinkId);
                if (linktoUpdate != null)
                {
                    linktoUpdate.IsLocked = !linktoUpdate.IsLocked;

                    if (linktoUpdate.IsLocked)
                    {
                        linktoUpdate.LockedBy = userId;
                    }
                    else
                    {
                        linktoUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int LinkId)
        {
            try
            {
                var link = _oMCSDbContext.Links.FirstOrDefault(f => f.Id == LinkId);
                if (link != null)
                {
                    link.IsActive = !link.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<Link> GetLinks(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Link> Links = (from link in _oMCSDbContext.Links
                                          select link);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(Link).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            Links = this.SortByText(Links, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            Links = this.SortByTransactionCategory(Links, filter.Value);
                        }
                        else
                        {
                            Links = WhereQuery(Links, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = Links.Where(l => !l.IsInternal).Count();

                if (searchCriteria.Ascending)
                {
                    Links = Links.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    Links = Links.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return Links.ToList().Select(tl => new Link
                {
                    Id = tl.Id,
                    TransactionCategories = tl.TransactionCategories,
                    Text = tl.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
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

        public IList<Link> GetLinks(TransactionCategories sourceTransactionType, string cultureName)
        {
            try
            {
                IList<Link> links = (from link in _oMCSDbContext.Links
                                     where (link.TransactionCategories.HasFlag(sourceTransactionType) && link.IsActive==true)
                                     select new
                                     {
                                         link.Id,
                                         link.TransactionCategories,
                                         link.LocalizationIdentifier.Localizations.Where(loc => loc.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         link.IsInternal
                                     }).ToList().Select(l => new Link
                                     {
                                         Id = l.Id,
                                         TransactionCategories = l.TransactionCategories,
                                         Text = l.Text,
                                         IsInternal = l.IsInternal
                                     }).ToList();
                return links;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Link> SortByText(IQueryable<Link> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from transactionLink in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select transactionLink);
                case FilterType.EndsWidth:
                    return (from transactionLink in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select transactionLink);
                case FilterType.StartsWith:
                    return (from transactionLink in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select transactionLink);
                case FilterType.Equals:
                    return (from transactionLink in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select transactionLink);
            }

            return source;
        }

        private IQueryable<Link> SortByTransactionCategory(IQueryable<Link> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from link in source
                    where ((int)link.TransactionCategories == value)
                    select link);
        }

        #endregion Methods
    }
}
