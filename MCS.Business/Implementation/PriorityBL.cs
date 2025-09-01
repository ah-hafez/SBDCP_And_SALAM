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
    public class PriorityBL : BaseBL, IPriorityBL
    {
        public int AddPriority(Priority priority)
        {
            try
            {
                IPriorityRepository priorityRepository = IoC.Resolve<PriorityRepository>();
                int priorityId = priorityRepository.AddPriority(priority);
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

        public void UpdatePriority(Priority priority)
        {
            try
            {
                IPriorityRepository priorityRepository = IoC.Resolve<PriorityRepository>();
                priorityRepository.UpdatePriority(priority);
                CacheHelper.Remove(CachedObjectsKey.Priorities,"ar");
                CacheHelper.Remove(CachedObjectsKey.Priorities,"en");

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

        public Priority GetPriorityById(int priorityId)
        {
            try
            {
                IPriorityRepository priorityRepository = IoC.Resolve<PriorityRepository>();
                return priorityRepository.Get(priorityId);
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
        public Priority GetPriorityById(SearchCriteria searchCriteria, int priorityId, out int PriorityExceptionsRowsCount)
        {
            try
            {
                IPriorityRepository priorityRepository = IoC.Resolve<PriorityRepository>();
                return priorityRepository.GetPriorityById(searchCriteria, priorityId, out PriorityExceptionsRowsCount);
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
        public void DeletePriorities(IList<int> ids, out IList<int> prioritiesCannotBeDeleted)
        {
            try
            {
                IPriorityRepository priorityRepository = IoC.Resolve<PriorityRepository>();
                IList<Transaction> transactions;

                prioritiesCannotBeDeleted = new List<int>();

                foreach (var id in ids)
                {
                    transactions = TransactionBL.GetTransactions(t => t.Priority.Id == id);

                    if (transactions.Count > 0)
                    {
                        prioritiesCannotBeDeleted.Add(id);

                        continue;
                    }
                    CacheHelper.Remove(CachedObjectsKey.Priorities, "ar");
                    CacheHelper.Remove(CachedObjectsKey.Priorities, "en");
                    priorityRepository.DeletePriority(id);
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

        public IList<Priority> GetPriorities(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IPriorityRepository priorityRepository = IoC.Resolve<PriorityRepository>();
                IList<Priority> priorities = priorityRepository.GetPriorities(searchCriteria, out rowsCount);
                return priorities;
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

        public IList<Priority> GetPriorities(TransactionCategories transactionCategories, string cultureName, int OrgUnitId , int UserId)
        {
            try
            {
                IList<Priority> priorities = CacheHelper.Get(CachedObjectsKey.Priorities, cultureName) as IList<Priority>;

                if (priorities == null)
                {
                    IPriorityRepository priorityRepository = IoC.Resolve<PriorityRepository>();
                    priorities = priorityRepository.GetPriorities(cultureName,OrgUnitId,UserId);
                    CacheHelper.Insert(CachedObjectsKey.Priorities, priorities, cultureName);
                }

                var result = (from p in priorities where (p.TransactionCategories & transactionCategories) != 0 select p);

                return result.ToList();
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
