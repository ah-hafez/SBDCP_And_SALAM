using MCS.Common;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Shared;
using System;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class ExplanationVM
    {
        public int Id { get; set; }
        public EditorType EditorType { get; set; }
        public DocumentVM DocumentVM { get; set; }
        public string Description { get; set; }
        public string ConfidentialityName { get; set; }

        [CustomRequired("User.Explanation.ConfidentialityRequired")]
        [CustomDisplayName("User.Explanation.Confidentiality")]
        public int ConfidentialityId { get; set; }

        public string FromUser { get; set; }
        public int FromUserId { get; set; }
        public bool CanBeDeleted { get; set; }
        public bool isCopies { get; set; }
        public long TransactionNumber { get; set; }
        public DateTime Date { get; set; }
        public string Key { get; set; }
        public int? DocumentId { get; set; }
        public string DateH { get; set; }
        public int RowNumber { get; set; }
        public string FileName { get; set; }
        public bool CanBeSigned { get; set; } = false;


    }
}