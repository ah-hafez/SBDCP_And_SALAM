using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class FollowUpMethodBL : BaseBL, IFollowUpMethodBL
    {
        public int AddFollowUpMethod(FollowUpMethod followUpMethod)
        {
            try
            {
                IFollowUpMethodRepository followUpMethodRepository = IoC.Resolve<IFollowUpMethodRepository>();
                var addfollowUpMethod = followUpMethodRepository.AddFollowUpMethod(followUpMethod);
                CacheHelper.Remove(CachedObjectsKey.FollowUpMethod, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpMethod, "en");
                return addfollowUpMethod;
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

        public void UpdateFollowUpMethod(FollowUpMethod followUpMethod)
        {
            try
            {
                IFollowUpMethodRepository followUpMethodRepository = IoC.Resolve<IFollowUpMethodRepository>();
                followUpMethodRepository.UpdateFollowUpMethod(followUpMethod);
                CacheHelper.Remove(CachedObjectsKey.FollowUpMethod, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpMethod, "en");
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

        public FollowUpMethod GetFollowUpMethodId(int FollowUpMethodId)
        {
            try
            {
                IFollowUpMethodRepository followUpMethodRepository = IoC.Resolve<IFollowUpMethodRepository>();
                return followUpMethodRepository.Get(FollowUpMethodId);
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

        public void DeleteFollowUpMethod(IList<int> ids, out IList<int> FollowUpMethodCannotBeDeleted)
        {
            try
            {
                IFollowUpMethodRepository followUpMethodRepository = IoC.Resolve<IFollowUpMethodRepository>();
                FollowUpMethodCannotBeDeleted = new List<int>();

                foreach (int id in ids)
                {
                    if (followUpMethodRepository.CheckIfFollowUpMethodUsed(id))
                    {
                        FollowUpMethodCannotBeDeleted.Add(id);

                        continue;
                    }
                    followUpMethodRepository.DeleteFollowUpMethod(id);
                }
                CacheHelper.Remove(CachedObjectsKey.FollowUpMethod, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpMethod, "en");
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

        public IList<FollowUpMethod> GetFollowUpMethods(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IFollowUpMethodRepository followUpMethodRepository = IoC.Resolve<IFollowUpMethodRepository>();
                return followUpMethodRepository.GetFollowUpMethods(searchCriteria, out rowsCount);
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

        public IList<FollowUpMethod> GetFollowUpMethods(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<FollowUpMethod> followUpMethods = CacheHelper.Get(CachedObjectsKey.FollowUpMethod, cultureName) as IList<FollowUpMethod>;
                if (followUpMethods == null || followUpMethods.Count == 0)
                {
                    IFollowUpMethodRepository followUpMethodRepository = IoC.Resolve<IFollowUpMethodRepository>();

                    followUpMethods = followUpMethodRepository.GetFollowUpMethods(transactionCategories, cultureName);

                    CacheHelper.Insert(CachedObjectsKey.FollowUpMethod, followUpMethods, cultureName);
                }
                return followUpMethods;
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
