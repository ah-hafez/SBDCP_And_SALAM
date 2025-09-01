using MCS.UI.Areas.User.Models.Transaction;
using System.Collections.Generic;
using System.Drawing;

namespace MCS.UI.Areas.User.Models.Shared
{
    public class PrintAllVM
    {

        public string[] MainDocumentImages { get; set; }
        public List<PrintAttachVM> AttachmentDocumentImages { get; set; }
        public List<PrintExplanationVM> ExplanationDocumentImages { get; set; }

    }

    public class PrintAttachVM
    {
        public string[] AttachmentDocumentImages { get; set; }

        public TransactionAttachmentVM transactionAttachmentVM { get; set; }
    }

    public class PrintExplanationVM
    {
        public string[] ExplanationDocumentImages { get; set; }

        public ExplanationVM explanationVM { get; set; }
    }
}