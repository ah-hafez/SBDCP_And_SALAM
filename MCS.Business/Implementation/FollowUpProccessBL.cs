using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class FollowUpProccessBL : BaseBL, IFollowUpProccessBL
    {
        public int AddFollowUpProccess(FollowUpProccess followUpProccess)
        {
            try
            {
                IFollowUpProccessRepository followUpProccessRepository = IoC.Resolve<IFollowUpProccessRepository>();
                var addfollowUpProccess = followUpProccessRepository.AddFollowUpProccess(followUpProccess);
                CacheHelper.Remove(CachedObjectsKey.FollowUpProccess, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpProccess, "en");
                return addfollowUpProccess;
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

        public void UpdateFollowUpProccess(FollowUpProccess followUpProccess)
        {
            try
            {
                IFollowUpProccessRepository followUpProccessRepository = IoC.Resolve<IFollowUpProccessRepository>();
                followUpProccessRepository.UpdateFollowUpProccess(followUpProccess);
                CacheHelper.Remove(CachedObjectsKey.FollowUpProccess, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpProccess, "en");
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

        public FollowUpProccess GetFollowUpProccessId(int FollowUpProccessId)
        {
            try
            {
                IFollowUpProccessRepository followUpProccessRepository = IoC.Resolve<IFollowUpProccessRepository>();
                return followUpProccessRepository.Get(FollowUpProccessId);
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

        public void DeleteFollowUpProccess(IList<int> ids, out IList<int> FollowUpProccessCannotBeDeleted)
        {
            try
            {
                IFollowUpProccessRepository followUpProccessRepository = IoC.Resolve<IFollowUpProccessRepository>();
                FollowUpProccessCannotBeDeleted = new List<int>();

                foreach (int id in ids)
                {
                    if (followUpProccessRepository.CheckIfFollowUpProccessUsed(id))
                    {
                        FollowUpProccessCannotBeDeleted.Add(id);

                        continue;
                    }
                    followUpProccessRepository.DeleteFollowUpProccess(id);
                }
                CacheHelper.Remove(CachedObjectsKey.FollowUpProccess, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpProccess, "en");
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

        public IList<FollowUpProccess> GetFollowUpProccess(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IFollowUpProccessRepository followUpProccessRepository = IoC.Resolve<IFollowUpProccessRepository>();
                return followUpProccessRepository.GetFollowUpProccesss(searchCriteria, out rowsCount);
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

        public IList<FollowUpProccess> GetFollowUpProccess(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<FollowUpProccess> followUpProccesss = CacheHelper.Get(CachedObjectsKey.FollowUpProccess, cultureName) as IList<FollowUpProccess>;
                if (followUpProccesss == null || followUpProccesss.Count == 0)
                {
                    IFollowUpProccessRepository followUpProccessRepository = IoC.Resolve<IFollowUpProccessRepository>();

                    followUpProccesss = followUpProccessRepository.GetFollowUpProccesss(transactionCategories, cultureName);

                    CacheHelper.Insert(CachedObjectsKey.FollowUpProccess, followUpProccesss, cultureName);
                }
                return followUpProccesss;
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
