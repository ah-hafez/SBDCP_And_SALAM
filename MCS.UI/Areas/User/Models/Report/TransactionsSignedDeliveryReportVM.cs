using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionsSignedDeliveryReportVM
    {
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.Number")]
        //[CustomRequired("User.Report.TransactionsDeliveryReport.NumberRequired")]
        public int? Number { get; set; }
        [CustomDisplayName("User.Report.TransactionsDeliveryReport.OrgUnit")]
        public int? OrgUnitId { get; set; }

        //[CustomRequired("User.Report.TransactionsDeliveryReport.DateRequired")]
        public DateTime? Date { get; set; }
        //[CustomRequired("User.Transaction.Link.TransactionNumberRequired")]
        [CustomStringLength("User.Inbound.BasicInfo.SubjectLength", 12, 0)]
        [CustomRegularExpressionAttribute("^[0-9ء-ي//\\\\-]*$", "User.Transaction.InboundNumber")]
        public int? TransactionNumber { get; set; }
    }
}