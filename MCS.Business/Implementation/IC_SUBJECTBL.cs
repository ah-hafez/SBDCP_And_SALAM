using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using IC_SUBJECT = MCS.Domain.IC_SUBJECT;
using MCS.Domain.IC;
using MCS.DTO;

namespace MCS.Business
{
    public class IC_SUBJECTBL : BaseBL, IIC_SUBJECTBL
    {
        int IIC_SUBJECTBL.AddIC_SUBJECT(IC_SUBJECT icSubject)
        {


            try
            {
                IIC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                return repository.AddIC_SUBJECT(icSubject);
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

        public void DeleteIC_SUBJECT(int id)
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                repository.DeleteIC_SUBJECT(id);
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

        public IList<IC_SUBJECT> GetIC_SUBJECS(SearchCriteria searchCriteria, out int rowsCount, string cultureName)
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                return repository.GetIC_SUBJECS(searchCriteria, out rowsCount, cultureName);
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

        public IList<IC_SUBJECT> GetAllIC_SUBJECS(string cultureName)
        {

            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                return repository.GetAllIC_SUBJECS(cultureName);
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

        public IC_SUBJECT GetIC_SUBJECTById(int Id)
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                return repository.GetIC_SUBJECTById(Id);
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
        public List<IC_CLASSIFICATION> GetClassificationTypes()
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                return repository.GetClassificationTypes();
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

        public int UpdateIC_SUBJECT(IC_SUBJECT icSubject)
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                return repository.UpdateIC_SUBJECT(icSubject);
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

        public IList<IC_SUBJECT> GetIC_SUBJECTByParentId(int? Id, string query)
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                return repository.GetIC_SUBJECTByParentId(Id, query);
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

        public int AddIC_SUBJECT_TRANSACTION(IC_SUBJECTTransactionDTO icSubjectDTO)
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();
                
                return repository.AddIC_SUBJECT_TRANSACTION(icSubjectDTO.TransactionId, icSubjectDTO.IcId, icSubjectDTO.Number, icSubjectDTO.Description,icSubjectDTO.CreatedBy);
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

        public void RemoveIC_SUBJECT_TRANSACTION(int transId, int ic_id)
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

                repository.RemoveIC_SUBJECT_TRANSACTION(transId, ic_id);
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

        public IC_SUBJECTS_TRANSACTION IC_GetTransaction(int transId)
        {
            try
            {
                IC_SUBJECTRepository repository = IoC.Resolve<IC_SUBJECTRepository>();

               return repository.IC_GetTransaction(transId);
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
