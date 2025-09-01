using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class FormBL : BaseBL, IFormBL
    {
        public int AddForm(Form form)
        {
            try
            {
                IFormRepository formRepository = IoC.Resolve<FormRepository>();

                return formRepository.AddForm(form);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateForm(Form form)
        {
            try
            {

                IFormRepository formRepository = IoC.Resolve<FormRepository>();
                formRepository.UpdateForm(form);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Form GetFormById(int formId)
        {
            try
            {
                IFormRepository formRepository = IoC.Resolve<FormRepository>();

                return formRepository.Get(formId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public DocumentInfo GetContentByFormId(int formId)
        {
            try
            {
                IFormRepository formRepository = IoC.Resolve<FormRepository>();

                return formRepository.GetContentByFormId(formId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void DeleteForms(IList<int> ids)
        {
            try
            {
                IFormRepository formRepository = IoC.Resolve<FormRepository>();

                foreach (var id in ids)
                {
                    formRepository.DeleteForm(id);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Form> GetForms(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IFormRepository formRepository = IoC.Resolve<FormRepository>();

                return formRepository.GetForms(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Form> GetOrgUnitForms(int OrgUnitId, string cultureName)
        {
            try
            {
                IFormRepository formRepository = IoC.Resolve<FormRepository>();

                return formRepository.GetOrgUnitForms(OrgUnitId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

    }
}
