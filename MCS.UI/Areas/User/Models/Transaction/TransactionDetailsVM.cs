using System;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionDetailsVM
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string HijriDate { get; set; }
        public long Number { get; set; }
        public string TransactionSources { get; set; }
        public int TransactionTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? LetterTypeId { get; set; }
        public int PriorityId { get; set; }
        public int ConfidentialityId { get; set; }
        public string Subject { get;  set; }
        public string LetterType { get;  set; }
        public string Priority { get;  set; }
        public string Confidentiality { get;  set; }
        public string SourceType { get;  set; }
        public string Creator { get;  set; }
        public int Year { get; set; }
        public string CurrentUser { get; set; }
        public string LetterNumber { get; set; }
    }
}