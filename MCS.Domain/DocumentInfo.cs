using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class DocumentInfo : EntityBase, IAuditable
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string MimeType { get; set; }
        public bool IsDeleted { get; set; }
        public string ECMId { get; set; }
        public int? FromUserId { get; set; }
        public int? FromEntityId { get; set; }
        public virtual UserProfile FromUser { get; set; }
        public virtual OrgUnit FromEntity { get; set; }
        public virtual Document Document { get; set; }
        public int? TransactionId { get; set; }
        public int DocumentType { get; set; }
        //public bool IsDigitallySigned { get; set; }
    }
}
