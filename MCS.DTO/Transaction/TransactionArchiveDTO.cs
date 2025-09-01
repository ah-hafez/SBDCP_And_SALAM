using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionArchiveDTO
    {


        public string Id { get; set; }

        public int DocumentId { get; set; }

        public TransactionAttachmentType TransactionAttachmentType { get; set; }

        //[CustomDisplayName("User.Transaction.Archive.AttachmentType")]
        public int? AttachmentTypeId { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }
        public bool IsMainDocument{ get; set; }

        public string ArcivingTypeName{ get; set; }

    }
}
