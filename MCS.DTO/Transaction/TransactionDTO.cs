using System;
using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{
    [Serializable]

    [System.Web.Http.ModelBinding.ModelBinder(typeof(TransactionDTOBinder))]
    public abstract class TransactionDTO
    {

        public abstract TransactionCategory TransactionCategory { get; }
        public List<TransactionNameDTO> Names { get; set; }
        public List<TransactionLinkDTO> Links { get; set; }
        public List<TransactionAttachmentDTO> Attachments { get; set; }
        public List<TransactionExternalCopyDTO> ExternalCopies { get; set; }
        public DocumentDTO DocumentDTO { get; set; }
        public DocumentDTO OldDocumentDTO { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public DateTime RecordDate { get; set; }
        public String HijriRecordDate { get; set; }
        public int StatusId { get; set; }
        public bool IsSigned { get; set; }
        public UserProfileDTO FromUser { get; set; }
        public UserProfileDTO ToUser { get; set; }
        public int ProcessPeriodTransaction { get; set; }

        public int? RecordNumber { get; set; }
        //public int? SideContactExternalEntityID { get; set; }
        public string NumberContact { get; set; }
        public string ComplaintNumber { get; set; }
        public string FromOrgunitName { get; set; }
        public string SavedTransactionAssignment { get; set; }

    }
}
