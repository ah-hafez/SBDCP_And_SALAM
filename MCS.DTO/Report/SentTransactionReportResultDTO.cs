using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class SentTransactionReportResultDTO
    {
        public int TransactionId { get; set; }
        public int TransactionTypeId { get; set; }
        public int TransactionCategoryId { get; set; }
        public string TransactionCategoryText { get; set; }
        public string OrgUnitText { get; set; }
        public long Number { get; set; }
        public string ConfidentialityText { get; set; }
        public string PriorityText { get; set; }
        public string Subject { get; set; }
        public string FromEntityText { get; set; } 
        public string ToEntityText { get; set; }
        public string TransactionStatusText { get; set; }
        public string TransactionTypeText { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime AssignedDate { get; set; }
        public string TransactionElcOwner { get; set; }
        public string TransactionPhysicalOwner { get; set; }
        public bool Viewed { get; set; }


    }
}
