using System.Collections.Generic;
using MCS.Common;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionAttachmentVM : EntityBase
    {
        public int Id { get; set; }
        [CustomDisplayName("User.Transaction.Attachment.TypeId")]
        [CustomRequired("User.Transaction.Attachment.TypeIdRequired")]
        public int TypeId { get; set; } //النوع//
        public string TypeName { get; set; }
        //public List<LocalizationVM> Names { get; set; }
        [CustomDisplayName("User.Transaction.Attachment.Number")]
        [CustomRequired("User.Transaction.Attachment.NumberRequired")]
        [CustomRegularExpression("^[1-9][0-9]*$", "User.Transaction.Attachment.NumberGreaterThanZero")]
        public int Number { get; set; } //العدد//
        [CustomDisplayName("User.Transaction.Attachment.Attachments")]
        public string AttachmentName { get; set; } //المرفقات//
        public bool Archivable { get; set; }
        public DocumentVM DocumentVM { get; set; }
        public int AttachmentSource { get; set; }
        public List<TransactionAttachmentVM> Attachments { get; set; } = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAttachmentVM>(), 1, 0, false);
        public int? DocumentInfoId { get; set; }
        public string JFile { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
    }
}