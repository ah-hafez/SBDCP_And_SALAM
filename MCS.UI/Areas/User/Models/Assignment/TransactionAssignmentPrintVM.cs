using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Assignment
{
    public class TransactionAssignmentPrintVM
    {
        public List<TransactionAssignmentVM> TransactionAssignmentVM { get; set; }

        public string Subject { get; set; }
        public string Confedentiality { get; set; }
        public int ConfedentialityId { get; set; }
        public string PriorityLevel { get; set; }
        public int PriorityLevelId { get; set; }
        public string Number { get; set; } // transaction nnumber
        public string TransactionDateH { get; set; }
        public string Explanation { get; set; }
        public int TransactionId { get; set; }
        public string DateTimeNowG { get; set; }
        public string DateTimeNowH { get; set; }
        public string FromOrgUnit { get; set; }
        public string InboundNumber { get; set; } // transaction table 
        public bool Generalization { get; set; }
        public string InboundDateH { get; set; }

        public string ToOrgUnit { get; set; }
        public string ReminderDate { get; set; }

        public int ActionId { get; set; }
        public string ParentOrgUnit { get; set; }
        public int ExplanationPriority { get; set; }


    }
}