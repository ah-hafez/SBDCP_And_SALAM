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
    public class AttachmentTypeBL : BaseBL, IAttachmentTypeBL
    {
        public int AddAttachmentType(AttachmentType attachmentType)
        {
            try
            {
                IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<IAttachmentTypeRepository>();
                attachmentType.IsActive = true;
                int attachmentTypeId = attachmentTypeRepository.Add(attachmentType);

                return attachmentTypeId;
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

        public void UpdateAttachmentType(AttachmentType attachmentType)
        {
            try
            {
                IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<IAttachmentTypeRepository>();
                attachmentTypeRepository.UpdateAttachmentType(attachmentType);
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

        public AttachmentType GetAttachmentTypeById(int attachmentTypeId)
        {
            try
            {
                IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<IAttachmentTypeRepository>();
                return attachmentTypeRepository.Get(attachmentTypeId);
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

        public void DeleteAttachmentTypes(IList<int> ids, out IList<int> attachmentTypesCannotBeDeleted)
        {
            try
            {
                IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<IAttachmentTypeRepository>();

                attachmentTypesCannotBeDeleted = new List<int>();

                foreach (int id in ids)
                {
                    if (attachmentTypeRepository.CheckIfAttachmentTypeUsed(id))
                    {
                        attachmentTypesCannotBeDeleted.Add(id);
                        continue;
                    }
                    attachmentTypeRepository.Delete(id);
                }
                CacheHelper.Remove(CachedObjectsKey.AttachmentTypes, "ar");
                CacheHelper.Remove(CachedObjectsKey.AttachmentTypes, "en");
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
        public IList<AttachmentExtension> GetAttachmentExtentions(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<IAttachmentTypeRepository>();

                return attachmentTypeRepository.GetAttachmentExtentions(searchCriteria, out rowsCount);
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

        public IList<AttachmentType> GetAttachmentTypes(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<IAttachmentTypeRepository>();

                return attachmentTypeRepository.GetAttachmentTypes(searchCriteria, out rowsCount);
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

        public IList<AttachmentType> GetAttachmentTypes(TransactionCategories transactionCategories, string cultureName)
        {
            try
            {
                IList<AttachmentType> attachmentTypes = CacheHelper.Get(CachedObjectsKey.AttachmentTypes, cultureName) as IList<AttachmentType>;

                if (attachmentTypes == null)
                {
                    IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<IAttachmentTypeRepository>();

                    attachmentTypes = attachmentTypeRepository.GetAttachmentTypes(cultureName);

                    CacheHelper.Insert(CachedObjectsKey.AttachmentTypes, attachmentTypes, cultureName);
                }

                return attachmentTypes.Where(a => a.TransactionCategories.HasFlag(transactionCategories)).ToList();
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
