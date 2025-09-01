using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class DashboardTransactionDetails
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int? LetterTypeId { get; set; }
        public string LetterType { get; set; }
        public int PriorityId { get; set; }
        public string Priority { get; set; }
        public int ConfidentialityId { get; set; }
        public string Confidentiality { get; set; }
        public int TransactionTypeId { get; set; }//مصدر القيد//
        public string Subject { get; set; } //الموضوع//
        public string TransactionType { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Creator { get; set; }
        public string CurrentUser { get; set; }
    }
}
