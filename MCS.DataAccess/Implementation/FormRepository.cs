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
    public class FormRepository : BaseLookupRepository<Form>, IFormRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public FormRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddForm(Form form)
        {
            try
            {
                form.IsActive = true;
                _oMCSDbContext.Forms.Add(form);

                _oMCSDbContext.SaveChanges();

                return form.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateForm(Form form)
        {
            try
            {


                IList<FormDepartment> formDepartments =
                    _oMCSDbContext.FormDepartments.Where(d => d.FormId == form.Id).ToList();

                foreach (FormDepartment formDepartment in formDepartments)
                {
                    _oMCSDbContext.Entry(formDepartment).State = EntityState.Deleted;
                }
                if (form.Departments != null)
                {
                    foreach (FormDepartment formDepartment in form.Departments)
                    {
                        _oMCSDbContext.Entry(formDepartment).State = EntityState.Added;
                    }
                }

                Form formOld = GetFormById(form.Id);

                if (formOld != null)
                {
                    form.IsActive = formOld.IsActive;

                    _oMCSDbContext.Entry(formOld).CurrentValues.SetValues(form);

                    foreach (Localization localization in form.LocalizationIdentifier.Localizations)
                    {
                        Localization currentlocalization = formOld.LocalizationIdentifier.Localizations
                         .Where(l => l.Id == localization.Id)
                         .FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    formOld.FormContent = form.FormContent;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Form GetFormById(int formId)
        {
            try
            {
                return this.FindBy(f => f.Id == formId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public DocumentInfo GetContentByFormId(int formId)
        {
            try
            {
                Form form = _oMCSDbContext.Forms.Where(f => f.Id == formId).FirstOrDefault();

                if (form != null)
                {
                    return form.FormContent;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteForm(int id)
        {
            try
            {

                Form entityToDelete = _oMCSDbContext.Forms.Where(f => f.Id == id).FirstOrDefault();
                if (entityToDelete != null)
                {
                    if (entityToDelete.LocalizationIdentifier != null)
                    {
                        int localizationCount = entityToDelete.LocalizationIdentifier.Localizations.Count;
                        for (int i = 0; i < localizationCount; i++)
                        {
                            _oMCSDbContext.Entry(entityToDelete.LocalizationIdentifier.Localizations[0]).State = EntityState.Deleted;
                        }
                    }
                    _oMCSDbContext.Forms.Remove(entityToDelete);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Form> GetForms(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Form> forms = (from form in _oMCSDbContext.Forms
                                          select form);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(Form).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            forms = this.SortByText(forms, filter.Value, filter.Type);
                        }
                        else if (propertyInfo != null && typeof(TransactionCategories).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            forms = this.SortByTransactionCategory(forms, filter.Value);
                        }
                        else
                        {
                            forms = WhereQuery(forms, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }
                if (searchCriteria.OrgUnitId.HasValue && searchCriteria.OrgUnitId > 0)
                {
                    forms = forms.Where(x => x.Departments.Any(de => de.DepartmentId == searchCriteria.OrgUnitId));
                }

                rowsCount = forms.Count();

                if (searchCriteria.Ascending)
                {
                    forms = forms.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);

                }
                else
                {
                    forms = forms.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return forms.ToList().Select(f => new Form
                {
                    Id = f.Id,
                    TransactionCategories = f.TransactionCategories,
                    LocalizationIdentifier = f.LocalizationIdentifier,
                    Text = f.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    IsLocked = f.IsLocked,
                    LockedBy = f.LockedBy,
                    IsActive = f.IsActive
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Form> GetOrgUnitForms(int orgUnitId, string cultureName)
        {
            try
            {
                IList<Form> forms = _oMCSDbContext.FormDepartments
                    .Where(o => (o.DepartmentId == orgUnitId || o.DepartmentId == null) && o.Form.IsActive == true)
                                          .Select(formDepartment => new
                                          {
                                              formDepartment.Form.FormContent,
                                              formDepartment.Form.Id,
                                              formDepartment.Form.LocalizationIdentifier.Localizations.Where(cul => cul.Culture.ShortName == cultureName).FirstOrDefault().Text
                                          }).ToList().Select(f => new Form
                                          {
                                              Id = f.Id,
                                              FormContent = f.FormContent,
                                              Text = f.Text
                                          }).ToList();

                return forms;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void LockUnlockLookup(int formId, int userId)
        {
            try
            {
                var formtoUpdate = _oMCSDbContext.Forms.FirstOrDefault(f => f.Id == formId);
                if (formtoUpdate != null)
                {
                    formtoUpdate.IsLocked = !formtoUpdate.IsLocked;

                    if (formtoUpdate.IsLocked)
                    {
                        formtoUpdate.LockedBy = userId;
                    }
                    else
                    {
                        formtoUpdate.LockedBy = null;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActiveDeactiveLookup(int FormId)
        {
            try
            {
                var form = _oMCSDbContext.Forms.FirstOrDefault(f => f.Id == FormId);
                if (form != null)
                {
                    form.IsActive = !form.IsActive;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQueryable<Form> SortByText(IQueryable<Form> forms, string textValue, FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from form in forms.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select form);
                case FilterType.EndsWidth:
                    return (from form in forms.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.EndsWith(textValue))
                            select form);
                case FilterType.StartsWith:
                    return (from form in forms.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.StartsWith(textValue))
                            select form);
                case FilterType.Equals:
                    return (from form in forms.Where(p => p.LocalizationIdentifier.Localizations.FirstOrDefault().Text.Equals(textValue))
                            select form);
            }

            return forms;
        }

        private IQueryable<Form> SortByTransactionCategory(IQueryable<Form> source, string textValue)
        {
            int value = -1;

            if (!string.IsNullOrEmpty(textValue))
            {
                value = Convert.ToInt32(textValue);
            }

            return (from form in source
                    where ((int)form.TransactionCategories == value)
                    select form);
        }


        #endregion Methods
    }
}
