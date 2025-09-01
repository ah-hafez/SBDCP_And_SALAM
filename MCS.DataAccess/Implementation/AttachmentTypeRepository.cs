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
    public class AttachmentTypeRepository : BaseLookupRepository<AttachmentType>, IAttachmentTypeRepository
    {
        #region Constructors

        public AttachmentTypeRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
        }

        #endregion Constructors

        #region Methods

        public void UpdateAttachmentType(AttachmentType attachmentType)
        {
            try
            {
                AttachmentType attachmentTypeOld = this.FindBy(a => a.Id == attachmentType.Id);
                attachmentType.IsActive = attachmentTypeOld.IsActive;
                _oMCSDbContext.Entry(attachmentTypeOld).CurrentValues.SetValues(attachmentType);

                foreach (Localization localization in attachmentType.LocalizationIdentifier.Localizations)
                {
                    Localization currentlocalization = attachmentTypeOld.LocalizationIdentifier.Localizations
                     .Where(l => l.Id == localization.Id)
                     .FirstOrDefault();

                    if (currentlocalization != null)
                    {
                        _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                    }
                }

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        //public void DeleteAttachmentType(int id)
        //{
        //    try
        //    {
        //        AttachmentType attachmentType = FindBy(a => a.Id == id);

        //        if (attachmentType != null)
        //        {
        //            _oMCSDbContext.Entry(attachmentType.LocalizationIdentifier).State = EntityState.Deleted;

        //            _oMCSDbContext.AttachmentTypes.Remove(attachmentType);

        //            _oMCSDbContext.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw DataAccessException.Translate(ex);
        //    }
        //}

        public IList<AttachmentType> GetAttachmentTypes(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<AttachmentType> attachmentTypes = (from attachmentType in _oMCSDbContext.AttachmentTypes
                                                              select attachmentType);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(AttachmentType).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            attachmentTypes = SortByText(attachmentTypes, filter.Value, filter.Type);
                        }

                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            attachmentTypes = SortByTransactionCategory(attachmentTypes, filter.Value);
                        }
                        else
                        {
                            attachmentTypes = WhereQuery(attachmentTypes, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = attachmentTypes.Count();

                if (searchCriteria.Ascending)
                {
                    attachmentTypes = attachmentTypes.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);

                }
                else
                {
                    attachmentTypes = attachmentTypes.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return attachmentTypes.ToList().Select(a => new AttachmentType
                {
                    Id = a.Id,
                    PrintBarcode = a.PrintBarcode,
                    Archivable = a.Archivable,
                    TransactionCategories = a.TransactionCategories,
                    Text = a.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    LocalizationIdentifier = a.LocalizationIdentifier,
                    IsActive = a.IsActive,
                    IsLocked = a.IsLocked,
                    LockedBy = a.LockedBy
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<AttachmentExtension> GetAttachmentExtentions(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<AttachmentExtension> attachmentExtentions = (from attachmentExtention in _oMCSDbContext.AttachmentExtensions
                                                                        select attachmentExtention);

                rowsCount = attachmentExtentions.Count();

                if (searchCriteria.Ascending)
                {
                    attachmentExtentions = attachmentExtentions.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);

                }
                else
                {
                    attachmentExtentions = attachmentExtentions.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return attachmentExtentions.ToList().Select(a => new AttachmentExtension
                {
                    Id = a.Id,
                    ExtensionName = a.ExtensionName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<AttachmentType> GetAttachmentTypes(string cultureName)
        {
            try
            {
                IList<AttachmentType> attachmentTypes = (from attachmentType in _oMCSDbContext.AttachmentTypes
                                                         where (attachmentType.IsActive == true)
                                                         select new
                                                         {
                                                             attachmentType.Id,
                                                             attachmentType.PrintBarcode,
                                                             attachmentType.Archivable,
                                                             attachmentType.TransactionCategories,
                                                             attachmentType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                         }).ToList().Select(a => new AttachmentType
                                                         {
                                                             Id = a.Id,
                                                             PrintBarcode = a.PrintBarcode,
                                                             Archivable = a.Archivable,
                                                             TransactionCategories = a.TransactionCategories,
                                                             Text = a.Text
                                                         }).ToList();
                return attachmentTypes;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckIfAttachmentTypeUsed(int attachmnetTypeId)
        {
            try
            {
                return (_oMCSDbContext.Attachments.FirstOrDefault(a => a.Type.Id == attachmnetTypeId) != null);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void LockUnlockLookup(int AttachmentTypeId, int userId)
        {
            try
            {
                var AttachmentTypeToUpdate = _oMCSDbContext.AttachmentTypes.FirstOrDefault(f => f.Id == AttachmentTypeId);
                if (AttachmentTypeToUpdate != null)
                {
                    AttachmentTypeToUpdate.IsLocked = !AttachmentTypeToUpdate.IsLocked;

                    if (AttachmentTypeToUpdate.IsLocked)
                    {
                        AttachmentTypeToUpdate.LockedBy = userId;
                    }
                    else
                    {
                        AttachmentTypeToUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int AttachmentTypeId)
        {
            try
            {
                var attachmentType = _oMCSDbContext.AttachmentTypes.FirstOrDefault(f => f.Id == AttachmentTypeId);
                if (attachmentType != null)
                {
                    attachmentType.IsActive = !attachmentType.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQueryable<AttachmentType> SortByText(IQueryable<AttachmentType> source, string textValue, FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from attachmentType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select attachmentType);
                case FilterType.EndsWidth:
                    return (from attachmentType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select attachmentType);
                case FilterType.StartsWith:
                    return (from attachmentType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select attachmentType);
                case FilterType.Equals:
                    return (from attachmentType in source.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select attachmentType);
            }

            return source;
        }

        private IQueryable<AttachmentType> SortByTransactionCategory(IQueryable<AttachmentType> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from attachmentType in source
                    where ((int)attachmentType.TransactionCategories == value)
                    select attachmentType);
        }

        #endregion Methods
    }
}
