using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class TaskReportResultDTO
    {
        public int TransactionId { get; set; }
        public int TransactionTypeId { get; set; }
        public int TransactionCategoryId { get; set; }
        public string TransactionCategoryText { get; set; }
        public DateTime Date { get; set; }
        public long Number { get; set; }
        public string ConfidentialityText { get; set; }
        public string PriorityText { get; set; }
        public string LetterTypeText { get; set; }    
        public string FromEntityText { get; set; }
        public string FromUserText { get; set; }
        public string ToEntityText { get; set; }
        public string ToUserText { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? RemindDate { get; set; }
        public string TransactionTypeText { get; set; }
        public int ToUserId { get; set; }
        public string TransactionStatusText { get; set; }
        public string NumberWithDate { get; set; }


    }
}
