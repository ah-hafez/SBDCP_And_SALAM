using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IAttachmentTypeBL
    {
        int AddAttachmentType(AttachmentType attachmentType);
        void UpdateAttachmentType(AttachmentType attachmentType);
        void DeleteAttachmentTypes(IList<int> ids, out IList<int> attachmentTypesCannotBeDeleted);
        AttachmentType GetAttachmentTypeById(int attachmentTypeId);
        IList<AttachmentType> GetAttachmentTypes(SearchCriteria searchCriteria, out int rowsCount);
        IList<AttachmentType> GetAttachmentTypes(TransactionCategories transactionCategories, string cultureName);
        IList<AttachmentExtension> GetAttachmentExtentions(SearchCriteria searchCriteria, out int rowsCount);
    }
}
