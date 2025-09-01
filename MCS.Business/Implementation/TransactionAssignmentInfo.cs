using System;

namespace MCS.Business
{
    public class TransactionAssignmentInfo
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public string FromUser { get; set; }
        public int? ToUserId { get; set; }
        public string ToUser { get; set; }
        public int? ActionId { get; set; }
        public string Action { get; set; }
        public int FromEntityId { get; set; }
        public string FromEntity { get; set; }
        public int ToEntityId { get; set; }
        public string ToEntity { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int TransactionId { get; set; }
        public bool HasCollaboration { get; set; }
        public bool IsLate { get; set; }
        public bool Viewed { get; set; }
        public string Description { get; set; }
    }
}
