using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class HubAttachment : EntityBase
    {
        public int TypeId { get; set; }
        public virtual AttachmentType Type { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public virtual DocumentInfo DocumentInfo { get; set; }
        public string ExternalAttachementId { get; set; }
        public string AttachementId { get; set; }
    }
}
