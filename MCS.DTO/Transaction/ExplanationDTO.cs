using MCS.Common;
using MCS.Common.CustomAttributes;
using System;

namespace MCS.DTO
{
    public class ExplanationDTO
    {
        public int Id { get; set; }
        public EditorType EditorType { get; set; }
        public DocumentDTO DocumentDTO { get; set; }
        public string ConfidentialityName { get; set; }

        [CustomRequired("User.Explanation.ConfidentialityRequired")]
        //[CustomDisplayName("User.Explanation.Confidentiality")]
        public int ConfidentialityId { get; set; }

        public string FromUser { get; set; }
        public string DateH { get; set; }
        public int FromUserId { get; set; }
        public bool CanBeDeleted { get; set; }
        public bool isCopies { get; set; }
        public long TransactionNumber { get; set; }
        public DateTime Date { get; set; }
        public int? DocumentId { get; set; }
        public int RowNumber { get; set; }
        public bool? CanBeSigned { get; set; }

    }
}
