using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class ConfidentialityAcknowledgmentBL : BaseBL, IConfidentialityAcknowledgmentsBL
    {
        public int AddConfidentialityAcknowledgments(ConfidentialityAcknowledgment ConfidentialityAcknowledgment)
        {
            try
            {
                IConfidentialityAcknowledgmentRepository ConfidentialityAcknowledgmentRepository = IoC.Resolve<IConfidentialityAcknowledgmentRepository>();
                ConfidentialityAcknowledgment.IsActive = true;
                int ConfidentialityAcknowledgmentId = ConfidentialityAcknowledgmentRepository.Add(ConfidentialityAcknowledgment);

                return ConfidentialityAcknowledgmentId;
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

        public void UpdateConfidentialityAcknowledgments(ConfidentialityAcknowledgment ConfidentialityAcknowledgment)
        {
            try
            {
                IConfidentialityAcknowledgmentRepository ConfidentialityAcknowledgmentRepository = IoC.Resolve<IConfidentialityAcknowledgmentRepository>();
                ConfidentialityAcknowledgmentRepository.UpdateConfidentialityAcknowledgment(ConfidentialityAcknowledgment);
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

        public ConfidentialityAcknowledgment GetConfidentialityAcknowledgmentsById(int ConfidentialityAcknowledgmentId)
        {
            try
            {
                IConfidentialityAcknowledgmentRepository ConfidentialityAcknowledgmentRepository = IoC.Resolve<IConfidentialityAcknowledgmentRepository>();
                return ConfidentialityAcknowledgmentRepository.Get(ConfidentialityAcknowledgmentId);
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

        public void DeleteConfidentialityAcknowledgments(IList<int> ids, out IList<int> ConfidentialityAcknowledgmentsCannotBeDeleted)
        {
            try
            {
                IConfidentialityAcknowledgmentRepository ConfidentialityAcknowledgmentRepository = IoC.Resolve<IConfidentialityAcknowledgmentRepository>();

                ConfidentialityAcknowledgmentsCannotBeDeleted = new List<int>();

                foreach (int id in ids)
                {
                    if (ConfidentialityAcknowledgmentRepository.CheckIfConfidentialityAcknowledgmentUsed(id))
                    {
                        ConfidentialityAcknowledgmentsCannotBeDeleted.Add(id);
                        continue;
                    }
                    ConfidentialityAcknowledgmentRepository.Delete(id);
                }
                CacheHelper.Remove(CachedObjectsKey.ConfidentialityAcknowledgments, "ar");
                CacheHelper.Remove(CachedObjectsKey.ConfidentialityAcknowledgments, "en");
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
 
        public IList<ConfidentialityAcknowledgment> GetConfidentialityAcknowledgments(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IConfidentialityAcknowledgmentRepository ConfidentialityAcknowledgmentRepository = IoC.Resolve<IConfidentialityAcknowledgmentRepository>();

                return ConfidentialityAcknowledgmentRepository.GetConfidentialityAcknowledgments(searchCriteria, out rowsCount);
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

        public IList<ConfidentialityAcknowledgment> GetConfidentialityAcknowledgments(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<ConfidentialityAcknowledgment> ConfidentialityAcknowledgments = CacheHelper.Get(CachedObjectsKey.ConfidentialityAcknowledgments, cultureName) as IList<ConfidentialityAcknowledgment>;

                if (ConfidentialityAcknowledgments == null)
                {
                    IConfidentialityAcknowledgmentRepository ConfidentialityAcknowledgmentRepository = IoC.Resolve<IConfidentialityAcknowledgmentRepository>();

                    ConfidentialityAcknowledgments = ConfidentialityAcknowledgmentRepository.GetConfidentialityAcknowledgments(cultureName);

                    CacheHelper.Insert(CachedObjectsKey.ConfidentialityAcknowledgments, ConfidentialityAcknowledgments, cultureName);
                }

                return ConfidentialityAcknowledgments.Where(a => a.TransactionCategories.HasFlag(transactionCategories)).ToList();
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
