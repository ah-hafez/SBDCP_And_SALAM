using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class FollowUpSourceBL : BaseBL, IFollowUpSourceBL
    {
        public int AddFollowUpSource(FollowUpSource followUpSource)
        {
            try
            {
                IFollowUpSourceRepository followUpSourceRepository = IoC.Resolve<IFollowUpSourceRepository>();
                var addfollowUpSource = followUpSourceRepository.AddFollowUpSource(followUpSource);
                CacheHelper.Remove(CachedObjectsKey.FollowUpSource, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpSource, "en");
                return addfollowUpSource;
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

        public void UpdateFollowUpSource(FollowUpSource followUpSource)
        {
            try
            {
                IFollowUpSourceRepository followUpSourceRepository = IoC.Resolve<IFollowUpSourceRepository>();
                followUpSourceRepository.UpdateFollowUpSource(followUpSource);
                CacheHelper.Remove(CachedObjectsKey.FollowUpSource, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpSource, "en");
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

        public FollowUpSource GetFollowUpSourceId(int FollowUpSourceId)
        {
            try
            {
                IFollowUpSourceRepository followUpSourceRepository = IoC.Resolve<IFollowUpSourceRepository>();
                return followUpSourceRepository.Get(FollowUpSourceId);
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

        public void DeleteFollowUpSource(IList<int> ids, out IList<int> FollowUpSourceCannotBeDeleted)
        {
            try
            {
                IFollowUpSourceRepository followUpSourceRepository = IoC.Resolve<IFollowUpSourceRepository>();
                FollowUpSourceCannotBeDeleted = new List<int>();

                foreach (int id in ids)
                {
                    if (followUpSourceRepository.CheckIfFollowUpSourceUsed(id))
                    {
                        FollowUpSourceCannotBeDeleted.Add(id);

                        continue;
                    }
                    followUpSourceRepository.DeleteFollowUpSource(id);
                }
                CacheHelper.Remove(CachedObjectsKey.FollowUpSource, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpSource, "en");
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

        public IList<FollowUpSource> GetFollowUpSources(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IFollowUpSourceRepository followUpSourceRepository = IoC.Resolve<IFollowUpSourceRepository>();
                return followUpSourceRepository.GetFollowUpSources(searchCriteria, out rowsCount);
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

        public IList<FollowUpSource> GetFollowUpSources(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<FollowUpSource> followUpSources = CacheHelper.Get(CachedObjectsKey.FollowUpSource, cultureName) as IList<FollowUpSource>;
                if (followUpSources == null || followUpSources.Count == 0)
                {
                    IFollowUpSourceRepository followUpSourceRepository = IoC.Resolve<IFollowUpSourceRepository>();

                    followUpSources = followUpSourceRepository.GetFollowUpSources(transactionCategories, cultureName);

                    CacheHelper.Insert(CachedObjectsKey.FollowUpSource, followUpSources, cultureName);
                }
                return followUpSources;
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
