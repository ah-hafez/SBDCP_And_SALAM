using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TaskReportVM
    {



        [CustomRequired("User.Transaction.Link.TransactionTypeRequired")]
        public int TransactionCategory { get; set; }

        [CustomRequired("User.DateRequired")]
        [CustomDateTimeCompareAttribute("To", Operation.LessThanOrEqual, "User.InboundSearch.DateCompare")]
        public DateTime From { get; set; }

        [CustomRequired("User.DateRequired")]
        [CustomDateTimeCompareAttribute("From", Operation.GreaterThanOrEqual)]
        public DateTime To { get; set; }
        public List<int> ColumnsToGrid { get; set; }
        public int TotalCount { get; set; }
        public bool? IsPrint { get; set; }

        // public int TransactionStatusId { get; set; }
        public int TransactionTypeId { get; set; }
        public string ummalqura { get; set; }
        public string gregorian { get; set; }
        public CommonVM CommonVM { get; set; } = new CommonVM();


    }
}