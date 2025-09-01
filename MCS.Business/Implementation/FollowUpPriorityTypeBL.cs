using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class FollowUpPriorityTypeBL : BaseBL, IFollowUpPriorityTypeBL
    {
        public int AddFollowUpPrioritytype(FollowUpPriorityType followUpPriorityType)
        {
            try
            {
                IFollowUpPriorityTypeRepository followUpPriorityTypeRepository = IoC.Resolve<IFollowUpPriorityTypeRepository>();
                var addfollowUpPriorityType = followUpPriorityTypeRepository.AddFollowUpPriorityType(followUpPriorityType);
                CacheHelper.Remove(CachedObjectsKey.FollowUpPriorityType, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpPriorityType, "en");
                return addfollowUpPriorityType;
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

        public void UpdateFollowUpPrioritytype(FollowUpPriorityType followUpPriorityType)
        {
            try
            {
                IFollowUpPriorityTypeRepository followUpPriorityTypeRepository = IoC.Resolve<IFollowUpPriorityTypeRepository>();
                followUpPriorityTypeRepository.UpdateFollowUpPriorityType(followUpPriorityType);
                CacheHelper.Remove(CachedObjectsKey.FollowUpPriorityType, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpPriorityType, "en");
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

        public FollowUpPriorityType GetFollowUpPrioritytypeId(int FollowUpPriorityTypeId)
        {
            try
            {
                IFollowUpPriorityTypeRepository followUpPriorityTypeRepository = IoC.Resolve<IFollowUpPriorityTypeRepository>();
                return followUpPriorityTypeRepository.Get(FollowUpPriorityTypeId);
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

        public void DeleteFollowUpPrioritytype(IList<int> ids, out IList<int> FollowUpPriorityTypeCannotBeDeleted)
        {
            try
            {
                IFollowUpPriorityTypeRepository followUpPriorityTypeRepository = IoC.Resolve<IFollowUpPriorityTypeRepository>();
                FollowUpPriorityTypeCannotBeDeleted = new List<int>();

                foreach (int id in ids)
                {
                    if (followUpPriorityTypeRepository.CheckIfFollowUpPriorityTypeUsed(id))
                    {
                        FollowUpPriorityTypeCannotBeDeleted.Add(id);

                        continue;
                    }
                    followUpPriorityTypeRepository.DeleteFollowUpPriorityType(id);
                }
                CacheHelper.Remove(CachedObjectsKey.FollowUpPriorityType, "ar");
                CacheHelper.Remove(CachedObjectsKey.FollowUpPriorityType, "en");
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

        public IList<FollowUpPriorityType> GetFollowUpPrioritytypes(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IFollowUpPriorityTypeRepository followUpPriorityTypeRepository = IoC.Resolve<IFollowUpPriorityTypeRepository>();
                return followUpPriorityTypeRepository.GetFollowUpPriorityTypes(searchCriteria, out rowsCount);
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

        public IList<FollowUpPriorityType> GetFollowUpPrioritytypes(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<FollowUpPriorityType> followUpPriorityTypes = CacheHelper.Get(CachedObjectsKey.FollowUpPriorityType, cultureName) as IList<FollowUpPriorityType>;
                if (followUpPriorityTypes == null || followUpPriorityTypes.Count == 0)
                {
                    IFollowUpPriorityTypeRepository followUpPriorityTypeRepository = IoC.Resolve<IFollowUpPriorityTypeRepository>();

                    followUpPriorityTypes = followUpPriorityTypeRepository.GetFollowUpPriorityTypes(transactionCategories, cultureName);

                    CacheHelper.Insert(CachedObjectsKey.FollowUpPriorityType, followUpPriorityTypes, cultureName);
                }
                return followUpPriorityTypes;
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
