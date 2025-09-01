using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using System.Linq;

namespace MCS.Business
{
    public class LookupBL : BaseBL, ILookupBL
    {
        public Lookup GetLookupItem(int lookupId)
        {
            try
            {
                ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();
                return lookupRepository.Get(lookupId);
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

        public Lookup GetLookupItem(int lookupId, string cultureName)
        {
            try
            {
                ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();
                return lookupRepository.GetLookupItem(lookupId, cultureName);
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

        public IList<Lookup> GetLookupItems(LookupCategory lookupCategory, string cultureName)
        {
            try
            {
                IList<Lookup> lookups = CacheHelper.Get(CachedObjectsKey.Lookups + lookupCategory.ToString(), cultureName) as IList<Lookup>;

                if (lookups == null)
                {
                    ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();

                    lookups = lookupRepository.GetLookupItems((int)lookupCategory, cultureName);




                    CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
                }

                return lookups;
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
        public IList<Lookup> GetLookupItemsWithoutCach(LookupCategory lookupCategory, string cultureName)
        {
            try
            {
                ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();
                IList<Lookup> lookups = lookupRepository.GetLookupItems((int)lookupCategory, cultureName);

                if (lookups == null)
                {

                    lookups = lookupRepository.GetLookupItems((int)lookupCategory, cultureName);
                    //CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
                }

                return lookups;
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

        public IList<Lookup> GetActiveLookupItemsWithoutCach(LookupCategory lookupCategory, string cultureName)
        {
            try
            {
                ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();
                IList<Lookup> lookups = lookupRepository.GetActiveLookupItems((int)lookupCategory, cultureName);

                if (lookups == null)
                {

                    lookups = lookupRepository.GetActiveLookupItems((int)lookupCategory, cultureName);
                    //CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
                }

                return lookups;
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

        public int AddLookupItem(Lookup lookup)
        {
            try
            {
                ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();
                return lookupRepository.AddLookupItem(lookup);
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
        public void LockUnlockLookup(int lookupType, int lookUpId, int UserId)
        {
            switch ((LookupType)lookupType)
            {
                case LookupType.Form:
                    IFormRepository formRepository = IoC.Resolve<FormRepository>();
                    formRepository.LockUnlockLookup(lookUpId, UserId);
                    break;
                case LookupType.Link:
                    ILinkRepository linkRepository = IoC.Resolve<LinkRepository>();
                    linkRepository.LockUnlockLookup(lookUpId, UserId);
                    break;
                case LookupType.AttachmentType:
                    IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<AttachmentTypeRepository>();
                    attachmentTypeRepository.LockUnlockLookup(lookUpId, UserId);
                    break;
                case LookupType.Actions:
                    IActionRepository actionRepository = IoC.Resolve<ActionRepository>();
                    actionRepository.LockUnlockLookup(lookUpId, UserId);
                    break;
                case LookupType.FollowUpPriorityType:
                    IFollowUpPriorityTypeRepository FollowUpPriorityTypeRepository = IoC.Resolve<IFollowUpPriorityTypeRepository>();
                    FollowUpPriorityTypeRepository.LockUnlockLookup(lookUpId, UserId);
                    break;
                case LookupType.Correspondent:
                    ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
                    correspondentRepository.LockUnlockLookup(lookUpId, UserId);
                    break;
                
                default:
                    break;
            }
        }
        public void ActiveDeactiveLookup(int lookupType, int lookUpId)
        {
            switch ((LookupType)lookupType)
            {
                case LookupType.Form:
                    IFormRepository formRepository = IoC.Resolve<FormRepository>();
                    formRepository.ActiveDeactiveLookup(lookUpId);
                    break;
                case LookupType.Link:
                    ILinkRepository linkRepository = IoC.Resolve<LinkRepository>();
                    linkRepository.ActiveDeactiveLookup(lookUpId);
                    CacheHelper.Remove(CachedObjectsKey.Links, "ar");
                    CacheHelper.Remove(CachedObjectsKey.Links, "en");
                    break;
                case LookupType.AttachmentType:
                    IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<AttachmentTypeRepository>();
                    attachmentTypeRepository.ActiveDeactiveLookup(lookUpId);
                    CacheHelper.Remove(CachedObjectsKey.AttachmentTypes, "ar");
                    CacheHelper.Remove(CachedObjectsKey.AttachmentTypes, "en");
                    break;
                case LookupType.Actions:
                    IActionRepository actionRepository = IoC.Resolve<ActionRepository>();
                    actionRepository.ActiveDeactiveLookup(lookUpId);
                    CacheHelper.Remove(CachedObjectsKey.Actions, "ar");
                    CacheHelper.Remove(CachedObjectsKey.Actions, "en");
                    break;
                case LookupType.FollowUpPriorityType:
                    IFollowUpPriorityTypeRepository FollowUpPriorityTypeRepository = IoC.Resolve<IFollowUpPriorityTypeRepository>();
                    FollowUpPriorityTypeRepository.ActiveDeactiveLookup(lookUpId);
                    break;
                case LookupType.Correspondent:
                    ICorrespondentRepository correspondentRepository = IoC.Resolve<CorrespondentRepository>();
                    correspondentRepository.ActiveDeactiveLookup(lookUpId);
                    break;
                case LookupType.SaveReason:
                    ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();
                    lookupRepository.ActiveDeactiveLookup(lookUpId);
                    break;
                default:
                    break;
            }
        }

        public void UpdateLetterTypeNotifyOption(int letterTypeId, bool operationType)
        {
            ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();
            lookupRepository.UpdateLetterTypeNotifyOption(letterTypeId, operationType);
        }

        public void UpdateLetterTypeWithExtraFieldOption(int letterTypeId, bool operationType)
        {
            ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();
            lookupRepository.UpdateLetterTypeWithExtraFieldOption(letterTypeId, operationType);
        }
    }
}
