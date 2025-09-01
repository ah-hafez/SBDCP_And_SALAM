using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    class PriorityExceptionBL : BaseBL, IPriorityExceptionBL
    {
        public int AddPriorityException(PriorityException priorityException)
        {
            try
            {
                IPriorityExceptionRepository priorityExceptionRepository = IoC.Resolve<PriorityExceptionRepository>();
                int priorityId = priorityExceptionRepository.AddPriorityException(priorityException);
                CacheHelper.Remove(CachedObjectsKey.Priorities, "ar");
                CacheHelper.Remove(CachedObjectsKey.Priorities, "en");

                return priorityId;
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

        public void DeletePriorityException(int priorityExceptionId)
        {
            try
            {
                IPriorityExceptionRepository priorityExceptionRepository = IoC.Resolve<PriorityExceptionRepository>();
                priorityExceptionRepository.DeletePriorityException(priorityExceptionId);
                CacheHelper.Remove(CachedObjectsKey.Priorities, "ar");
                CacheHelper.Remove(CachedObjectsKey.Priorities, "en");

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

        public PriorityException GetPriorityExceptionById(int priorityExceptionId)
        {
            try
            {
                IPriorityExceptionRepository priorityExceptionRepository = IoC.Resolve<PriorityExceptionRepository>();
                return priorityExceptionRepository.GetPriorityExceptionById(priorityExceptionId);
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

        public IList<PriorityException> GetPriorityExceptions(SearchCriteria searchCriteria, int priorityId, out int rowsCount)
        {
            try
            {
                IPriorityExceptionRepository priorityExceptionRepository = IoC.Resolve<PriorityExceptionRepository>();
                return priorityExceptionRepository.GetPriorityExceptions(searchCriteria, priorityId, out rowsCount);
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
        public PriorityException GetPriorityExceptionByPriorityId(int priorityId)
        {
            try
            {
                IPriorityExceptionRepository priorityExceptionRepository = IoC.Resolve<PriorityExceptionRepository>();
                return priorityExceptionRepository.GetPriorityExceptionByPriorityId(priorityId);
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

        public void UpdatePriorityException(PriorityException priorityException)
        {
            try
            {
                IPriorityExceptionRepository priorityExceptionRepository = IoC.Resolve<PriorityExceptionRepository>();
                priorityExceptionRepository.UpdatePriorityException(priorityException);
                CacheHelper.Remove(CachedObjectsKey.Priorities, "ar");
                CacheHelper.Remove(CachedObjectsKey.Priorities, "en");
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
