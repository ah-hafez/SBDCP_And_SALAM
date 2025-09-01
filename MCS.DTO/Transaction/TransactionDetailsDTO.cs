using System;

namespace MCS.DTO
{
    public  class TransactionDetailsDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string HijriDate { get; set; }
        public long Number { get; set; }
        public string TransactionsTypes { get; set; }//مصدر القيد//
        public string Subject { get; set; } //الموضوع//
        public int Year { get; set; } //السنة//
        public int TransactionCategoryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? LetterTypeId { get; set; }
        public int PriorityId { get; set; }
        public int ConfidentialityId { get; set; }
        public string LetterType { get; set; }
        public string Priority { get; set; }
        public string Confidentiality { get; set; }
        public string TransactionType { get; set; }
        public string Creator { get; set; }

        public int? RecordNumber { get; set; }
        public int? SideContactExternalEntityID { get; set; }
        public string NumberContact { get; set; }
        public string CurrentUser { get; set; }
        public int? privacyLevelId { get; set; }
        public string Privacy { get; set; }
        public string LetterNumber { get; set; }


        public string InboundNumber { get; set; } // transaction table 

        public string InboundDateH { get; set; }

        public string FromOrgUnit { get; set; }
        public string FromOrgUnitId { get; set; }
        public string ToOrgUnitId { get; set; }

        public string ReminderDate { get; set; }
        public int? ToUserId { get; set; }


    }
}
