using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class Attachment : EntityBase, IAuditable
    {
        public int TypeId { get; set; }
        public virtual AttachmentType  Type { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public virtual DocumentInfo DocumentInfo { get; set; }
        public AttachmentSource AttachmentSource { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
