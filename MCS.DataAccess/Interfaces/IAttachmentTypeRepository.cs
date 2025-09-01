using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IAttachmentTypeRepository : IRepository<AttachmentType>
    {
        IList<AttachmentType> GetAttachmentTypes(SearchCriteria searchCriteria, out int rowsCount);
        IList<AttachmentType> GetAttachmentTypes(string cultureName);
        bool CheckIfAttachmentTypeUsed(int attachmnetTypeId);
        void UpdateAttachmentType(AttachmentType attachmentType);
        IList<AttachmentExtension> GetAttachmentExtentions(SearchCriteria searchCriteria, out int rowsCount);
        void LockUnlockLookup(int AttachmentTyped, int UserId);
        void ActiveDeactiveLookup(int AttachmentTypeId);
    }
}
