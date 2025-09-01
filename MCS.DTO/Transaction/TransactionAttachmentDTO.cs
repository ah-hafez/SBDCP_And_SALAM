using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionAttachmentDTO
    {
        public int Id { get; set; }

        //[CustomDisplayName("User.Transaction.Attachment.TypeId")]
        [CustomRequired("User.Transaction.Attachment.TypeIdRequired")]
        public int TypeId { get; set; } //النوع//

        public string TypeName { get; set; }

        //public List<LocalizationDTO> Names { get; set; }

        //[CustomDisplayName("User.Transaction.Attachment.Number")]
        [CustomRequired("User.Transaction.Attachment.NumberRequired")]
        [CustomRegularExpression("^[1-9][0-9]*$", "User.Transaction.Attachment.NumberGreaterThanZero")]
        public int Number { get; set; } //العدد//

        //[CustomDisplayName("User.Transaction.Attachment.Attachments")]
        public string Attachments { get; set; } //المرفقات//

        public bool Archivable { get; set; }
        
        public DocumentDTO DocumentDTO { get; set; }
        public string Description { get; set; }
        public int AttachmentSource { get; set; }
        public bool IsCopyAttachment { get; set; }
        public int UserId { get; set; }

    }
}