using System.Collections.Generic;
using MCS.Common;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionArchiveVM : EntityBase
    {
        public TransactionArchiveVM()
        {

        }
        public TransactionArchiveVM(TransactionAttachmentType transactionAttachmentType, bool bIsMainDocument, bool bAnnotationOnly = false, bool bShowBarcode = true, bool bReadOnly = false,
                                    bool bStampPermission = false, bool bSignaturePermission = false, bool bAnnotationsPermission = false, bool bColorScanningPermission = false, bool bShowArchives = true)
        {
            TransactionAttachmentType = transactionAttachmentType;
            IsMainDocument = bIsMainDocument;
            AnnotationOnly = bAnnotationOnly;
            ShowBarcode = bShowBarcode;
            ReadOnly = bReadOnly;
            StampPermission = bStampPermission;
            SignaturePermission = bSignaturePermission;
            AnnotationsPermission = bAnnotationsPermission;
            ColorScanningPermission = bColorScanningPermission;
            IsShowArchives = bShowArchives;

        }
        public string Id { get; set; }

        public int DocumentId { get; set; }
        public string EncryptDocumentId { get; set; }

        public TransactionAttachmentType TransactionAttachmentType { get; set; }

        [CustomDisplayName("User.Transaction.Archive.AttachmentType")]
        public int? AttachmentTypeId { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }
        public bool IsMainDocument { get; set; }
        public string ArcivingTypeName { get; set; }
        public bool AnnotationOnly { get; set; }
        public bool ShowBarcode { get; set; }
        public bool ReadOnly { get; set; }
        public bool StampPermission { get; set; }
        public bool ColorScanningPermission { get; set; }
        public bool IsShowArchives { get; set; }
        public bool SignaturePermission { get; set; }
        public bool AnnotationsPermission { get; set; }
        [CustomDisplayName("User.Transaction.Attachment.Number")]
        [CustomRequired("User.Transaction.Attachment.NumberRequired")]
        [CustomRegularExpression("^[1-9][0-9]*$", "User.Transaction.Attachment.NumberGreaterThanZero")]
        public int Number { get; set; } //العدد//
        [CustomDisplayName("User.Transaction.Attachment.Attachments")]
        public string AttachmentName { get; set; } //المرفقات//
        public bool Archivable { get; set; }
        public int AttachmentSource { get; set; }
        public string FileName { get; set; }
        public int FromUserId { get; set; }
        public int FromEntityId { get; set; }
        public string JFile { get; set; }
        public bool IsNewFile { get; set; }
        public int UserId { get; set; }
        public List<TransactionArchiveVM> Archives { get; set; } = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionArchiveVM>(), 1, 0, false);
        public string Empty { get; set; }
        public string MimeType { get; set; }
    }
}